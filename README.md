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

Build solution
```shell
dotnet build
```

Build specific project
```shell
dotnet build <path-to-.csproj>
dotnet build ./GenerateAst/GenerateAst.csproj
```

## Testing ##
[Documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-vstest#filter-option-details)

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
