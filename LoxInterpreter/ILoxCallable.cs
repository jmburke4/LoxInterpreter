namespace LoxInterpreter;

/// <summary>
/// Provides an interface for the implementation of native and user defined functions.
/// </summary>
public interface ILoxCallable
{
    /// <summary>
    /// The number of parameters the function expects.
    /// </summary>
    /// <returns>An integer</returns>
    public int Arity();

    /// <summary>
    /// Calls the defined function.
    /// </summary>
    /// <param name="interpreter">The interpreter to execute the function with</param>
    /// <param name="arguments">The arguments to supply to the call</param>
    /// <returns>A nullable object</returns>
    public object? Call(Interpreter interpreter, List<object> arguments);
}

/// <summary>
/// A native function that returns the number of seconds since 1970-01-01T00:00:00.000Z.
/// </summary>
internal class Clock : ILoxCallable
{
    public int Arity() => 0;

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
    }

    public override string ToString() => "<native fn clock>";
}

/// <summary>
/// Returns the index of the first occurence of the second parameter in the first parameter.
/// </summary>
internal class Indexof : ILoxCallable
{
    public int Arity() => 2;

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        string str = (string)arguments[0];
        string target = (string)arguments[1];
        return (double)str.IndexOf(target);
    }

    public override string ToString() => "<native fn indexof>";
}

/// <summary>
/// Returns the string element in the string array in the first parameter at the index in
/// the second parameter.
/// </summary>
internal class Strat : ILoxCallable
{
    public int Arity() => 2;

    /// Lox usage: strat("ab cde fghi", 2) => "fghi"
    public object Call(Interpreter interpreter, List<object> arguments)
    {
        string element = ((string)arguments[0]).Trim().Split(' ')[(int)(double)arguments[1]];
        return double.TryParse(element, out double rtn) ? rtn : element;
    }

    public override string ToString() => "<native fn strat>";
}

/// <summary>
/// Returns the length of the string passed as the argument.
/// </summary>
internal class Strlen : ILoxCallable
{
    public int Arity() => 1;

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        string str = (string)arguments[0];
        return (double)str.Length;
    }

    public override string ToString() => "<native fn strlen>";
}

/// <summary>
/// Returns a substring from the first argument with the starting index of the second
/// argument with the length of the third argument.
/// </summary>
internal class Substring : ILoxCallable
{
    public int Arity() => 3;

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        string str = (string)arguments[0];
        double start = (double)arguments[1];
        double len = (double)arguments[2];
        return str.Substring((int)start, (int)len);
    }

    public override string ToString() => "<native fn substring>";
}
