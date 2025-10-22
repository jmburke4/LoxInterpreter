namespace LoxInterpreter.Tests;

public class TokenTests
{
    [Fact]
    public void IdentifiersEqual()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "a", "a", 0);

        Assert.Equal(a, b);
    }

    [Fact]
    public void IdentifiersNotEqual()
    {
        Token a = new(TokenType.IDENTIFIER, "a", "a", 0);
        Token b = new(TokenType.IDENTIFIER, "b", "b", 0);

        Assert.NotEqual(a, b);
    }
}
