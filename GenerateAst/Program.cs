namespace GenerateAst;

class Program
{
    private static void DefineAst(string dir, string baseName, List<string> types)
    {
        string path = $"{dir}\\{baseName}.cs";

        using (StreamWriter sw = new(path, true))
        {
            sw.WriteLine("namespace LoxInterpreter;");
            sw.WriteLine();
            sw.WriteLine($"abstract class {baseName}");
            sw.WriteLine("{\n\n}");
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: generateast <output_dir> | ast <output_dir>");
            Console.ReadKey();
            return;
        }
        else if (!Directory.Exists(args[0]))
        {
            Console.WriteLine($"{args[0]} is an invalid directory");
            Console.ReadKey();
            return;
        }

        DefineAst(args[0], "test", []);

        Console.WriteLine("Generation complete.");
        Console.ReadKey();
    }
}
