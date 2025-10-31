namespace LoxInterpreter.Tests;

public class ParserTests
{
    [Fact]
    public void Expression()
    {
        string input = "(5 - (3 - 1.8)) + -1;";
        string expected = "(+ (group (- 5 (group (- 3 1.8)))) (- 1))";
        var errorHandler = new ErrorHandler();

        Scanner scanner = new(errorHandler, input);
        scanner.ScanTokens();

        Parser parser = new(errorHandler, scanner.Tokens);
        var stmt = (Stmt.Expression)parser.Parse().First();

        Assert.NotNull(stmt);
        Assert.Equal(expected, new AstPrinter().Print(stmt.Expr));
    }
}
