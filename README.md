### Crafting Interpreters ###
[Table of Contents](https://craftinginterpreters.com/contents.html)

### Testing with *dotnet test* ###
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
