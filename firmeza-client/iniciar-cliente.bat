@echo off
echo ================================
echo   FIRMEZA CLIENT - Frontend
echo ================================
echo.
echo Iniciando servidor de desarrollo...
echo.

cd /d "%~dp0"

echo Verificando dependencias...
npm list autoprefixer >nul 2>&1
if errorlevel 1 (
    echo Instalando dependencias faltantes...
    npm install autoprefixer postcss
)

echo.
echo Iniciando Next.js en http://localhost:3000
echo.
echo Presiona Ctrl+C para detener el servidor
echo ================================
echo.

npm run dev

pause

