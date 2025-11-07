namespace GenerateAst;

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: generateast <output_dir> | ast");
            return;
        }
        else if (!Directory.Exists(args[0]))
        {
            Console.WriteLine($"{args[0]} is an invalid directory");
            return;
        }

        var expr = new Generator("Expr", args[0], "T",
        [
            "Assign   : Token Name, Expr Value",
            "Binary   : Expr Left, Token Operator, Expr Right",
            "Call     : Expr Callee, Token Paren, List<Expr> Arguments",
            "Grouping : Expr Expression",
            "Literal  : object? Value",
            "Logical  : Expr Left, Token Operator, Expr Right",
            "Unary    : Token Operator, Expr Right",
            "Variable : Token Name"
        ]);
        expr.Generate();

        var stmt = new Generator("Stmt", args[0], "void", [
            "Block      : List<Stmt> Statements",
            "Expression : Expr Expr",
            "Function   : Token Name, List<Token> Params, List<Stmt> Body",
            "If         : Expr Condition, Stmt ThenBranch, Stmt? ElseBranch",
            "Print      : Expr Expr",
            "Return     : Token Keyword, Expr Value",
            "Var        : Token Name, Expr Initializer",
            "While      : Expr Condition, Stmt Body"
        ]);
        stmt.Generate();
    }
}
