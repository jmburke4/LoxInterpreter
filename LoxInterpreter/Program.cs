namespace LoxInterpreter;

class Program
{
    public static ErrorHandler ErrorHandler { get; } = new ErrorHandler();

    public static Interpreter Interpreter { get; } = new Interpreter(ErrorHandler);

    private static void Run(string line)
    {
        try
        {
            Scanner scanner = new(ErrorHandler, line);
            scanner.ScanTokens();

            Parser parser = new(ErrorHandler, scanner.Tokens);
            Expr? expr = parser.Parse();

            if (ErrorHandler.HadError || expr == null) return;

            // We use the same interpreter object for variable and function definition tracking
            var result = Interpreter.Interpret(expr);
            if (ErrorHandler.HadRuntimeError) return;
            
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
        }
    }

    private static void RunFile(string path)
    {
        try
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                Run(line);
            }
            RunPrompt();
        }
        catch (Exception ex)
        {
            ErrorHandler.Exception(ex);
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
            Run(line);
            ErrorHandler.ResetErrorFlag();
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
