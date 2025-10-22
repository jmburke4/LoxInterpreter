namespace LoxInterpreter.Tests;

public class TokenTests
{
    [Fact]
    public void Equal()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "a", "a", 0);

        Assert.Equal(a, b);
    }

    [Fact]
    public void NotEqual1()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "b", "b", 0);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void NotEqual2()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.STRING, "a", "a", 0);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void NotEqual3()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "b", "a", 0);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void NotEqual4()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "a", "b", 0);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void NotEqual5()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "a", "a", 1);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void TestToString()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        string a_str = "IDENTIFIER a a 0";

        Assert.Equal(a_str, a.ToString());
    }
}
