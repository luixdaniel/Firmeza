@echo off
echo ================================================
echo   PRUEBA DIRECTA DE ENVIO DE EMAIL
echo ================================================
echo.

set EMAIL=%1
if "%EMAIL%"=="" (
    set EMAIL=muyguapoluisguapo@gmail.com
    echo Usando email predeterminado: %EMAIL%
) else (
    echo Usando email: %EMAIL%
)

echo.
echo Enviando correo de prueba...
echo.

curl -X POST "http://localhost:5090/api/testemail/send-test" ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"%EMAIL%\"}"

echo.
echo.
echo ================================================
echo Si el correo no llega:
echo 1. Revisa la carpeta de spam
echo 2. Verifica los logs de la API
echo 3. Asegúrate de que la API esté ejecutándose
echo ================================================
echo.
pause

