namespace LoxInterpreter;

/// <summary>
/// Represents a single token scanned from a line of Lox.
/// </summary>
/// <remarks>
/// Constructs a Token object.
/// </remarks>
/// <param name="type"></param>
/// <param name="lexeme"></param>
/// <param name="literal"></param>
/// <param name="line"></param>
public class Token(TokenType type, string lexeme, object? literal, int line)
{
    /// <summary>
    /// The <see cref="TokenType"/> of the token. 
    /// </summary>
    private readonly TokenType _type = type;

    /// <summary>
    /// The string value lexed from input.
    /// </summary>
    private readonly string _lexeme = lexeme;

    /// <summary>
    /// The literal object value of the token.
    /// </summary>
    private readonly object? _literal = literal;

    /// <summary>
    /// The line of input the token was found on.
    /// </summary>
    private readonly int _line = line;

    /// <inheritdoc cref="_type"/>
    public TokenType Type => _type;

    /// <inheritdoc cref="_lexeme"/>
    public string Lexeme => _lexeme;

    /// <inheritdoc cref="_literal"/>
    public object? Literal => _literal;

    /// <inheritdoc cref="_line"/>
    public int Line => _line;

    /// <summary>
    /// Overrides the ToString() method.
    /// </summary>
    /// <returns>
    /// <see cref="Type"/> <see cref="Lexeme"/> <see cref="Literal"/>
    /// </returns>
    public override string ToString()
    {
        return $"{Type} {Lexeme}";
    }
}

/// <summary>
/// Types of Tokens found in Lox.
/// </summary>
public enum TokenType
{
    // Single-character tokens.
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

    // One or two character tokens.
    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,

    // Literals.
    IDENTIFIER, STRING, NUMBER,

    // Keywords.
    AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
    PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

    EOF
}
