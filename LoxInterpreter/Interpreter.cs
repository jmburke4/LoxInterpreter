namespace LoxInterpreter;

public class Interpreter(ErrorHandler errorHandler) : IVisitor<object>
{
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
        if (obj is bool) return (bool)obj;
        return true;
    }

    private object Evaluate(Expr expr)
    {
        return expr.Accept(this);
    }

    public void Interpret(Expr expr)
    {
        try
        {
            object val = Evaluate(expr);
            Console.WriteLine(val ?? "nil");
        }
        catch (RuntimeError ex)
        {
            ErrorHandler.Exception(ex);
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
        }
    }

    public object VisitBinaryExpr(Binary expr)
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

    public object VisitGroupingExpr(Grouping expr) => Evaluate(expr.Expression);

    public object VisitLiteralExpr(Literal expr)
    {
        // This is stupid
        return expr.Value ?? new Literal(null);
    }

    public object VisitUnaryExpr(Unary expr)
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
}

public class RuntimeError : Exception
{
    public Token Token { get; }

    public RuntimeError(Token token, string message) : base(message)
    {
        Token = token;
    }

    public RuntimeError(Token token, string message, Exception inner) : base(message, inner)
    {
        Token = token;
    }
}
