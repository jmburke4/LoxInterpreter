namespace LoxInterpreter;

/// <summary>
/// Scans a list of tokens from a source string.
/// </summary>
/// <param name="source">The Lox string to scan tokens from.</param>
public class Scanner(ErrorHandler errorHandler, string source)
{
    /// <summary>
    /// The ErrorHandler object to display errors and exceptions to the user.
    /// </summary>
    public readonly ErrorHandler ErrorHandler = errorHandler;

    /// <summary>
    /// The source string of Lox to scan tokens from.
    /// </summary>
    private readonly string _source = source;

    /// <summary>
    /// A list of the tokens scanned from the source string.
    /// </summary>
    private readonly List<Token> _tokens = [];

    /// <summary>
    /// Index of the first character in the lexeme being scanned.
    /// </summary>
    private int start = 0;

    /// <summary>
    /// Index of the current character in the lexeme being scanned.
    /// </summary>
    private int current = 0;

    /// <summary>
    /// Maps reserved words to a <see cref="TokenType"/>. 
    /// </summary>
    private static readonly Dictionary<string, TokenType> _keywords = new()
    {
        { "and", TokenType.AND },
        { "class", TokenType.CLASS },
        { "else", TokenType.ELSE },
        { "false", TokenType.FALSE },
        { "for", TokenType.FOR },
        { "fun", TokenType.FUN },
        { "if", TokenType.IF },
        { "nil", TokenType.NIL },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "super", TokenType.SUPER },
        { "this", TokenType.THIS },
        { "true", TokenType.TRUE },
        { "var", TokenType.VAR },
        { "while", TokenType.WHILE }
    };

    /// <summary>
    /// The line number of the current lexeme being scanned.
    /// </summary>
    private int line = 1;

    /// <summary>
    /// Checks if the current index is greater than the length of the source string.
    /// </summary>
    private bool AtEnd => current >= _source.Length;

    /// <summary>
    /// Returns the list of scanned tokens.
    /// </summary>
    public List<Token> Tokens => _tokens;

    /// <summary>
    /// Adds a new <see cref="Token"/> to the list of tokens. 
    /// </summary>
    /// <param name="type">The <see cref="TokenType"/> of the new Token</param>
    private void AddToken(TokenType type)
    {
        _tokens.Add(new Token(type, "", null, line));
    }

    /// <inheritdoc cref="AddToken"/>
    /// <param name="literal">The value of the new Token</param>
    private void AddToken(TokenType type, object literal)
    {
        _tokens.Add(new Token(type, _source[start..current], literal, line));
    }

    /// <summary>
    /// Returns the next character in the source string.
    /// </summary>
    /// <returns>A char</returns>
    /// <remarks>Increments the current index.</remarks>
    private char Advance() => _source[current++];

    /// <summary>
    /// Scans an identifer and checks if it is a reserved word.
    /// </summary>
    private void Identifier()
    {
        while (IsAlphaNumeric(Peek()))
            Advance();

        if (!_keywords.TryGetValue(_source[start..current], out TokenType type))
            type = TokenType.IDENTIFIER;

        AddToken(type, _source[start..current]);
    }

    /// <summary>
    /// Checks if a char is an alphabetical character or an underscore.
    /// </summary>
    /// <param name="c">The char to check.</param>
    /// <returns>True or False</returns>
    static public bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

    /// <summary>
    /// Checks if a char is an alphanumeric character.
    /// </summary>
    /// <param name="c">The char to check.</param>
    /// <returns>True or False</returns>
    static public bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    /// <summary>
    /// Checks if a char is a digit.
    /// </summary>
    /// <param name="c">The char to check.</param>
    /// <returns>True or False</returns>
    static public bool IsDigit(char c) => c >= '0' && c <= '9';

    /// <summary>
    /// Compares the parameter to the next character in the source string.
    /// </summary>
    /// <param name="expected">The character to compare the next character to.</param>
    /// <returns>True or False</returns>
    /// <remarks>Does not increment the current index.</remarks>
    private bool Match(char expected)
    {
        if (AtEnd) return false;
        if (_source[current] != expected) return false;

        // Only consume the next character if it is the expected character
        current++;
        return true;
    }

    /// <summary>
    /// Scans a <see cref="Token"/> from a number.
    /// </summary>
    private void Number()
    {
        while (IsDigit(Peek())) Advance();

        // Look for a fractional part
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();
            while (IsDigit(Peek())) Advance();
        }

        AddToken(TokenType.NUMBER, double.Parse(_source[start..current]));
    }

    /// <summary>
    /// Returns the next character in the source string.
    /// </summary>
    /// <returns>The char value of the next character.</returns>
    /// /// <remarks>Does not increment the current index.</remarks>
    private char Peek() => AtEnd ? '\0' : _source[current];

    /// <summary>
    /// Returns the character after the next in the source string.
    /// </summary>
    /// <returns>The char value of the second-next character.</returns>
    /// <remarks>Does not increment the current index.</remarks>
    private char PeekNext() => current + 1 >= _source.Length ? '\0' : _source[current + 1];

    /// <summary>
    /// Prints the tokens in the token list to the console.
    /// </summary>
    public void PrintTokens()
    {
        foreach (var token in _tokens) Console.WriteLine(token);
    }

    /// <summary>
    /// Scans one token from the input string.
    /// </summary>
    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (Match('/'))
                {
                    // A comment goes until the end of the line
                    while (Peek() != '\n' && !AtEnd) Advance();
                }
                else AddToken(TokenType.SLASH);
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace
                break;
            case '\n':
                line++;
                break;
            case '"':
                String();
                break;
            default:
                if (IsDigit(c)) Number();
                else if (IsAlpha(c)) Identifier();
                else ErrorHandler.Error(line, $"Unexpected character \"{c}\"");
                break;
        }
    }

    /// <summary>
    /// Iterates through the input string and scans tokens.
    /// </summary>
    public void ScanTokens()
    {
        while (!AtEnd)
        {
            start = current;
            ScanToken();
        }

        AddToken(TokenType.EOF);
    }

    /// <summary>
    /// Scans a string starting and ending with a double quote.
    /// </summary>
    private void String()
    {
        while (Peek() != '"' && !AtEnd)
        {
            if (Peek() == '\n') line++;
            Advance();
        }

        if (AtEnd)
        {
            ErrorHandler.Error(line, "Unterminated string");
            return;
        }

        Advance();
        var val = _source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.STRING, val);
    }
}
