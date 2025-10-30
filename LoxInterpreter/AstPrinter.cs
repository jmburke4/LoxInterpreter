using System.Text;

namespace LoxInterpreter;

public class AstPrinter : Expr.IVisitor<string>
{
    public string Print(Expr expr) => expr.Accept(this);

    public string VisitBinaryExpr(Expr.Binary expr) => Parenthesize(expr.Operator.Lexeme, [expr.Left, expr.Right]);

    public string VisitGroupingExpr(Expr.Grouping expr) => Parenthesize("group", [expr.Expression]);

    public string VisitLiteralExpr(Expr.Literal expr) => expr.Value?.ToString() ?? "nil";

    public string VisitUnaryExpr(Expr.Unary expr) => Parenthesize(expr.Operator.Lexeme, [expr.Right]);

    // I added this just to get the project to compile, this needs fact checked
    public string VisitVariableExpr(Expr.Variable expr) => expr.Name.Lexeme;

    private string Parenthesize(string name, List<Expr> exprs)
    {
        StringBuilder builder = new();
        builder.Append('(').Append(name);

        foreach (var expr in exprs)
        {
            builder.Append(' ');
            builder.Append(expr.Accept(this));
        }

        builder.Append(')');

        return builder.ToString();
    }
}
