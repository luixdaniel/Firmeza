$baseUrl = "http://localhost:5090/api"

# Login
$loginBody = '{"email":"cliente_100718@test.com","password":"Test123$"}'
$login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"
$token = $login.token

Write-Host "Token obtenido" -ForegroundColor Green

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# Obtener productos
$productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET

Write-Host "Producto a comprar:" -ForegroundColor Yellow
Write-Host "  ID: $($productos[0].id)" -ForegroundColor Gray
Write-Host "  Nombre: $($productos[0].nombre)" -ForegroundColor Gray
Write-Host "  Precio: $($productos[0].precio)" -ForegroundColor Gray
Write-Host "  Stock: $($productos[0].stock)" -ForegroundColor Gray
Write-Host ""

# Crear venta con captura de error detallado
$ventaBody = @{
    metodoPago = "Efectivo"
    detalles = @(
        @{
            productoId = $productos[0].id
            cantidad = 1
            precioUnitario = $productos[0].precio
        }
    )
} | ConvertTo-Json -Depth 10

Write-Host "Body de la venta:" -ForegroundColor Yellow
Write-Host $ventaBody -ForegroundColor DarkGray
Write-Host ""

Write-Host "Creando venta..." -ForegroundColor Yellow

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headers
    Write-Host "SUCCESS!" -ForegroundColor Green
    Write-Host "Venta ID: $($venta.id)" -ForegroundColor Gray
    Write-Host "Total: $($venta.total)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR!" -ForegroundColor Red
    Write-Host "Status Code: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "Status Description: $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    Write-Host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    
    if ($_.ErrorDetails -and $_.ErrorDetails.Message) {
        Write-Host "Error Details:" -ForegroundColor Red
        $errorObj = $_.ErrorDetails.Message | ConvertFrom-Json
        Write-Host ($errorObj | ConvertTo-Json -Depth 10) -ForegroundColor Red
    }
}

