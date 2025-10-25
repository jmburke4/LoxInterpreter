namespace LoxInterpreter.Tests;

public class ScannerTests
{
    [Fact]
    public void ListEqual()
    {
        var a = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.EOF, "", null, 1)
        };

        var b = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.EOF, "", null, 1)
        };

        Assert.Equal(a, b);
    }

    [Fact]
    public void ListNotEqual()
    {
        var a = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.EOF, "", null, 0)
        };

        var b = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.EOF, "", null, 1)
        };

        Assert.NotEqual(a, b);
    }
    
    [Fact]
    public void ScanError()
    {
        var source = "|";

        Scanner scanner = new(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.True(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        
        // Should still scan EOF
        Assert.Single(scanner.Tokens);
    }

    [Fact]
    public void ListTokens()
    {
        var a = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.EOF, "", null, 1)
        };

        Scanner b = new(new ErrorHandler(), "andy");
        b.ScanTokens();
        
        Assert.False(b.ErrorHandler.HadError, b.ErrorHandler.ErrorMessage);
        Assert.Equal(a, b.Tokens);
    }

    [Fact]
    public void Identifiers()
    {
        var source = "andy formless fo _ _123 _abc ab123\nabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";

        var tokens = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "andy", "andy", 1),
            new(TokenType.IDENTIFIER, "formless", "formless", 1),
            new(TokenType.IDENTIFIER, "fo", "fo", 1),
            new(TokenType.IDENTIFIER, "_", "_", 1),
            new(TokenType.IDENTIFIER, "_123", "_123", 1),
            new(TokenType.IDENTIFIER, "_abc", "_abc", 1),
            new(TokenType.IDENTIFIER, "ab123", "ab123", 1),
            new
            (
                TokenType.IDENTIFIER,
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_",
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_",
                2
            ),
            new(TokenType.EOF, "", null, 2)
        };

        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }

    [Fact]
    public void Keywords()
    {
        var source = "and class else false for fun if nil or return super this true var while";

        var tokens = new List<Token>()
        {
            new(TokenType.AND, "and", "and", 1),
            new(TokenType.CLASS, "class", "class", 1),
            new(TokenType.ELSE, "else", "else", 1),
            new(TokenType.FALSE, "false", "false", 1),
            new(TokenType.FOR, "for", "for", 1),
            new(TokenType.FUN, "fun", "fun", 1),
            new(TokenType.IF, "if", "if", 1),
            new(TokenType.NIL, "nil", "nil", 1),
            new(TokenType.OR, "or", "or", 1),
            new(TokenType.RETURN, "return", "return", 1),
            new(TokenType.SUPER, "super", "super", 1),
            new(TokenType.THIS, "this", "this", 1),
            new(TokenType.TRUE, "true", "true", 1),
            new(TokenType.VAR, "var", "var", 1),
            new(TokenType.WHILE, "while", "while", 1),
            new(TokenType.EOF, "", null, 1)
        };

        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }

    [Fact]
    public void Numbers()
    {
        var source = "123 123.456 .456 123.";

        var tokens = new List<Token>()
        {
            new(TokenType.NUMBER, "123", 123.0, 1),
            new(TokenType.NUMBER, "123.456", 123.456, 1),
            new(TokenType.DOT, ".", null, 1),
            new(TokenType.NUMBER, "456", 456.0, 1),
            new(TokenType.NUMBER, "123", 123.0, 1),
            new(TokenType.DOT, ".", null, 1),
            new(TokenType.EOF, "", null, 1)
        };
        
        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }

    [Fact]
    public void Punctuators()
    {
        var source = "(){};,+-*!===<=>=!=<>/.";

        var tokens = new List<Token>()
        {
            new(TokenType.LEFT_PAREN, "(", null, 1),
            new(TokenType.RIGHT_PAREN, ")", null, 1),
            new(TokenType.LEFT_BRACE, "{", null, 1),
            new(TokenType.RIGHT_BRACE, "}", null, 1),
            new(TokenType.SEMICOLON, ";", null, 1),
            new(TokenType.COMMA, ",", null, 1),
            new(TokenType.PLUS, "+", null, 1),
            new(TokenType.MINUS, "-", null, 1),
            new(TokenType.STAR, "*", null, 1),
            new(TokenType.BANG_EQUAL, "!=", null, 1),
            new(TokenType.EQUAL_EQUAL, "==", null, 1),
            new(TokenType.LESS_EQUAL, "<=", null, 1),
            new(TokenType.GREATER_EQUAL, ">=", null, 1),
            new(TokenType.BANG_EQUAL, "!=", null, 1),
            new(TokenType.LESS, "<", null, 1),
            new(TokenType.GREATER, ">", null, 1),
            new(TokenType.SLASH, "/", null, 1),
            new(TokenType.DOT, ".", null, 1),
            new(TokenType.EOF, "", null, 1)
        };

        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }

    [Fact]
    public void Strings()
    {
        var source = "\"\"\"string\"";

        var tokens = new List<Token>()
        {
            new(TokenType.STRING, "\"\"", "", 1),
            new(TokenType.STRING, "\"string\"", "string", 1),
            new(TokenType.EOF, "", null, 1)
        };

        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }

    [Fact]
    public void Whitespace()
    {
        var source = @"space    tabs				newlines




        end";

        var tokens = new List<Token>()
        {
            new(TokenType.IDENTIFIER, "space", "space", 1),
            new(TokenType.IDENTIFIER, "tabs", "tabs", 1),
            new(TokenType.IDENTIFIER, "newlines", "newlines", 1),
            new(TokenType.IDENTIFIER, "end", "end", 6),
            new(TokenType.EOF, "", null, 6)
        };

        var scanner = new Scanner(new ErrorHandler(), source);
        scanner.ScanTokens();

        Assert.False(scanner.ErrorHandler.HadError, scanner.ErrorHandler.ErrorMessage);
        Assert.Equal(tokens, scanner.Tokens);
    }
}
