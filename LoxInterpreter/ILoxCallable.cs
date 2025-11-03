namespace LoxInterpreter;

public interface ILoxCallable
{
    public int Arity();

    public object Call(Interpreter interpreter, List<object> arguments);
}

internal class Clock : ILoxCallable
{
    public int Arity() => 0;

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
    }
    
    public override string ToString() => "<native fn>";
}
    