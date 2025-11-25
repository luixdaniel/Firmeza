@echo off
echo ========================================
echo   INICIANDO PROYECTO FIRMEZA COMPLETO
echo ========================================
echo.

echo 1. Iniciando API Backend (ASP.NET Core)...
start "Firmeza API" cmd /k "cd /d C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web && echo === FIRMEZA API === && dotnet run"

echo.
echo Esperando 5 segundos para que la API inicie...
timeout /t 5 /nobreak > nul

echo.
echo 2. Iniciando Frontend (Next.js)...
start "Firmeza Client" cmd /k "cd /d C:\Users\luisc\RiderProjects\Firmeza\firmeza-client && echo === FIRMEZA CLIENT === && npm run dev"

echo.
echo ========================================
echo   PROYECTO INICIADO
echo ========================================
echo.
echo API Backend: http://localhost:5090/swagger
echo Frontend: http://localhost:3000
echo.
echo Credenciales:
echo   Email: admin@firmeza.com
echo   Password: Admin123$
echo.
echo Presiona cualquier tecla para cerrar esta ventana...
pause > nul

