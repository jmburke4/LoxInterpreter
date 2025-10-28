using System.Runtime.CompilerServices;

namespace LoxInterpreter;

/// <summary>
/// An instantiable class for presenting errors and exceptions to the user.
/// </summary>
public class ErrorHandler
{
    /// <summary>
    /// Stores the error messages written to the console.
    /// </summary>
    private string _errorMessage = "";

    /// <summary>
    /// Flag to record whether an error was thrown.
    /// </summary>
    private bool _hadError = false;

    /// <summary>
    /// Flag to record whether a runtime error occurred.
    /// </summary>
    private bool _hadRuntimeError = false;

    /// <inheritdoc cref="_errorMessage"/>
    public string ErrorMessage => _errorMessage;

    /// <inheritdoc cref="_hadError"/>
    public bool HadError => _hadError;

    /// <inheritdoc cref="_hadRuntimeError"/>
    public bool HadRuntimeError => _hadRuntimeError;

    /// <summary>
    /// Displays an error in the user's input.
    /// </summary>
    /// <param name="line">The user entered line where the error occurred.</param>
    /// <param name="message">The error message to display.</param>
    /// <remarks>Trips the <see cref="HadError" /> flag.</remarks>
    public void Error(int line, string message) => Report(line, "", message);

    /// <summary>
    /// Displays an error in the user's input.
    /// </summary>
    /// <param name="token">The token the error happened on.</param>
    /// <param name="message">The error message to display.</param>
    /// <remarks>Trips the <see cref="HadError" /> flag.</remarks>
    public void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF) Report(token.Line, " at end", message);
        else Report(token.Line, $" at '{token.Lexeme}'", message);
    }

    /// <summary>
    /// Displays the exception message to the user with the caller method's name and line number.
    /// </summary>
    /// <param name="ex">The exception caught.</param>
    /// <param name="callerName">The name of the calling method.</param>
    /// <param name="callerLine">The line number where the exception was caught.</param>
    /// <remarks>Trips the <see cref="HadError" /> flag.</remarks>
    public void Exception(Exception ex, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0)
    {
        string formattedMessage = $"[{callerName}() Line {callerLine}] Exception: {ex.Message}\n{ex.StackTrace}";
        Console.Error.WriteLine(formattedMessage);
        _errorMessage += "\n" + formattedMessage;
        _hadError = true;
    }

    /// <inheritdoc cref="Error"/>
    /// <param name="location">The location in the line where the error occurred.</param>
    public void Report(int line, string location, string message)
    {
        string formattedMessage = $"[Line {line}] Error {location}: {message}";
        Console.Error.WriteLine(formattedMessage);
        _errorMessage += "\n" + formattedMessage;
        _hadError = true;
    }

    /// <summary>
    /// Resets the <see cref="HadError"/> flag to false, and clears the error messages.
    /// </summary>
    public void ResetErrorFlag()
    {
        _hadError = false;
        _hadRuntimeError = false;
        _errorMessage = "";
    }

    /// <summary>
    /// Displays the exception message to the user with the caller method's name and line number.
    /// </summary>
    /// <param name="ex">The exception caught.</param>
    /// <param name="callerName">The name of the calling method.</param>
    /// <param name="callerLine">The line number where the exception was caught.</param>
    /// <remarks>Trips the <see cref="HadRuntimeError" /> flag.</remarks>
    public void RuntimeException(Exception ex, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0)
    {
        string formattedMessage = $"[{callerName}() Line {callerLine}] Exception: {ex.Message}\n{ex.StackTrace}";
        Console.Error.WriteLine(formattedMessage);
        _errorMessage += "\n" + formattedMessage;
        _hadRuntimeError = true;
    }
}
