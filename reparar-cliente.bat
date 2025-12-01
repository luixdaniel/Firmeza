@echo off
echo ========================================
echo   REPARAR Y EJECUTAR CLIENTE FIRMEZA
echo ========================================
echo.

cd /d "%~dp0firmeza-client"

echo [1/5] Deteniendo procesos de Node...
taskkill /F /IM node.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo [2/5] Eliminando archivos antiguos...
if exist node_modules rmdir /s /q node_modules
if exist .next rmdir /s /q .next
if exist package-lock.json del /f /q package-lock.json

echo.
echo [3/5] Limpiando cache de npm...
call npm cache clean --force

echo.
echo [4/5] Instalando dependencias...
call npm install

echo.
echo [5/5] Iniciando servidor de desarrollo...
echo.
echo ========================================
echo   Servidor corriendo en:
echo   http://localhost:3000
echo ========================================
echo.

call npm run dev

