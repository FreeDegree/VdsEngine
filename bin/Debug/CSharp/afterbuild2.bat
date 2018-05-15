@echo off
copy %~dp0CSharpAssembly.dll E:\VirtualDataScene\SLN32\bin\CSharp /y
copy %~dp0CSharpAssembly.dll.mdb E:\VirtualDataScene\SLN32\bin\CSharp /y
copy %~dp0VdsEngine.dll E:\VirtualDataScene\SLN32\bin\CSharp /y
copy %~dp0VdsEngine.dll.mdb E:\VirtualDataScene\SLN32\bin\CSharp /y
exit