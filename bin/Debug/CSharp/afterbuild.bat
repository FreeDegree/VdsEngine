@for %%i in (mono.exe) do @set monoName=%%~$PATH:i
@for %%a in ("%monoName%") do @set monoPath=%%~dpa

@echo off
for /r "%~dp0" %%i in (*.dll) do "%monoName%" "%monoPath%\..\lib\mono\4.5\pdb2mdb.exe" "%%i"

pause