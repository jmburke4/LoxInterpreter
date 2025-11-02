namespace LoxInterpreter.Tests;

/// <summary>
/// See https://github.com/munificent/craftinginterpreters/blob/master/test/precedence.lox
/// </summary>
public class ExpressionTests
{
    private static object? TestHelper(string input)
    {
        ErrorHandler handler = new();

        Scanner scanner = new(handler, input);
        scanner.ScanTokens();

        Parser parser = new(handler, scanner.Tokens);
        var expr = (Stmt.Expression?)parser.Parse().FirstOrDefault();

        // Scanning or parsing error
        if (handler.HadError || expr is null) return null;

        Interpreter interpreter = new(handler);
        return interpreter.Interpret(expr.Expr);
    }

    /// <summary>
    /// Basic order of operations
    /// </summary>

    [Fact]
    public void PlusStar() => Assert.Equal(14, (double?)TestHelper("2 + 3 * 4;"));

    [Fact]
    public void MinusStar() => Assert.Equal(8, (double?)TestHelper("20 - 3 * 4;"));

    [Fact]
    public void PlusSlash() => Assert.Equal(4, (double?)TestHelper("2 + 6 / 3;"));

    [Fact]
    public void MinusSlash() => Assert.Equal(0, (double?)TestHelper("2 - 6 / 3;"));

    [Fact]
    public void Grouping() => Assert.Equal(4, (double?)TestHelper("(2 * (6 - (2 + 2)));"));

    /// <summary>
    /// Comparison precedence
    /// </summary>

    [Fact]
    public void EqLt() => Assert.Equal(true, TestHelper("false == 2 < 1;"));

    [Fact]
    public void EqGt() => Assert.Equal(true, TestHelper("false == 1 > 2;"));

    [Fact]
    public void EqLte() => Assert.Equal(true, TestHelper("false == 2 <= 1;"));

    [Fact]
    public void EqGte() => Assert.Equal(true, TestHelper("false == 1 >= 2;"));

    [Fact]
    public void EqLtF() => Assert.Equal(false, TestHelper("true == 2 < 1;"));

    [Fact]
    public void EqGtF() => Assert.Equal(false, TestHelper("true == 1 > 2;"));

    [Fact]
    public void EqLteF() => Assert.Equal(false, TestHelper("true == 2 <= 1;"));

    [Fact]
    public void EqGteF() => Assert.Equal(false, TestHelper("true == 1 >= 2;"));

    /// <summary>
    /// Testing the spacing of minus
    /// </summary>

    [Fact]
    public void OneMinusOne() => Assert.Equal(0, (double?)TestHelper("1 - 1;"));

    [Fact]
    public void OneNegOne() => Assert.Equal(0, (double?)TestHelper("1 -1;"));

    [Fact]
    public void OneMinusOneOther() => Assert.Equal(0, (double?)TestHelper("1- 1;"));

    [Fact]
    public void OneMinusOneNoSpace() => Assert.Equal(0, (double?)TestHelper("1-1;"));

    [Fact]
    public void OneMinusNegOne() => Assert.Equal(2, (double?)TestHelper("1 - -1;"));

    [Fact]
    public void OnePlusNegOne() => Assert.Equal(0, (double?)TestHelper("1 + -1;"));

    [Fact]
    public void OneStarNegOne() => Assert.Equal(-1, (double?)TestHelper("1 * -1;"));

    [Fact]
    public void OneSlashNegOne() => Assert.Equal(-1, (double?)TestHelper("1 / -1;"));

    /// <summary>
    /// Divide by zero
    /// </summary>

    [Fact]
    public void DivideByZero() => Assert.Equal("nil", TestHelper("1 / 0;"));

    [Fact]
    public void NestedDivideByZero() => Assert.Equal("nil", TestHelper("10 / 0 * 2;"));
}
