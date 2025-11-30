Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  VERIFICAR EMAIL DEL CLIENTE" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "http://localhost:5090"

Write-Host "1. Iniciando sesion..." -ForegroundColor Yellow
$loginBody = @{
    email = "muyguapoluisguapo@gmail.com"
    password = "Luis1206$"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "   [OK] Login exitoso" -ForegroundColor Green
} catch {
    Write-Host "   [ERROR] Error en login: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Verifica que:" -ForegroundColor Yellow
    Write-Host "- La API este corriendo en puerto 5090" -ForegroundColor White
    Write-Host "- El usuario este registrado con esas credenciales" -ForegroundColor White
    pause
    exit 1
}

Write-Host ""
Write-Host "2. Obteniendo informacion del cliente..." -ForegroundColor Yellow

$headers = @{
    "Authorization" = "Bearer $token"
}

try {
    $clientes = Invoke-RestMethod -Uri "$apiUrl/api/clientes" -Method GET -Headers $headers
    $miCliente = $clientes | Where-Object { $_.email -eq "muyguapoluisguapo@gmail.com" }

    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "  INFORMACION DEL CLIENTE" -ForegroundColor Cyan
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ""
    
    if ($miCliente) {
        Write-Host "ID: $($miCliente.id)" -ForegroundColor White
        Write-Host "Nombre Completo: $($miCliente.nombreCompleto)" -ForegroundColor White
        Write-Host "Email: $($miCliente.email)" -ForegroundColor Yellow
        Write-Host "Telefono: $($miCliente.telefono)" -ForegroundColor White
        Write-Host "Direccion: $($miCliente.direccion)" -ForegroundColor White
        Write-Host "Ciudad: $($miCliente.ciudad)" -ForegroundColor White
        Write-Host "Activo: $($miCliente.activo)" -ForegroundColor White
        Write-Host ""
        Write-Host "================================================" -ForegroundColor Cyan
        Write-Host ""
        
        if ([string]::IsNullOrEmpty($miCliente.email)) {
            Write-Host "[ERROR] EL CLIENTE NO TIENE EMAIL CONFIGURADO!" -ForegroundColor Red
            Write-Host ""
            Write-Host "Este es el problema por el que no llegan los correos." -ForegroundColor Yellow
            Write-Host "El cliente necesita tener un email valido en la base de datos." -ForegroundColor Yellow
        } else {
            Write-Host "[OK] El cliente tiene email configurado correctamente" -ForegroundColor Green
            Write-Host ""
            Write-Host "El email esta configurado como: $($miCliente.email)" -ForegroundColor Cyan
            Write-Host ""
            Write-Host "Si las compras desde el frontend no envian correo," -ForegroundColor Yellow
            Write-Host "revisa los logs de la API cuando hagas la compra." -ForegroundColor Yellow
        }
    } else {
        Write-Host "[ERROR] No se encontro un cliente con ese email" -ForegroundColor Red
        Write-Host ""
        Write-Host "Clientes encontrados:" -ForegroundColor Yellow
        foreach ($c in $clientes) {
            Write-Host "  - $($c.email) ($($c.nombreCompleto))" -ForegroundColor White
        }
    }
} catch {
    Write-Host "[ERROR] Error al obtener clientes: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
pause

