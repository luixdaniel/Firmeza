@echo off
echo ========================================
echo   INICIANDO SISTEMA COMPLETO FIRMEZA
echo ========================================
echo.
echo Este script iniciara:
echo  - API Backend (Puerto 5090)
echo  - Frontend Next.js (Puerto 3000)
echo.
echo ========================================
echo.

echo [1/3] Verificando servicios existentes...
echo.

REM Verificar si los puertos ya estan en uso
netstat -ano | findstr ":5090" >nul
if %errorlevel% equ 0 (
    echo [!] La API ya esta corriendo en el puerto 5090
    echo     Si quieres reiniciarla, primero detén los procesos.
) else (
    echo [OK] Puerto 5090 disponible para la API
)

netstat -ano | findstr ":3000" >nul
if %errorlevel% equ 0 (
    echo [!] El Frontend ya esta corriendo en el puerto 3000
    echo     Si quieres reiniciarlo, primero detén los procesos.
) else (
    echo [OK] Puerto 3000 disponible para el Frontend
)

echo.
echo ========================================
echo [2/3] Iniciando API Backend...
echo ========================================
echo.

cd /d "%~dp0ApiFirmeza.Web"
start "API Firmeza - Puerto 5090" cmd /k "echo Iniciando API en puerto 5090... && dotnet run"

echo [OK] API iniciada en una nueva ventana
echo      Espera 10-15 segundos para que compile...
echo.

timeout /t 10 /nobreak >nul

echo ========================================
echo [3/3] Iniciando Frontend Next.js...
echo ========================================
echo.

cd /d "%~dp0firmeza-client"
start "Frontend Firmeza - Puerto 3000" cmd /k "echo Iniciando Frontend en puerto 3000... && npm run dev"

echo [OK] Frontend iniciado en una nueva ventana
echo      Espera 10-15 segundos para que compile...
echo.

echo ========================================
echo   SISTEMA INICIADO CORRECTAMENTE
echo ========================================
echo.
echo URLs disponibles:
echo  - Frontend:  http://localhost:3000
echo  - API:       http://localhost:5090
echo  - Swagger:   http://localhost:5090/swagger
echo.
echo Credenciales de Admin:
echo  - Email:     admin@firmeza.com
echo  - Password:  Admin123$
echo.
echo ========================================
echo.
echo [i] Se abriran 2 ventanas adicionales:
echo     - Una para la API (puerto 5090)
echo     - Una para el Frontend (puerto 3000)
echo.
echo [i] NO CIERRES esas ventanas mientras uses el sistema.
echo.
echo Presiona cualquier tecla para cerrar esta ventana...
pause >nul

