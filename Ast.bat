@echo off
rem Can add /B to make the program run in the same window, but it gets confusing 
rem about which process (lox or the shell) is recieving input

rem Or I can just shorten the dotnet run command, but I like the new window
start "" ".\GenerateAst\bin\Debug\net9.0\GenerateAst.exe" "R:\\LoxInterpreter\\LoxInterpreter"
