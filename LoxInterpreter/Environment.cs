namespace LoxInterpreter;

/// <summary>
/// An environment holds variables and function definitions for the lifetime of a Lox program.
/// </summary>
/// <param name="enclosing">The optional environment that encloses this environment.</param>
public class Environment(Environment? enclosing = null)
{
    /// <summary>
    /// Maps identifiers to values.
    /// </summary>
    private readonly Dictionary<string, object?> Values = [];

    /// <summary>
    /// A reference the environment that is up one scope level.
    /// </summary>
    private readonly Environment? Enclosing = enclosing;

    /// <summary>
    /// Assigns a value to a previously declared variable.
    /// </summary>
    /// <param name="name">The name of the identifier</param>
    /// <param name="value">The new value to assign</param>
    /// <exception cref="RuntimeError"></exception>
    public void Assign(Token name, object? value)
    {
        if (Values.ContainsKey(name.Lexeme)) Values[name.Lexeme] = value;
        else if (Enclosing != null) Enclosing.Assign(name, value);
        else throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    /// <summary>
    /// Declares and assigns a value to a variable.
    /// </summary>
    /// <param name="name">The identifier of the variable</param>
    /// <param name="value">The value of the variable</param>
    public void Define(string name, object? value) => Values[name] = value;

    /// <summary>
    /// The lookup method retrieving the value of a variable.
    /// </summary>
    /// <param name="name">The identifier</param>
    /// <returns>The value of the variable</returns>
    /// <exception cref="RuntimeError"></exception>
    public object? Get(Token name)
    {
        if (Values.ContainsKey(name.Lexeme)) return Values.GetValueOrDefault(name.Lexeme);
        if (Enclosing != null) return Enclosing.Get(name);
        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }
}
