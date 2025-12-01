@echo off
REM ====================================
REM   INICIAR APLICACIÓN FIRMEZA
REM ====================================
echo.
echo ========================================
echo   FIRMEZA - INICIAR APLICACIÓN COMPLETA
echo ========================================
echo.

cd /d "%~dp0"

echo [INFO] Directorio: %cd%
echo.
echo FLUJO DE EJECUCIÓN:
echo  1. Tests          - Pruebas automatizadas (OBLIGATORIO)
echo  2. Base de Datos  - PostgreSQL (Puerto 5432)
echo  3. API            - REST API (Puerto 5090)
echo  4. Admin          - Portal Web (Puerto 5000)
echo  5. Cliente        - Frontend SPA (Puerto 3000)
echo.
echo IMPORTANTE:
echo  - Si los tests FALLAN, todo se detiene
echo  - Si los tests PASAN, se levantan todos los servicios
echo.
echo Presiona Ctrl+C para detener todos los servicios
echo.

REM Ejecutar docker-compose
docker-compose up --build

REM Verificar si hubo error
if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Fallo en el despliegue
    echo [ERROR] Codigo de salida: %ERRORLEVEL%
    echo.
    echo Posibles causas:
    echo  - Tests fallaron
    echo  - Error de compilacion
    echo  - Docker no esta corriendo
    echo.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [EXITO] Aplicacion iniciada correctamente
echo.
pause

