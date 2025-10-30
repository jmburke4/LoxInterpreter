namespace LoxInterpreter;

public class Interpreter(ErrorHandler errorHandler) : Expr.IVisitor<object>, Stmt.IVisitor<object?>
{
    private readonly Environment environment = new();

    public readonly ErrorHandler ErrorHandler = errorHandler;

    private static void CheckNumberOperand(Token op, object operand)
    {
        if (operand is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    private static void CheckNumberOperand(Token op, object left, object right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    private static bool Equal(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }

    private static bool Truthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool b) return b;
        return true;
    }

    private object Evaluate(Expr expr) => expr.Accept(this);

    private void Execute(Stmt stmt)
    {
        stmt.Accept(this);
    }

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
                // Where are divide by zero errors handled?
                CheckNumberOperand(expr.Operator, left, right);
                if ((double)right != 0) return (double)left / (double)right;
                else throw new RuntimeError(expr.Operator, $"Divide by zero error. \"{left}/{right}\"");
            case TokenType.STAR:
                CheckNumberOperand(expr.Operator, left, right);
                return (double)left * (double)right;
            case TokenType.PLUS:
                // Do I support ints?
                if (left is double && right is double) return (double)left + (double)right;
                if (left is string && right is string) return (string)left + (string)right;
                throw new RuntimeError(expr.Operator, $"Operands must be two numbers or two strings. \"{left}\"+\"{right}\"");
        }

        throw new RuntimeError(expr.Operator, $"Unexpected token in VisitBinaryExpr(). \"{expr.Operator.Lexeme}\"");
    }

    public object? VisitExpressionStmt(Stmt.Expression stmt)
    {
        Evaluate(stmt.Expr);
        return null;
    }

    public object VisitGroupingExpr(Expr.Grouping expr) => Evaluate(expr.Expression);

    public object VisitLiteralExpr(Expr.Literal expr) => expr.Value ?? new Expr.Literal(null);

    // This is a void function, but C# does not allow void type Ts
    public object? VisitPrintStmt(Stmt.Print stmt)
    {
        Console.WriteLine(Evaluate(stmt.Expr));
        return null;
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
}

public class RuntimeError(Token token, string message) : Exception(message)
{
    public Token Token { get; } = token;
}
