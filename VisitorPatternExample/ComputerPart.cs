namespace VisitorPatternExample;

// Element interface
public interface IComputerPart
{
    void Accept(IComputerPartVisitor visitor);
}

// Concrete Elements
public class Keyboard : IComputerPart
{
    public void Accept(IComputerPartVisitor visitor)
    {
        visitor.Visit(this); // Calls the Visitor's visit method for Keyboard
    }
}

public class Monitor : IComputerPart
{
    public void Accept(IComputerPartVisitor visitor)
    {
        visitor.Visit(this); // Calls the Visitor's visit method for Monitor
    }
}

// Visitor interface
public interface IComputerPartVisitor
{
    void Visit(Keyboard keyboard);
    void Visit(Monitor monitor);
}

// Concrete Visitor
public class ComputerPartDisplayVisitor : IComputerPartVisitor
{
    public void Visit(Keyboard keyboard)
    {
        Console.WriteLine("Displaying Keyboard.");
    }

    public void Visit(Monitor monitor)
    {
        Console.WriteLine("Displaying Monitor.");
    }
}

