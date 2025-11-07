namespace LoxInterpreter.Tests;

/// <summary>
/// See https://github.com/munificent/craftinginterpreters/blob/master/test/precedence.lox
/// </summary>
public class ExpressionTests
{
    [Theory]
    /// Order of Operations
    [InlineData(14, "2 + 3 * 4;")]
    [InlineData(8, "20 - 3 * 4;")]
    [InlineData(4, "2 + 6 / 3;")]
    [InlineData(0, "2 - 6 / 3;")]
    [InlineData(4, "(2 * (6 - (2 + 2)));")]
    /// Comparison precedence
    [InlineData(true, "false == 2 < 1;")]
    [InlineData(true, "false == 1 > 2;")]
    [InlineData(true, "false == 2 <= 1;")]
    [InlineData(true, "false == 1 >= 2;")]
    [InlineData(false, "true == 2 < 1;")]
    [InlineData(false, "true == 1 > 2;")]
    [InlineData(false, "true == 2 <= 1;")]
    [InlineData(false, "true == 1 >= 2;")]
    /// Testing the spacing of minus
    [InlineData(0, "1 - 1;")]
    [InlineData(0, "1 -1;")]
    [InlineData(0, "1- 1;")]
    [InlineData(0, "1-1;")]
    [InlineData(2, "1 - -1;")]
    [InlineData(0, "1 + -1;")]
    [InlineData(-1, "1 * -1;")]
    [InlineData(-1, "1 / -1;")]
    /// Divide by zero
    [InlineData("nil", "1 / 0;")]
    [InlineData("nil", "10 / 0 * 2;")]
    public void Test(object? expected, string input)
    {
        ErrorHandler handler = new();

        Scanner scanner = new(handler, input);
        scanner.ScanTokens();

        Parser parser = new(handler, scanner.Tokens);
        var expr = (Stmt.Expression?)parser.Parse().FirstOrDefault();

        // Scanning or parsing error
        if (handler.HadError || expr is null) Assert.Fail();

        Interpreter interpreter = new(handler);
        Assert.Equal(expected?.ToString(), interpreter.Interpret(expr.Expr).ToString());
    }
}
