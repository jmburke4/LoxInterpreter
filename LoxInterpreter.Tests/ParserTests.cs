namespace LoxInterpreter.Tests;

public class ParserTests
{
    [Fact]
    public void Expression()
    {
        string input = "(5 - (3 - 1.8)) + -1";
        string expected = "(+ (group (- 5 (group (- 3 1.8)))) (- 1))";
        var errorHandler = new ErrorHandler();

        Scanner scanner = new Scanner(errorHandler, input);
        scanner.ScanTokens();

        Parser parser = new Parser(errorHandler, scanner.Tokens);
        Expr? expr = parser.Parse();

        Assert.NotNull(expr);
        Assert.Equal(expected, new AstPrinter().Print(expr));
    }
}
