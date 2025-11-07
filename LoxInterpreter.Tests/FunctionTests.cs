namespace LoxInterpreter.Tests;

[Collection("RequireConsole")]
public class FunctionTests
{
    [Theory]
    [InlineData("3\r\n", "fun add(a, b) { print a + b; } add(1, 2);")]
    [InlineData("3\r\n", "fun add(a, b) { return a + b; } print add(1, 2);")]
    [InlineData("3\r\n", "var a = 3; fun msg() { print a; } msg();")]
    [InlineData("3\r\n", "var a = 2; fun msg(a) { print a; } msg(3);")]
    public void Test(string expected, string input)
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
    public void Counter()
    {
        string input = @"
        fun makeCounter() {
            var i = 0;
            fun count() {
                i = i + 1;
                print i;
            }

            return count;
        }

        var counter = makeCounter();
        counter();
        counter();
        ";

        string expected = "1\r\n2\r\n";

        Test(expected, input);
    }
}
