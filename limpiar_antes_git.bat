@echo off
echo Limpiando archivos temporales antes de Git...
echo ================================================
echo.

cd /d "%~dp0"

REM Eliminar archivos de prueba temporal
echo Eliminando archivos de prueba temporal...
if exist TestGmailSMTP rmdir /s /q TestGmailSMTP
if exist test_smtp_connection.sh del /q test_smtp_connection.sh
if exist verificar_gmail.sh del /q verificar_gmail.sh
if exist DIAGNOSTICO_FINAL_GMAIL.md del /q DIAGNOSTICO_FINAL_GMAIL.md
if exist SOLUCION_GMAIL_SMTP_LINUX.md del /q SOLUCION_GMAIL_SMTP_LINUX.md

REM Limpiar bins y objs
echo Limpiando directorios de compilacion...
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d"

REM Limpiar node_modules
if exist firmeza-client\node_modules (
    echo Limpiando node_modules...
    rmdir /s /q firmeza-client\node_modules
)
if exist firmeza-client\.next (
    rmdir /s /q firmeza-client\.next
)

echo.
echo Limpieza completada!
echo.
echo Siguiente paso:
echo    git add .
echo    git commit -m "tu mensaje"
echo    git push origin main
echo.
pause

