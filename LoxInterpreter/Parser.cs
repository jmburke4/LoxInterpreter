namespace LoxInterpreter;

public class Parser(ErrorHandler errorHandler, List<Token> tokens)
{
    private readonly List<Token> _tokens = tokens;

    private bool AtEnd => Peek.Type == TokenType.EOF;

    private int current = 0;

    private Token Peek => _tokens.ElementAt(current);

    private Token Previous => _tokens.ElementAt(current - 1);

    public readonly ErrorHandler ErrorHandler = errorHandler;

    private Token Advance()
    {
        if (!AtEnd) current++;
        return Previous;
    }

    private bool Check(TokenType type) => !AtEnd && Peek.Type == type;

    private Expr Comparison()
    {
        Expr expr = Term();

        while (Match([TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL]))
        {
            Token op = Previous;
            Expr right = Term();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Token Consume(TokenType type, string msg)
    {
        if (Check(type)) return Advance();

        throw Error(Peek, msg);
    }

    private ParseError Error(Token token, string msg)
    {
        ErrorHandler.Error(token, msg);
        return new ParseError();
    }

    private Expr Equality()
    {
        Expr expr = Comparison();

        while (Match([TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL]))
        {
            Token op = Previous;
            Expr right = Comparison();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Expression() => Equality();

    private Expr Factor()
    {
        Expr expr = Unary();

        while (Match([TokenType.SLASH, TokenType.STAR]))
        {
            Token op = Previous;
            Expr right = Unary();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private bool Match(List<TokenType> types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private Expr Primary()
    {
        if (Match([TokenType.FALSE])) return new Literal(false);
        if (Match([TokenType.TRUE])) return new Literal(true);
        if (Match([TokenType.NIL])) return new Literal(null);

        if (Match([TokenType.NUMBER, TokenType.STRING])) return new Literal(Previous.Literal);

        if (Match([TokenType.LEFT_PAREN]))
        {
            Expr expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Grouping(expr);
        }

        throw Error(Peek, "Expect expression.");
    }

    private void Synchronize()
    {
        Advance();

        while (!AtEnd)
        {
            if (Previous.Type == TokenType.SEMICOLON) return;

            switch (Peek.Type)
            {
                case TokenType.CLASS:
                case TokenType.FUN:
                case TokenType.VAR:
                case TokenType.FOR:
                case TokenType.IF:
                case TokenType.WHILE:
                case TokenType.PRINT:
                case TokenType.RETURN:
                    return;
            }

            Advance();
        }
    }

    private Expr Term()
    {
        Expr expr = Factor();

        while (Match([TokenType.MINUS, TokenType.PLUS]))
        {
            Token op = Previous;
            Expr right = Factor();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Unary()
    {
        if (Match([TokenType.BANG, TokenType.MINUS]))
        {
            Token op = Previous;
            Expr right = Unary();
            return new Unary(op, right);
        }

        return Primary();
    }

    public Expr? Parse()
    {
        try
        {
            return Expression();
        }
        catch (ParseError ex)
        {
            _ = ex;
            return null;
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
            return null;
        }
    }
    
    private class ParseError : Exception
    {
        
    }
}
