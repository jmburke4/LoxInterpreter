namespace LoxInterpreter;

class Program
{
    public static ErrorHandler ErrorHandler { get; } = new ErrorHandler();

    public static Interpreter Interpreter { get; } = new Interpreter(ErrorHandler);

    private static void Run(string line)
    {
        Scanner scanner = new(ErrorHandler, line);
        scanner.ScanTokens();

        Parser parser = new(ErrorHandler, scanner.Tokens);
        var stmts = parser.Parse();

        if (ErrorHandler.HadError) return;

        // We use the same interpreter object for variable and function definition tracking
        Interpreter.Interpret(stmts);
    }

    private static void RunFile(string path)
    {
        try
        {
            Run(File.ReadAllText(path));
            RunPrompt();
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
            Console.ReadKey();
        }
    }

    private static void RunPrompt()
    {
        Console.WriteLine("Welcome to the Lox Intepreter REPL! \nType exit or ctrl + c to exit...");
        string? line;
        while (true)
        {
            Console.Write("\n>");
            line = Console.ReadLine();
            if (line == null || line == "exit") break;
            if (line == "cls") Console.Clear();
            else
            {
                Run(line);
                ErrorHandler.ResetErrorFlag();
            }
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: loxinterpreter <script> | lox <script>");
            return;
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }
}
