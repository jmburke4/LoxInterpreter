namespace VisitorPatternExample;

internal class Program
{
    static void Main(string[] args)
    {
        var computerParts = new List<IComputerPart>
        {
            new Keyboard(),
            new Monitor()
        };

        var visitor = new ComputerPartDisplayVisitor();

        foreach (var part in computerParts)
        {
            part.Accept(visitor); // Part calls the correct visitor method
        }

        Console.ReadKey();
    }
}
