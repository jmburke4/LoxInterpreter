namespace LoxInterpreter;

public partial class Interpreter
{
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

        return function.Call(this, args) ?? "nil";
    }

    public object? VisitExpressionStmt(Stmt.Expression stmt)
    {
        Evaluate(stmt.Expr);
        return null;
    }

    public object? VisitFunctionStmt(Stmt.Function stmt)
    {
        LoxFunction function = new(stmt, environment);
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
}
