@echo off
cls
echo ========================================
echo    INICIANDO API FIRMEZA
echo ========================================
echo.
echo Deteniendo procesos anteriores...
taskkill /F /IM dotnet.exe >nul 2>&1
timeout /t 2 >nul

echo Cambiando al directorio de la API...
cd /d C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web

echo.
echo Compilando la API...
dotnet build --no-restore

echo.
echo ========================================
echo    EJECUTANDO API EN http://localhost:5090
echo ========================================
echo.
echo Presiona Ctrl+C para detener la API
echo.

dotnet run --no-build

pause

