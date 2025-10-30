namespace LoxInterpreter;

public class Environment
{
    // See also OrderedDictionary which maintains keys ordered by the sequence they are added in
    private readonly Dictionary<string, object?> Values = [];

    public void Assign(Token name, object? value)
    {
        if (Values.ContainsKey(name.Lexeme)) Values[name.Lexeme] = value;
        else throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Define(string name, object? value) => Values[name] = value;

    public object? Get(Token name)
    {
        if (Values.ContainsKey(name.Lexeme)) return Values.GetValueOrDefault(name.Lexeme);
        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }
}
