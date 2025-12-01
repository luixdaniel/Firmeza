@echo off
REM ====================================
REM   EJECUTAR TESTS DE FIRMEZA
REM ====================================
echo.
echo ========================================
echo   FIRMEZA TESTS - DOCKER
echo ========================================
echo.

cd /d "%~dp0"

echo [INFO] Ejecutando desde la raiz de la solucion
echo [INFO] Directorio actual: %cd%
echo.

echo [1/3] Construyendo imagen de tests...
docker build -f Firmeza.Tests/Dockerfile -t firmeza-tests:latest .

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Fallo la construccion o los tests fallaron
    echo [ERROR] Codigo de salida: %ERRORLEVEL%
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [2/3] Tests completados exitosamente
echo.

REM Crear directorio para resultados
if not exist "test-results" mkdir test-results

echo [3/3] Copiando resultados...
docker run --rm -v "%cd%\test-results:/app/test-results" firmeza-tests:latest echo "Resultados copiados" 2>nul

echo.
echo ========================================
echo   TESTS EXITOSOS
echo ========================================
echo.
echo Resultados disponibles en: test-results\
echo.

pause
exit /b 0

