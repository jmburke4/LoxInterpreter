using System;

namespace LoxInterpreter;

public class Return(object? value) : Exception
{
    private readonly object? value = value;

    public object? Value => value;
}
