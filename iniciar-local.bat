@echo off
REM ====================================
REM   INICIAR FIRMEZA - SIN DOCKER
REM ====================================
echo.
echo ========================================
echo   FIRMEZA - INICIAR LOCALMENTE
echo ========================================
echo.

cd /d "%~dp0"

echo [INFO] Iniciando servicios localmente (sin Docker)
echo.

REM Verificar que .NET está instalado
dotnet --version >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [ERROR] .NET SDK no esta instalado
    pause
    exit /b 1
)

echo [1/4] Ejecutando Tests...
cd Firmeza.Tests
dotnet test --verbosity minimal
if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Tests fallaron - Deteniendo inicio
    cd ..
    pause
    exit /b 1
)
cd ..

echo.
echo [2/4] Iniciando API (Puerto 5090)...
start "Firmeza API" cmd /k "cd ApiFirmeza.Web && dotnet run"
timeout /t 5 /nobreak >nul

echo.
echo [3/4] Iniciando Admin Portal (Puerto 5000)...
start "Firmeza Admin" cmd /k "cd Firmeza.Web && dotnet run"
timeout /t 5 /nobreak >nul

echo.
echo [4/4] Iniciando Frontend (Puerto 3000)...
start "Firmeza Client" cmd /k "cd firmeza-client && npm run dev"

echo.
echo ========================================
echo   SERVICIOS INICIADOS
echo ========================================
echo.
echo Frontend:  http://localhost:3000
echo Admin:     http://localhost:5000
echo API:       http://localhost:5090
echo Swagger:   http://localhost:5090/swagger
echo.
echo Presiona cualquier tecla para detener todos los servicios...
pause >nul

REM Detener todos los procesos
taskkill /FI "WINDOWTITLE eq Firmeza*" /F >nul 2>&1

echo.
echo [INFO] Servicios detenidos
pause

