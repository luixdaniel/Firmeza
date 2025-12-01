@echo off
echo ========================================
echo Estado de contenedores Docker - Firmeza
echo ========================================
echo.

echo --- Contenedores en ejecucion ---
docker ps
echo.

echo --- Todos los contenedores ---
docker ps -a
echo.

echo --- Logs de la API (ultimas 10 lineas) ---
docker logs firmeza-api --tail 10 2>&1
echo.

echo --- Logs del Admin (ultimas 10 lineas) ---
docker logs firmeza-admin --tail 10 2>&1
echo.

echo --- Logs del Client (ultimas 10 lineas) ---
docker logs firmeza-client --tail 10 2>&1
echo.

echo --- Puertos en uso ---
netstat -an | findstr "5090 5000 3000"
echo.

echo --- Test de endpoints ---
echo API (puerto 5090):
curl -s http://localhost:5090/health
echo.
echo Admin (puerto 5000):
curl -s http://localhost:5000/
echo.
echo Client (puerto 3000):
curl -s http://localhost:3000/
echo.

echo ========================================
echo Verificacion completada
echo ========================================
pause

