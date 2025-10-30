namespace LoxInterpreter;

/// <summary>
/// Creates an Expression tree from a list of tokens.
/// </summary>
/// <param name="errorHandler">The error handler object to use.</param>
/// <param name="tokens">The list of tokens to consume.</param>
public class Parser(ErrorHandler errorHandler, List<Token> tokens)
{
    /// <summary>
    /// The list of tokens to parse.
    /// </summary>
    private readonly List<Token> _tokens = tokens;

    /// <summary>
    /// Returns true if the next token is the EOF token.
    /// </summary>
    private bool AtEnd => Peek.Type == TokenType.EOF;

    /// <summary>
    /// An integer for tracking the index of the current token being parsed.
    /// </summary>
    private int current = 0;

    /// <summary>
    /// Returns the token in the list at the current index without incrementing the index.
    /// </summary>
    private Token Peek => _tokens.ElementAt(current);

    /// <summary>
    /// Returns the token in the list preceding the current index without incrementing the index.
    /// </summary>
    private Token Previous => _tokens.ElementAt(current - 1);

    /// <summary>
    /// The ErrorHandler object to display errors and exceptions to the user.
    /// </summary>
    public readonly ErrorHandler ErrorHandler = errorHandler;

    /// <summary>
    /// Returns the current token in the list and increments the index.
    /// </summary>
    /// <returns>The <see cref="Token"/> at the current index</returns>
    private Token Advance()
    {
        if (!AtEnd) current++;
        return Previous;
    }

    private Expr Assignment()
    {
        Expr expr = Equality();

        if (Match([TokenType.EQUAL]))
        {
            Token equals = Previous;
            Expr value = Assignment();

            if (expr.GetType() == typeof(Expr.Variable))
            {
                Token name = ((Expr.Variable)expr).Name;
                return new Expr.Assign(name, value);
            }

            Error(equals, "Invalid assignment target.");
        }

        return expr;
    }

    /// <summary>
    /// Checks if the current token matches the TokenType parameter.
    /// </summary>
    /// <param name="type">The <see cref="TokenType"/> to compare.</param>
    /// <returns>True or False</returns>
    private bool Check(TokenType type) => !AtEnd && Peek.Type == type;

    /// <summary>
    /// Parses comparison (>, <, >=, <=) expressions.
    /// </summary>
    /// <returns>An expression</returns>
    private Expr Comparison()
    {
        Expr expr = Term();

        while (Match([TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL]))
        {
            Token op = Previous;
            Expr right = Term();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    /// <summary>
    /// Checks if the next token is the expected token and increments the index, otherwise throws error.
    /// </summary>
    /// <param name="type">The type of the token expected.</param>
    /// <param name="msg">The error message to display if the next token is unexpected.</param>
    /// <returns>The expected token</returns>
    private Token Consume(TokenType type, string msg)
    {
        if (Check(type)) return Advance();

        throw Error(Peek, msg);
    }

    /// <summary>
    /// Generates a <see cref="ParseError"/> exception and sends a message to the error handler.
    /// </summary>
    /// <param name="token">The unexpected token.</param>
    /// <param name="msg">The error message.</param>
    /// <returns>A <see cref="ParseError"/> exception</returns>
    private ParseError Error(Token token, string msg)
    {
        ErrorHandler.Error(token, msg);
        return new ParseError();
    }

    private Stmt? Declaration()
    {
        try
        {
            if (Match([TokenType.VAR])) return VarDeclaration();
            return Statement();
        }
        catch (ParseError ex)
        {
            ErrorHandler.Exception(ex);
            Synchronize();
            return null;
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
            Synchronize();
            return null;
        }
    }

    /// <summary>
    /// Parses equality (!=, ==) expressions.
    /// </summary>
    /// <returns>An expression</returns>
    private Expr Equality()
    {
        Expr expr = Comparison();

        while (Match([TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL]))
        {
            Token op = Previous;
            Expr right = Comparison();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    /// <summary>
    /// Starts the recursive descent parser.
    /// </summary>
    /// <returns>The parsed expression</returns>
    private Expr Expression() => Assignment();

    private Stmt.Expression ExpressionStatement()
    {
        Expr expr = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
        return new Stmt.Expression(expr);
    }

    /// <summary>
    /// Parses multiplication and division expressions.
    /// </summary>
    /// <returns>The parsed expression</returns>
    private Expr Factor()
    {
        Expr expr = Unary();

        while (Match([TokenType.SLASH, TokenType.STAR]))
        {
            Token op = Previous;
            Expr right = Unary();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    /// <summary>
    /// Checks if the current token is one of the types passed.
    /// </summary>
    /// <param name="types">The types to check the token against.</param>
    /// <returns>True or False</returns>
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

    /// <summary>
    /// Parses literal and grouping tokens.
    /// </summary>
    /// <returns>An expression</returns>
    private Expr Primary()
    {
        if (Match([TokenType.FALSE])) return new Expr.Literal(false);
        if (Match([TokenType.TRUE])) return new Expr.Literal(true);
        if (Match([TokenType.NIL])) return new Expr.Literal(null);

        if (Match([TokenType.NUMBER, TokenType.STRING])) return new Expr.Literal(Previous.Literal);
        if (Match([TokenType.IDENTIFIER])) return new Expr.Variable(Previous);

        if (Match([TokenType.LEFT_PAREN]))
        {
            Expr expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Expr.Grouping(expr);
        }

        throw Error(Peek, "Expect expression.");
    }

    private Stmt.Print PrintStatement()
    {
        Expr val = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after value.");
        return new Stmt.Print(val);
    }

    /// <summary>
    /// Recovers the parser after a syntax error
    /// </summary>
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

    /// <summary>
    /// Parses addition and subtraction expressions.
    /// </summary>
    /// <returns>An expression</returns>
    private Expr Term()
    {
        Expr expr = Factor();

        while (Match([TokenType.MINUS, TokenType.PLUS]))
        {
            Token op = Previous;
            Expr right = Factor();
            expr = new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    /// <summary>
    /// Parses not and negative signs.
    /// </summary>
    /// <returns>An expression</returns>
    private Expr Unary()
    {
        if (Match([TokenType.BANG, TokenType.MINUS]))
        {
            Token op = Previous;
            Expr right = Unary();
            return new Expr.Unary(op, right);
        }

        return Primary();
    }

    private Stmt VarDeclaration()
    {
        Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

        Expr init;
        if (Match([TokenType.EQUAL])) init = Expression();
        else throw new ParseError();

        Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
        return new Stmt.Var(name, init);
    }

    public List<Stmt> Parse()
    {
        var statements = new List<Stmt>();
        try
        {
            while (!AtEnd)
            {
                var t = Declaration();
                if (t != null) statements.Add(t);
            }
            
            return statements;
        }
        catch (ParseError ex)
        {
            _ = ex;
            return statements;
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
            return statements;
        }
    }

    // This was made public so that it is accessible to the test project
    public Stmt Statement()
    {
        if (Match([TokenType.PRINT])) return PrintStatement();
        return ExpressionStatement();
    }

    /// <summary>
    /// Simple exception extension for reporting syntax errors.
    /// </summary>
    private class ParseError : Exception
    {

    }
}
