using System.Runtime.CompilerServices;

namespace LoxInterpreter;

/// <summary>
/// An instantiable class for presenting errors and exceptions to the user.
/// </summary>
public class ErrorHandler
{
    /// <summary>
    /// Flag to record whether an error was thrown.
    /// </summary>
    private bool _hadError = false;

    /// <summary>
    /// Gets the Error flag.
    /// </summary>
    public bool HadError => _hadError;

    /// <summary>
    /// Displays an error in the user's input.
    /// </summary>
    /// <param name="line">The user entered line where the error occurred.</param>
    /// <param name="message">The error message to display.</param>
    /// <remarks>Trips the <see cref="HadError" /> flag.</remarks>
    public void Error(int line, string message) => Report(line, "", message);

    /// <summary>
    /// Displays the exception message to the user with the caller method's name and line number.
    /// </summary>
    /// <param name="ex">The exception caught.</param>
    /// <param name="callerName">The name of the calling method.</param>
    /// <param name="callerLine">The line number where the exception was caught.</param>
    /// <remarks>Trips the <see cref="HadError" /> flag.</remarks>
    public void Exception(Exception ex, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = 0)
    {
        Console.Error.WriteLine($"[{callerName}() Line {callerLine}] Exception: {ex.Message}\n{ex.StackTrace}");
        _hadError = true;
    }

    /// <inheritdoc cref="Error"/>
    /// <param name="location">The location in the line where the error occurred.</param>
    public void Report(int line, string location, string message)
    {
        Console.Error.WriteLine($"[Line {line}] Error {location}: {message}");
        _hadError = true;
    }

    /// <summary>
    /// Resets the <see cref="HadError"/> flag to false.
    /// </summary>
    public void ResetErrorFlag() => _hadError = false;
}
