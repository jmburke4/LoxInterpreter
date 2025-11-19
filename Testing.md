## Testing ##
LoxInterpreter.Tests is a C# project that contains unit tests using the xUnit testing framework to verify the different parts of the working Lox interpreter. Each file contains tests for a shared aspect of the interpreter.

[Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-vstest#filter-option-details)

To run all tests
```shell
dotnet test
```

To run tests in a specific class
```shell
dotnet test --filter "FullyQualifiedName~LoxInterpreter.Tests.TokenTests"
```

To run all tests excluding a specific class
```shell
dotnet test --filter "FullyQualifiedName!~LoxInterpreter.Tests.ScannerTests"
```

The output printed by ```dotnet test``` in a CLI after running all tests in the xUnit project is recorded in *testresults.txt*. Please note that some of the test cases print exceptions to the console, which are not captured when piping the output to a text file. In order to capture all of the output from running ```dotnet test```, I copy and pasted from the console into *testresult.txt*


At the bottom of the testing report see the *Test summary* line, where it indicates all 81 tests have passed. I have written three tests that throw exceptions, but the tests still pass because I was testing that the error was being handled correctly. In *ScannerTests.cs*, see ```ScanError()```, and in *ExpressionTests.cs* lines 34 and 35 are two tests with divide by zero errors. The generated exceptions are printed to the console when running ```dotnet test```.

### Quicksort.lox ###
To prove the functionality of my interpreter, I wrote a rudimentary implementation of the quicksort sorting algorithm in lox. Arrays and linked lists have not yet been implemented in Lox, and to get around this I added some native string functions that can be called in lox: ```indexof()```, ```strat()```, ```strlen()```, and ```substring()```. Each of these native functions is a class in *LoxInterpreter/ILoxCallable.cs*. Then I treated a string variable as an array with numbers, with each element in the array delimited by a space. For example, an array containing three numbers 1, 2, 3 is represented as ```var arr = "1 2 3";```. The ```strat()``` (string-at) function is used for accessing an element by index. The spacing in the returned list is a little off, but it works.
```
>print strat("a b c", 1)
b
```

Please see *quicksort.lox*, which defines the quicksort() function and a sorted list when called at the end of the file. Sample output is recorded below.
```
R:\LoxInterpreter>lox quicksort.lox
2 1 5 3 4
 1 2  3  4 5

>var list = "3 2 7 4 6";

>print quicksort(list);
 2 3  4  6 7
```
