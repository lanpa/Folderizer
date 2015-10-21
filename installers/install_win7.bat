@echo off
set installDir=%LOCALAPPDATA%

mkdir %installDir%\Folderizer\

copy %~dp0ClassLibrary1_win7.dll %installDir%\Folderizer\


rem .net 3.5
rem 64 bit
if exist %windir%\Microsoft.NET\Framework64\v2.0.50727\RegAsm.exe (
    echo v2_x64
    %windir%\Microsoft.NET\Framework64\v2.0.50727\RegAsm.exe %installDir%\Folderizer\ClassLibrary1_win7.dll /codebase
    exit
)

rem 32bit
if exist %windir%\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe (
    echo v2_x86
    %windir%\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe %installDir%\Folderizer\ClassLibrary1_win7.dll /codebase
    exit
)
