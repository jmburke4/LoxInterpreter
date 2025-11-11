# C# Implementation of Robert Nystrom's Lox Interpreter #

Jackson Burke - Fall 2025 - CS 403 Programming Languages

Please note that VSCode has a built-in markdown previewer/renderer that does not require any extensions. You may preview a markdown file by secondary clicking the filename within VSCode and selecting "Open Preview".

## Introduction ##

I have followed Robert Nystroms instructions from [Crafting Interpreters](https://craftinginterpreters.com/contents.html) through Chapter 10, building a Lox Interpreter using C# and .NET 9.0. This project was developed using Windows 10, but will run on any operating system that has the .NET 9.0 SDK and Runtime installed. The .NET solution contains three dotnet projects: two console applications and an xUnit testing project. Please see individual .cs files for code comments and documentation, and the testing section at the end of this README.

View on [GitHub](https://github.com/jmburke4/LoxInterpreter/tree/submission).

## Repository Structure ##

- Ast.bat
- counter.lox
- Lox.bat
- LoxInterpreter.sln
- quicksort.lox
- README.md
- sample.lox
- /.vscode
    - launch.json
    - tasks.json
- /GenerateAst
    - GenerateAst.csproj
    - Generator.cs
    - Program.cs
- /LoxInterpreter
    - AstPrinter.cs
    - Environment.cs
    - ErrorHandler.cs
    - Expr.cs
    - ILoxCallable.cs
    - Interpreter.cs
    - LoxFunction.cs
    - LoxInterpreter.csproj
    - Parser.cs
    - Program.cs
    - Return.cs
    - Scanner.cs
    - Stmt.cs
    - Token.cs
    - Visitors.cs
- /LoxInterpreter.Tests
    - AstPrinterTests.cs
    - ControlFlowTests.cs
    - ExpressionTests.cs
    - FunctionTests.cs
    - LoxInterpreter.Tests.csproj
    - ParserTests.cs
    - ScannerTests.cs
    - TokenTests.cs

## Building and Running ##

Build all projects in the solution:
```shell
dotnet build
```

Build only a specific project in the solution:
```shell
dotnet build <path-to-.csproj>
dotnet build ./GenerateAst/GenerateAst.csproj
```

If you are on Windows, you can use *Ast.bat* or *Lox.bat* to launch the AstGenerator or the LoxInterpreter REPL (respectively) from the root of the directory. Otherwise you use 
```shell
dotnet run --project <path-to-.csproj>
dotnet run --project ./LoxInterpreter/LoxInterpreter.csproj
```

*Lox.bat* is setup to take an optional file path parameter to pass to the interpreter executable. If passed a valid filepath, the interpreter will sequentially run all of the lines in the file, then enter the REPL. If the contents of *sample.lox* are

```
var a = "Hello, World";
fun print_msg(){
    print a;
}
```

From the root *LoxInterpreter* directory run:
```
R:\LoxInterpreter>lox sample.lox

>print_msg();
Hello, world!
```

## Testing ##

LoxInterpreter.Tests is a C# project that contains unit tests using the xUnit testing framework to verify the different parts of the working Lox interpreter. Each file contains tests for a shared aspect of the interpreter.

[Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-vstest#filter-option-details)

Run all tests
```shell
dotnet test
```

Run tests in a specific class
```shell
dotnet test --filter "FullyQualifiedName~LoxInterpreter.Tests.TokenTests"
```

Run all tests excluding a specific class
```shell
dotnet test --filter "FullyQualifiedName!~LoxInterpreter.Tests.ScannerTests"
```

The output printed by ```dotnet test``` after running all tests in the xUnit project:
```
R:\LoxInterpreter>dotnet test
Restore complete (1.1s)
  LoxInterpreter succeeded (0.3s) → LoxInterpreter\bin\Debug\net9.0\LoxInterpreter.dll
  LoxInterpreter.Tests succeeded (2.8s) → LoxInterpreter.Tests\bin\Debug\net9.0\LoxInterpreter.Tests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 9.0.10)
[xUnit.net 00:00:00.53]   Discovering: LoxInterpreter.Tests
[xUnit.net 00:00:00.59]   Discovered:  LoxInterpreter.Tests
[xUnit.net 00:00:00.59]   Starting:    LoxInterpreter.Tests
[Line 1] Error  at '/': Divide by zero error. "10/0"
[Line 1] Error : Unexpected character "|"
[Line 130] RuntimeException: Divide by zero error. "10/0"
   at LoxInterpreter.Interpreter.VisitBinaryExpr(Binary expr) in R:\LoxInterpreter\LoxInterpreter\Visitors.cs:line 48
   at LoxInterpreter.Expr.Binary.Accept[T](IVisitor`1 visitor) in R:\LoxInterpreter\LoxInterpreter\Expr.cs:line 62
   at LoxInterpreter.Interpreter.Evaluate(Expr expr) in R:\LoxInterpreter\LoxInterpreter\Interpreter.cs:line 86
   at LoxInterpreter.Interpreter.VisitBinaryExpr(Binary expr) in R:\LoxInterpreter\LoxInterpreter\Visitors.cs:line 14
   at LoxInterpreter.Expr.Binary.Accept[T](IVisitor`1 visitor) in R:\LoxInterpreter\LoxInterpreter\Expr.cs:line 62
   at LoxInterpreter.Interpreter.Evaluate(Expr expr) in R:\LoxInterpreter\LoxInterpreter\Interpreter.cs:line 86
   at LoxInterpreter.Interpreter.Interpret(Expr expr) in R:\LoxInterpreter\LoxInterpreter\Interpreter.cs:line 125
[Line 1] Error  at '/': Divide by zero error. "1/0"
[Line 130] RuntimeException: Divide by zero error. "1/0"
   at LoxInterpreter.Interpreter.VisitBinaryExpr(Binary expr) in R:\LoxInterpreter\LoxInterpreter\Visitors.cs:line 48
   at LoxInterpreter.Expr.Binary.Accept[T](IVisitor`1 visitor) in R:\LoxInterpreter\LoxInterpreter\Expr.cs:line 62
   at LoxInterpreter.Interpreter.Evaluate(Expr expr) in R:\LoxInterpreter\LoxInterpreter\Interpreter.cs:line 86
   at LoxInterpreter.Interpreter.Interpret(Expr expr) in R:\LoxInterpreter\LoxInterpreter\Interpreter.cs:line 125
[xUnit.net 00:00:00.96]   Finished:    LoxInterpreter.Tests
  LoxInterpreter.Tests test succeeded (2.1s)

Test summary: total: 81, failed: 0, succeeded: 81, skipped: 0, duration: 2.0s
Build succeeded in 6.6s

R:\LoxInterpreter>
```

At the bottom see the *Test summary* line, where it indicates all 81 tests have passed. I have written three tests that throw exceptions, but the tests still pass because I was testing that the error was being handled correctly. In *ScannerTests.cs*, see ```ScanError()```, and in *ExpressionTests.cs* lines 34 and 35 are two tests with divide by zero errors. The error messages are printed to the console when running ```dotnet test```.

### Quicksort.lox ###
To prove the functionality of my interpreter, I wrote a rudimentary implementation of the quicksort sorting algorithm in lox. Arrays and linked lists have not yet been implemented in Lox, and to get around this I added some native string functions that can be called in lox: ```indexof()```, ```strat()```, ```strlen()```, and ```substring()```. Each of these native functions is a class in *LoxInterpreter/ILoxCallable.cs*. Then I treated a string variable as an array with numbers, with each element in the array delimited by a space. For example, an array containing three numbers 1, 2, 3 is represented as ```var arr = "1 2 3";```. The ```strat()``` (string-at) function is used for accessing an element by index. 

```
>print strat("a b c", 1)
b
```

Please see *quicksort.lox*.

```
R:\LoxInterpreter>lox quicksort.lox
2 1 5 3 4
 1 2  3  4 5

>var list = "3 2 7 4 6";

>print quicksort(list);
 2 3  4  6 7
```
