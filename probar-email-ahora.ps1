Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  PROBANDO ENVÍO DE EMAIL" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$email = "muyguapoluisguapo@gmail.com"
Write-Host "Enviando correo de prueba a: $email" -ForegroundColor Yellow
Write-Host ""

try {
    $body = @{
        email = $email
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "http://localhost:5090/api/testemail/send-test" `
        -Method POST `
        -Body $body `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host ""
    Write-Host "✅ RESPUESTA DEL SERVIDOR:" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ($response | ConvertTo-Json -Depth 5) -ForegroundColor White
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ""
    
    if ($response.success) {
        Write-Host "✅ ¡CORREO ENVIADO EXITOSAMENTE!" -ForegroundColor Green
        Write-Host ""
        Write-Host "📧 Revisa tu bandeja de entrada en: $email" -ForegroundColor Yellow
        Write-Host "📧 No olvides revisar la carpeta de SPAM" -ForegroundColor Yellow
    } else {
        Write-Host "❌ Error al enviar el correo" -ForegroundColor Red
        Write-Host "Mensaje: $($response.message)" -ForegroundColor Red
    }
} catch {
    Write-Host ""
    Write-Host "❌ ERROR AL CONECTAR CON LA API:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Verifica que:" -ForegroundColor Yellow
    Write-Host "1. La API esté corriendo en http://localhost:5090" -ForegroundColor White
    Write-Host "2. No haya errores en la consola de la API" -ForegroundColor White
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
pause

