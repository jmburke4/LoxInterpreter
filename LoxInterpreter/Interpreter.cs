namespace LoxInterpreter;

/// <summary>
/// Evaluates expression trees and manages memory throughout the lifetime of the program.
/// </summary>
/// <param name="errorHandler">The <see cref="ErrorHandler"/> to use</param>
public partial class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor
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
        Globals.Define("indexof", new Indexof());
        Globals.Define("strat", new Strat());
        Globals.Define("strlen", new Strlen());
        Globals.Define("substring", new Substring());

        environment = Globals;
        ErrorHandler = errorHandler;
    }

    /// <summary>
    /// Checks if a value is a number type.
    /// </summary>
    /// <param name="op">The token preceding the operand</param>
    /// <param name="operand">The value to check</param>
    /// <exception cref="RuntimeError"></exception>
    private static void CheckNumberOperand(Token op, object operand) => CheckNumberOperand(op, 1.0, operand);

    /// <inheritdoc cref="CheckNumberOperand"/>
    private static void CheckNumberOperand(Token op, object left, object right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(op, $"Operand must be a number. \"{left}({left.GetType()}) - {right}({right.GetType()})\"");
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
