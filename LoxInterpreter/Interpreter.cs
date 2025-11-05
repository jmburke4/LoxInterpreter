namespace LoxInterpreter;

/// <summary>
/// Evaluates expression trees and manages memory throughout the lifetime of the program.
/// </summary>
/// <param name="errorHandler">The <see cref="ErrorHandler"/> to use</param>
public class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object?>
{
    /// <summary>
    /// A reference to the current scope to evaluate within.
    /// </summary>
    private Environment environment;

    /// <summary>
    /// The <see cref="ErrorHandler"/> instance to report syntax and runtime errors to the user. 
    /// </summary>
    public readonly ErrorHandler ErrorHandler;

    /// <summary>
    /// A fixed reference to the global environment.
    /// </summary>
    public readonly Environment Globals;

    /// <summary>
    /// Constructs an instance of an interpreter object.
    /// </summary>
    /// <param name="errorHandler"></param>
    public Interpreter(ErrorHandler errorHandler)
    {
        Globals = new();
        Globals.Define("clock", new Clock());

        environment = Globals;
        ErrorHandler = errorHandler;
    }

    /// <summary>
    /// Checks if a value is a number type.
    /// </summary>
    /// <param name="op">The token preceding the operand</param>
    /// <param name="operand">The value to check</param>
    /// <exception cref="RuntimeError"></exception>
    private static void CheckNumberOperand(Token op, object operand)
    {
        if (operand is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    /// <inheritdoc cref="CheckNumberOperand"/>
    private static void CheckNumberOperand(Token op, object left, object right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    /// <summary>
    /// Checks if two objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare</param>
    /// <param name="right">The second object to compare</param>
    /// <returns>True or False</returns>
    private static bool Equal(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// Associates an object with a truth value.
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <returns>True or False</returns>
    private static bool Truthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool b) return b;
        return true;
    }

    /// <summary>
    /// Visits an <see cref="Expr"/>.
    /// </summary>
    /// <param name="expr"></param>
    /// <returns>The evaluated expression</returns>
    private object Evaluate(Expr expr) => expr.Accept(this);

    /// <summary>
    /// Visits a <see cref="Stmt"/>. 
    /// </summary>
    /// <param name="stmt"></param>
    private void Execute(Stmt stmt) => stmt.Accept(this);

    /// <summary>
    /// Iterates through a list of <see cref="Stmt"/> executing them. 
    /// </summary>
    /// <param name="statements">The statements to execute</param>
    /// <param name="env">The environment to evaluate in</param>
    public void ExecuteBlock(List<Stmt> statements, Environment env)
    {
        Environment previous = environment;
        try
        {
            environment = env;
            foreach (var stmt in statements)
            {
                Execute(stmt);
            }
        }
        finally
        {
            environment = previous;
        }
    }

    /// <summary>
    /// A helper method retained for running xUnit tests.
    /// </summary>
    /// <param name="expr">An expression to evaluate</param>
    /// <returns>The evaluated expression</returns>
    public object Interpret(Expr expr)
    {
        try
        {
            object val = Evaluate(expr);
            return val;
        }
        catch (RuntimeError ex)
        {
            ErrorHandler.RuntimeError(ex);
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
        }
        return "nil";
    }

    /// <summary>
    /// Evaluates a list of statements.
    /// </summary>
    /// <param name="stmts"></param>
    public void Interpret(List<Stmt> stmts)
    {
        try
        {
            foreach (var stmt in stmts)
            {
                Execute(stmt);
            }
        }
        catch (RuntimeError ex)
        {
            ErrorHandler.RuntimeError(ex);
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
        }
    }

    #region Visitors

    public object VisitAssignExpr(Expr.Assign expr)
    {
        object val = Evaluate(expr.Value);
        environment.Assign(expr.Name, val);
        return val;
    }

    public object VisitBinaryExpr(Expr.Binary expr)
    {
        object left = Evaluate(expr.Left);
        object right = Evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case TokenType.GREATER:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left > (double)right;

            case TokenType.GREATER_EQUAL:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left >= (double)right;

            case TokenType.LESS:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left < (double)right;

            case TokenType.LESS_EQUAL:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left <= (double)right;

            case TokenType.BANG_EQUAL:
                return !Equal(left, right);

            case TokenType.EQUAL_EQUAL:
                return Equal(left, right);

            case TokenType.MINUS:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left - (double)right;

            case TokenType.SLASH:
                CheckNumberOperand(expr.Operator, left, right);
                if ((double)right != 0) return (double)left / (double)right;
                else throw new RuntimeError(expr.Operator, $"Divide by zero error. \"{left}/{right}\"");

            case TokenType.STAR:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left * (double)right;

            case TokenType.PLUS:
                if (left is double && right is double) return (double)left + (double)right;
                if (left is string || right is string) return left.ToString() + right.ToString();
                throw new RuntimeError(expr.Operator, $"Operands must be two numbers or two strings or a number and a string. \"{left}\"+\"{right}\"");
        }

        throw new RuntimeError(expr.Operator, $"Unexpected token in VisitBinaryExpr(). \"{expr.Operator.Lexeme}\"");
    }

    public object? VisitBlockStmt(Stmt.Block stmt)
    {
        ExecuteBlock(stmt.Statements, new Environment(environment));
        return null;
    }

    public object VisitCallExpr(Expr.Call expr)
    {
        object callee = Evaluate(expr.Callee);

        List<object> args = [];
        foreach (var arg in expr.Arguments)
        {
            args.Add(Evaluate(arg));
        }

        if (!callee.GetType().GetInterfaces().Contains(typeof(ILoxCallable)))
            throw new RuntimeError(expr.Paren, "Can only call functions and classes.");

        ILoxCallable function = (ILoxCallable)callee;

        if (args.Count != function.Arity())
            throw new RuntimeError(expr.Paren,
            $"Expected {function.Arity()} arguments but got {args.Count}.");

        return function.Call(this, args);
    }

    public object? VisitExpressionStmt(Stmt.Expression stmt)
    {
        Evaluate(stmt.Expr);
        return null;
    }

    public object? VisitFunctionStmt(Stmt.Function stmt)
    {
        LoxFunction function = new(stmt);
        environment.Define(stmt.Name.Lexeme, function);
        return null;
    }

    public object VisitGroupingExpr(Expr.Grouping expr) => Evaluate(expr.Expression);

    public object? VisitIfStmt(Stmt.If stmt)
    {
        if (Truthy(Evaluate(stmt.Condition))) Execute(stmt.ThenBranch);
        else if (stmt.ElseBranch != null) Execute(stmt.ElseBranch);
        return null;
    }

    public object VisitLiteralExpr(Expr.Literal expr) => expr.Value ?? new Expr.Literal(null);

    public object VisitLogicalExpr(Expr.Logical expr)
    {
        object left = Evaluate(expr.Left);

        if (expr.Operator.Type == TokenType.OR)
        {
            if (Truthy(left)) return left;
        }
        else
        {
            if (!Truthy(left)) return left;
        }

        return Evaluate(expr.Right);
    }

    // This is a void function, but C# does not allow void type Ts
    public object? VisitPrintStmt(Stmt.Print stmt)
    {
        Console.WriteLine(Evaluate(stmt.Expr));
        return null;
    }

    public object? VisitReturnStmt(Stmt.Return stmt)
    {
        object? val = null;
        if (stmt.Value != null) val = Evaluate(stmt.Value);

        throw new Return(val);
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        object right = Evaluate(expr.Right);

        switch (expr.Operator.Type)
        {
            case TokenType.BANG:
                return !Truthy(right);

            case TokenType.MINUS:
                CheckNumberOperand(expr.Operator, right);
                return -(double)right;
        }

        throw new RuntimeError(expr.Operator, $"Unexpected token in VisitUnaryExpr(). \"{expr.Operator.Lexeme}\"");
    }

    public object VisitVariableExpr(Expr.Variable expr) => environment.Get(expr.Name) ?? "nil";

    public object? VisitVarStmt(Stmt.Var stmt)
    {
        object? val = null;
        if (stmt.Initializer != null) val = Evaluate(stmt.Initializer);
        environment.Define(stmt.Name.Lexeme, val);
        return null;
    }

    public object? VisitWhileStmt(Stmt.While stmt)
    {
        while (Truthy(Evaluate(stmt.Condition)))
        {
            Execute(stmt.Body);
        }
        return null;
    }

    #endregion
}

/// <summary>
/// Custom exception for denoting runtime errors.
/// </summary>
/// <param name="token">The token near where the exception occurred</param>
/// <param name="message">The error message</param>
public class RuntimeError(Token token, string message) : Exception(message)
{
    public Token Token { get; } = token;
}
