namespace LoxInterpreter;

public class LoxFunction(Stmt.Function declaration) : ILoxCallable
{
    private readonly Stmt.Function declaration = declaration;

    public int Arity() => declaration.Params.Count;

    public object? Call(Interpreter interpreter, List<object> arguments)
    {
        Environment env = new(interpreter.Globals);

        for (int i = 0; i < declaration.Params.Count; i++)
        {
            env.Define(declaration.Params.ElementAt(i).Lexeme, arguments.ElementAt(i));
        }

        interpreter.ExecuteBlock(declaration.Body, env);
        return null;
    }

    public override string ToString() => $"<fn {declaration.Name.Lexeme}>";
}
