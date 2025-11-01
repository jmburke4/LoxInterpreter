namespace LoxInterpreter.Tests;

public class ControlFlowTests
{
    private const string t = "True\r\n";
    private const string f = "False\r\n";

    private static void Test(string expected, string input)
    {
        ErrorHandler handler = new();

        Scanner scanner = new(handler, input);
        scanner.ScanTokens();

        Parser parser = new(handler, scanner.Tokens);
        var stmts = parser.Parse();

        // Scanning or parsing error
        if (handler.HadError || stmts.Count < 1) Assert.Fail();

        var sw = new StringWriter();
        var original = Console.Out;
        Console.SetOut(sw);

        Interpreter interpreter = new(handler);
        interpreter.Interpret(stmts);

        Console.SetOut(original);

        Assert.Equal(expected, sw.ToString());
    }

    /// <summary>
    /// If Else
    /// </summary>

    [Fact]
    public void TestStringWriter() => Test("1\r\n", "print 1;");

    [Fact]
    public void TrueIf() => Test(t, "if (true) print true;");

    [Fact]
    public void FalseIf() => Test(string.Empty, "if (false) print true;");

    [Fact]
    public void TrueIfElse() => Test(t, "if (true) print true; else print false;");

    [Fact]
    public void FalseIfElse() => Test(f, "if (false) print true; else print false;");

    [Fact]
    public void TrueIfElseBlock() =>
    Test(t, "if (true) { var a = true; print a; } else { var b = false; print b; }");

    [Fact]
    public void FalseIfElseBlock() =>
    Test(f, "if (false) { var a = true; print a; } else { var b = false; print b; }");

    [Fact]
    public void TrueExpression() => Test(t, "if (1 < 2) print true; else print false;");

    [Fact]
    public void FalseExpression() => Test(f, "if (1 > 2) print true; else print false;");

    /// <summary>
    /// And
    /// </summary>

    [Fact]
    public void TrueAnd() => Test(t, "print true and true;");

    [Fact]
    public void FalseAnd() => Test(f, "print false and true;");

    [Fact]
    public void FalseAnd2() => Test(f, "print true and false;");

    [Fact]
    public void TrueAndChain() => Test(t, "print true and true and true;");

    [Fact]
    public void FalseAndChain() => Test(f, "print false and true and true;");

    [Fact]
    public void FalseAndChain2() => Test(f, "print true and false and true;");

    [Fact]
    public void FalseAndChain3() => Test(f, "print true and true and false;");

    /// <summary>
    /// Or
    /// </summary>

    [Fact]
    public void TrueOr() => Test(t, "print true or false;");

    [Fact]
    public void TrueOr2() => Test(t, "print false or true;");

    [Fact]
    public void FalseOr() => Test(f, "print false or false;");

    [Fact]
    public void TrueOrChain() => Test(t, "print true or false or false;");

    [Fact]
    public void TrueOrChain2() => Test(t, "print false or true or false;");

    [Fact]
    public void TrueOrChain3() => Test(t, "print false or false or true;");

    [Fact]
    public void FalseOrChain() => Test(f, "print false or false or false;");

    /// <summary>
    /// While
    /// </summary>

    [Fact]
    public void WhileIncrementA() =>
    Test("0\r\n1\r\n2\r\n", "var a = 0; while (a < 3) { print a; a = a + 1; }");

    [Fact]
    public void WhileFalse() => Test(string.Empty, "while (false) print test;");

    [Fact]
    public void WhileTrue() => Test(t, "var a = true; while (a) { print a; a = false; }");
}
