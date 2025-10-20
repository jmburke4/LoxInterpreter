namespace LoxInterpreter;

class Program
{
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

    private static void Run(string line)
    {
        Console.WriteLine(line);
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
            Console.WriteLine($"Error in runFile: {ex.Message}");
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
        }
    }
}
