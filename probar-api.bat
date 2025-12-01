@echo off
echo.
echo ========================================
echo   PROBANDO ACCESO A LA API
echo ========================================
echo.

echo Probando http://localhost:5090/swagger ...
powershell -Command "try { $response = Invoke-WebRequest -Uri 'http://localhost:5090/swagger' -UseBasicParsing -TimeoutSec 5; Write-Host '✅ SWAGGER RESPONDE - Codigo:' $response.StatusCode } catch { Write-Host '❌ SWAGGER NO RESPONDE:' $_.Exception.Message }"

echo.
echo Probando http://localhost:5090/health ...
powershell -Command "try { $response = Invoke-WebRequest -Uri 'http://localhost:5090/health' -UseBasicParsing -TimeoutSec 5; Write-Host '✅ HEALTH RESPONDE - Codigo:' $response.StatusCode; Write-Host 'Contenido:' ($response.Content | ConvertFrom-Json | ConvertTo-Json -Compress) } catch { Write-Host '❌ HEALTH NO RESPONDE:' $_.Exception.Message }"

echo.
echo Probando http://localhost:5090/api ...
powershell -Command "try { $response = Invoke-WebRequest -Uri 'http://localhost:5090/api' -UseBasicParsing -TimeoutSec 5; Write-Host '✅ API RESPONDE - Codigo:' $response.StatusCode } catch { Write-Host '❌ API NO RESPONDE:' $_.Exception.Message }"

echo.
echo ========================================
echo   VERIFICACION DE PUERTOS
echo ========================================
netstat -an | findstr ":5090"
echo.

echo ========================================
echo   ESTADO DEL CONTENEDOR
echo ========================================
docker inspect firmeza-api --format="Estado: {{.State.Status}}, Salud: {{.State.Health.Status}}"
echo.

pause

