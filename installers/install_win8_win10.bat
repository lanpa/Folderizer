@echo off
set installDir=%LOCALAPPDATA%

mkdir %installDir%\Folderizer\

copy %~dp0ClassLibrary1.dll %installDir%\Folderizer\


rem 64bit
if exist %windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe (
    echo v4_x64
    %windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe %installDir%\Folderizer\ClassLibrary1.dll /codebase
timeout /t 10
    exit
) 
rem 32 bit
if exist %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe (
    echo v4_x86
    %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe %installDir%\Folderizer\ClassLibrary1.dll /codebase
timeout /t 10
    exit
) 
