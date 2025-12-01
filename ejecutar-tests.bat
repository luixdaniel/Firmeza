@echo off
echo ========================================
echo   EJECUTANDO TESTS DE FIRMEZA
echo ========================================
echo.

cd /d "%~dp0Firmeza.Tests"

echo Limpiando proyecto...
dotnet clean --verbosity quiet

echo Restaurando paquetes...
dotnet restore --verbosity quiet

echo Compilando proyecto de tests...
dotnet build --verbosity quiet

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Fallo la compilacion
    pause
    exit /b 1
)

echo.
echo Ejecutando tests...
echo.
dotnet test --verbosity normal --no-build

echo.
echo ========================================
echo   TESTS COMPLETADOS
echo ========================================
pause

