namespace LoxInterpreter.Tests;

[Collection("RequireConsole")]
public class ControlFlowTests
{
    private const string t = "True\r\n";
    private const string f = "False\r\n";

    [Theory]
    /// If Else
    [InlineData("1\r\n", "print 1;")]
    [InlineData(t, "if (true) print true;")]
    [InlineData("", "if (false) print true;")]
    [InlineData(t, "if (true) print true; else print false;")]
    [InlineData(f, "if (false) print true; else print false;")]
    [InlineData(t, "if (true) { var a = true; print a; } else { var b = false; print b; }")]
    [InlineData(f, "if (false) { var a = true; print a; } else { var b = false; print b; }")]
    [InlineData(t, "if (1 < 2) print true; else print false;")]
    [InlineData(f, "if (1 > 2) print true; else print false;")]
    /// And
    [InlineData(t, "print true and true;")]
    [InlineData(f, "print false and true;")]
    [InlineData(f, "print true and false;")]
    [InlineData(t, "print true and true and true;")]
    [InlineData(f, "print false and true and true;")]
    [InlineData(f, "print true and false and true;")]
    [InlineData(f, "print true and true and false;")]
    /// Or
    [InlineData(t, "print true or false;")]
    [InlineData(t, "print false or true;")]
    [InlineData(f, "print false or false;")]
    [InlineData(t, "print true or false or false;")]
    [InlineData(t, "print false or true or false;")]
    [InlineData(t, "print false or false or true;")]
    [InlineData(f, "print false or false or false;")]
    /// While loop
    [InlineData("0\r\n1\r\n2\r\n", "var a = 0; while (a < 3) { print a; a = a + 1; }")]
    [InlineData("", "while (false) print test;")]
    [InlineData(t, "var a = true; while (a) { print a; a = false; }")]
    /// For loop
    [InlineData("0\r\n1\r\n2\r\n", "for (var a = 0; a <= 2; a = a + 1) print a;")]
    [InlineData("0\r\n2\r\n4\r\n", "for (var b = 0; b < 5; b = b + 2) print b;")]
    public static void Test(string expected, string input)
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

    [Fact]
    public void WhileNested() => Test(
        t + t, @"
        var a = true;
        while (a) {
            print (a);
            var b = true;
            while (b) {
                print (b);
                b = false;
            }
            a = false;
        }"
    );

    [Fact]
    public void ForNested() => Test(
        "0 0\r\n0 1\r\n0 2\r\n1 0\r\n1 1\r\n1 2\r\n", @"
        for (var i = 0; i < 2; i = i + 1) { 
            for (var j = 0; j < 3; j = j + 1) {
                print i + "" "" + j;
            }
        }"
    );
}
