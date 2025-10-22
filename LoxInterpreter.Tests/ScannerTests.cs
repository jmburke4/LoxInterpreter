namespace LoxInterpreter.Tests;

public class ScannerTests
{
    [Fact]
    public void PassingTest()
    {
        Assert.Equal(8, 8);
    }

    [Fact]
    public void FailingTest()
    {
        Assert.Equal(8, 9);
    }

    [Fact]
    public void IsAlpha()
    {
        Assert.False(Scanner.IsAlpha('1'));
    }
}
