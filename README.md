# C# Implementation of Robert Nystrom's Lox Interpreter #

Jackson Burke - Fall 2025 - CS 403 Programming Languages

Please note that VSCode has a built-in markdown previewer/renderer that does not require any extensions. You may preview a markdown file by secondary clicking the filename within VSCode and selecting "Open Preview".

## Introduction ##

I have followed Robert Nystroms instructions from [Crafting Interpreters](https://craftinginterpreters.com/contents.html) through Chapter 10, building a Lox Interpreter using C# and .NET 9.0. This project was developed using Windows 10, but will run on any operating system that has the .NET 9.0 SDK and Runtime installed. The .NET solution contains three dotnet projects: two console applications and an xUnit testing project. Please see individual .cs files for code comments and documentation, and the testing section at the end of this README. Please see *Test.md* for a description of my testing of the interpreter.

View on [GitHub](https://github.com/jmburke4/LoxInterpreter/tree/submission).

## Repository Structure ##

- Ast.bat
- counter.lox
- Lox.bat
- LoxInterpreter.sln
- quicksort.lox
- README.md
- sample.lox
- testresults.txt
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

Building this project requires the .NET 9 SDK for building from the command line. The SDK can be downloaded from Microsoft [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0). Instructions for using the SDK installer can be found [here](https://learn.microsoft.com/en-us/dotnet/core/install/). Once downloaded, run the installer. You can verify that the runtime is installed by running ```dotnet --version``` from your command line interpreter. Open your CLI at the root directory of this project .NET to see the solution and project files.

To build all projects in the solution:
```shell
dotnet build
```

To build only a specific project in the solution:
```shell
dotnet build <path-to-.csproj>
dotnet build ./GenerateAst/GenerateAst.csproj
```

If you are on Windows, you can use the *Ast.bat* or *Lox.bat* Windows Batch Files to launch the AstGenerator or the LoxInterpreter REPL (respectively) from the root of the directory. Otherwise you use 
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

