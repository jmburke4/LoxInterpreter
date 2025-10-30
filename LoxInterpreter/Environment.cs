namespace LoxInterpreter;

public class Environment(Environment? enclosing = null)
{
    // See also OrderedDictionary which maintains keys ordered by the sequence they are added in
    private readonly Dictionary<string, object?> Values = [];

    private readonly Environment? Enclosing = enclosing;

    public void Assign(Token name, object? value)
    {
        if (Values.ContainsKey(name.Lexeme)) Values[name.Lexeme] = value;
        else if (Enclosing != null) Enclosing.Assign(name, value);
        else throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Define(string name, object? value) => Values[name] = value;

    public object? Get(Token name)
    {
        if (Values.ContainsKey(name.Lexeme)) return Values.GetValueOrDefault(name.Lexeme);
        if (Enclosing != null) return Enclosing.Get(name);
        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }
}
