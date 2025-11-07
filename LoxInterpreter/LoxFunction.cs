namespace LoxInterpreter;

public class LoxFunction(Stmt.Function declaration, Environment closure) : ILoxCallable
{
    private readonly Environment closure = closure;

    private readonly Stmt.Function declaration = declaration;

    public int Arity() => declaration.Params.Count;

    public object? Call(Interpreter interpreter, List<object> arguments)
    {
        Environment env = new(closure);

        for (int i = 0; i < declaration.Params.Count; i++)
        {
            env.Define(declaration.Params.ElementAt(i).Lexeme, arguments.ElementAt(i));
        }

        try
        {
            interpreter.ExecuteBlock(declaration.Body, env);
        }
        catch (Return rtn)
        {
            return rtn.Value;
        }

        return null;
    }

    public override string ToString() => $"<fn {declaration.Name.Lexeme}>";
}
