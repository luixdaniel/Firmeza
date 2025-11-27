@echo off
echo ========================================
echo   DETENIENDO SISTEMA FIRMEZA
echo ========================================
echo.
echo Este script detendra:
echo  - API Backend (Puerto 5090)
echo  - Frontend Next.js (Puerto 3000)
echo.
echo ========================================
echo.

echo [1/2] Deteniendo procesos de .NET (API)...
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'dotnet'} | Stop-Process -Force"
if %errorlevel% equ 0 (
    echo [OK] Procesos de API detenidos
) else (
    echo [i] No se encontraron procesos de API corriendo
)
echo.

echo [2/2] Deteniendo procesos de Node.js (Frontend)...
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'node'} | Stop-Process -Force"
if %errorlevel% equ 0 (
    echo [OK] Procesos de Frontend detenidos
) else (
    echo [i] No se encontraron procesos de Frontend corriendo
)
echo.

echo ========================================
echo   SISTEMA DETENIDO
echo ========================================
echo.
echo Todos los servicios han sido detenidos.
echo Puedes iniciarlos nuevamente con INICIAR_TODO.bat
echo.
pause

