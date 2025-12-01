@echo off
chcp 65001 >nul
echo.
echo ╔════════════════════════════════════════════════════════════════╗
echo ║          🚀 INICIANDO PROYECTO FIRMEZA CON DOCKER              ║
echo ╚════════════════════════════════════════════════════════════════╝
echo.
echo 📦 Construyendo y levantando servicios...
echo.

docker-compose up --build -d

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ╔════════════════════════════════════════════════════════════════╗
    echo ║                  ✅ SERVICIOS INICIADOS                        ║
    echo ╠════════════════════════════════════════════════════════════════╣
    echo ║                                                                ║
    echo ║  🌐 ACCESO A LOS SERVICIOS:                                   ║
    echo ║                                                                ║
    echo ║  📱 Cliente (Next.js)                                         ║
    echo ║     👉 http://localhost:3000                                  ║
    echo ║                                                                ║
    echo ║  🔧 Portal Admin (ASP.NET MVC)                                ║
    echo ║     👉 http://localhost:5000                                  ║
    echo ║     🔐 Login: http://localhost:5000/Identity/Account/Login    ║
    echo ║                                                                ║
    echo ║  🔌 API REST (Swagger)                                        ║
    echo ║     👉 http://localhost:5090                                  ║
    echo ║     📚 Swagger: http://localhost:5090/swagger                 ║
    echo ║                                                                ║
    echo ╠════════════════════════════════════════════════════════════════╣
    echo ║  👤 CREDENCIALES DE ADMIN:                                    ║
    echo ║     Email:    admin@firmeza.com                               ║
    echo ║     Password: Admin123$                                       ║
    echo ╠════════════════════════════════════════════════════════════════╣
    echo ║  📊 COMANDOS ÚTILES:                                          ║
    echo ║     Ver logs:    docker-compose logs -f [servicio]            ║
    echo ║     Detener:     docker-compose down                          ║
    echo ║     Reiniciar:   docker-compose restart [servicio]            ║
    echo ║     Estado:      docker-compose ps                            ║
    echo ╚════════════════════════════════════════════════════════════════╝
    echo.
    
    REM Esperar 5 segundos para que los servicios se inicien
    timeout /t 5 /nobreak >nul
    
    echo 🔍 Verificando estado de los servicios...
    echo.
    docker-compose ps
    echo.
    
    echo 💡 TIP: Puedes ver los logs en tiempo real con: docker-compose logs -f
    echo.
) else (
    echo.
    echo ╔════════════════════════════════════════════════════════════════╗
    echo ║                  ❌ ERROR AL INICIAR SERVICIOS                 ║
    echo ╚════════════════════════════════════════════════════════════════╝
    echo.
    echo ⚠️  Revisa los logs con: docker-compose logs
    echo.
)

pause

