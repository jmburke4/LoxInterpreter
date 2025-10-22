@echo off
rem Can add /B to make the program run in the same window, but it gets confusing 
rem about which process (lox or the shell) is recieving input
start "" ".\LoxInterpreter\bin\Debug\net9.0\LoxInterpreter.exe" %*
