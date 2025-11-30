Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  PRUEBA COMPLETA: COMPRA + ENVÍO DE EMAIL" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "http://localhost:5090"

# 1. Login
Write-Host "1. Iniciando sesión..." -ForegroundColor Yellow
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
    Write-Host "   [!] Asegurate de que el usuario este registrado" -ForegroundColor Yellow
    pause
    exit 1
}

Write-Host ""

# 2. Obtener productos
Write-Host "2. Obteniendo productos..." -ForegroundColor Yellow
$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

try {
    $productos = Invoke-RestMethod -Uri "$apiUrl/api/productos" -Method GET -Headers $headers
    $producto = $productos | Where-Object { $_.stock -gt 0 -and $_.activo -eq $true } | Select-Object -First 1
    
    if ($null -eq $producto) {
        Write-Host "   [ERROR] No hay productos disponibles" -ForegroundColor Red
        pause
        exit 1
    }
    
    Write-Host "   [OK] Producto: $($producto.nombre) - `$$($producto.precio)" -ForegroundColor Green
} catch {
    Write-Host "   [ERROR] Error: $($_.Exception.Message)" -ForegroundColor Red
    pause
    exit 1
}

Write-Host ""

# 3. Crear venta
Write-Host "3. Creando venta (esto enviará el correo)..." -ForegroundColor Yellow
$ventaBody = @{
    metodoPago = "Tarjeta"
    detalles = @(
        @{
            productoId = $producto.id
            cantidad = 1
            precioUnitario = $producto.precio
        }
    )
} | ConvertTo-Json -Depth 10

try {
    $ventaResponse = Invoke-RestMethod -Uri "$apiUrl/api/ventas" -Method POST -Body $ventaBody -Headers $headers
    Write-Host "   [OK] VENTA CREADA EXITOSAMENTE!" -ForegroundColor Green
    Write-Host ""
    Write-Host "   Detalles de la venta:" -ForegroundColor Cyan
    Write-Host "   - ID: $($ventaResponse.venta.id)" -ForegroundColor White
    Write-Host "   - Total: `$$($ventaResponse.venta.total)" -ForegroundColor White
    Write-Host "   - Factura: $($ventaResponse.venta.numeroFactura)" -ForegroundColor White
    Write-Host ""
    Write-Host "   Mensaje: $($ventaResponse.mensaje)" -ForegroundColor Yellow
    Write-Host ""
} catch {
    Write-Host "   [ERROR] Error al crear venta: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails.Message) {
        Write-Host "   Detalles: $($_.ErrorDetails.Message)" -ForegroundColor Red
    }
    pause
    exit 1
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "[OK] PROCESO COMPLETADO" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "[EMAIL] AHORA REVISA TU CORREO:" -ForegroundColor Yellow
Write-Host "   Email: muyguapoluisguapo@gmail.com" -ForegroundColor White
Write-Host "   Busca: Comprobante de Compra" -ForegroundColor White
Write-Host "   [!] No olvides revisar SPAM" -ForegroundColor Yellow
Write-Host ""
Write-Host "[TIP] Los correos pueden tardar 1-2 minutos en llegar" -ForegroundColor Cyan
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
pause

