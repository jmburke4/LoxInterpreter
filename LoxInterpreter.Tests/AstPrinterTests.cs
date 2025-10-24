namespace LoxInterpreter.Tests;

public class AstPrinterTests
{
    [Fact]
    public void TestBinary()
    {
        string strexpr = "(+ 9 10)";
        Expr expr = new Binary(
            new Literal(9),
            new Token(TokenType.PLUS, "+", null, 1),
            new Literal(10)
        );

        Assert.Equal(strexpr, new AstPrinter().Print(expr));
    }

    [Fact]
    public void TestGrouping()
    {
        string strexpr = "(group 123)";
        Expr expr = new Grouping(new Literal(123));

        Assert.Equal(strexpr, new AstPrinter().Print(expr));
    }

    [Fact]
    public void TestLiteral()
    {
        string strexpr = "1.2";
        Expr expr = new Literal(1.2);

        Assert.Equal(strexpr, new AstPrinter().Print(expr));
    }
    
    [Fact]
    public void TestUnary()
    {
        string strexpr = "(- 2)";
        Expr expr = new Unary(
            new Token(TokenType.MINUS, "-", null, 1),
            new Literal(2)
        );

        Assert.Equal(strexpr, new AstPrinter().Print(expr));
    }

    [Fact]
    public void Test()
    {
        string strexpr = "(* (- 123) (group 45.67))";
        Expr expr = new Binary(
            new Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(123)
            ),
            new Token(TokenType.STAR, "*", null, 1),
            new Grouping(
                new Literal(45.67)
            )
        );

        Assert.Equal(strexpr, new AstPrinter().Print(expr));
    }
}
