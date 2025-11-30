# Script para probar el envío de correos
$apiUrl = "http://localhost:5090"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  PRUEBA DE ENVÍO DE CORREO ELECTRÓNICO" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Login como admin
Write-Host "1. Iniciando sesión como Admin..." -ForegroundColor Yellow
$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "✅ Login exitoso" -ForegroundColor Green
    Write-Host "Token: $($token.Substring(0, 20))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ Error en login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# 2. Obtener un producto activo
Write-Host "2. Obteniendo productos disponibles..." -ForegroundColor Yellow
$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

try {
    $productos = Invoke-RestMethod -Uri "$apiUrl/api/productos" -Method GET -Headers $headers
    $producto = $productos | Where-Object { $_.stock -gt 0 -and $_.activo -eq $true } | Select-Object -First 1
    
    if ($null -eq $producto) {
        Write-Host "❌ No hay productos disponibles" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "✅ Producto encontrado:" -ForegroundColor Green
    Write-Host "   ID: $($producto.id)" -ForegroundColor Gray
    Write-Host "   Nombre: $($producto.nombre)" -ForegroundColor Gray
    Write-Host "   Precio: $($producto.precio)" -ForegroundColor Gray
    Write-Host "   Stock: $($producto.stock)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Error al obtener productos: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# 3. Obtener cliente actual
Write-Host "3. Obteniendo información del cliente..." -ForegroundColor Yellow
try {
    $clientes = Invoke-RestMethod -Uri "$apiUrl/api/clientes" -Method GET -Headers $headers
    $cliente = $clientes | Where-Object { $_.email -eq "admin@firmeza.com" } | Select-Object -First 1
    
    if ($null -eq $cliente) {
        Write-Host "⚠️ Cliente no encontrado, usando valores predeterminados" -ForegroundColor Yellow
        $clienteId = 1
    } else {
        $clienteId = $cliente.id
        Write-Host "✅ Cliente encontrado:" -ForegroundColor Green
        Write-Host "   ID: $($cliente.id)" -ForegroundColor Gray
        Write-Host "   Nombre: $($cliente.nombreCompleto)" -ForegroundColor Gray
        Write-Host "   Email: $($cliente.email)" -ForegroundColor Gray
    }
} catch {
    Write-Host "⚠️ Error al obtener cliente, usando ID 1: $($_.Exception.Message)" -ForegroundColor Yellow
    $clienteId = 1
}

Write-Host ""

# 4. Crear una venta de prueba
Write-Host "4. Creando venta de prueba (esto debería enviar el correo)..." -ForegroundColor Yellow
$ventaBody = @{
    clienteId = $clienteId
    metodoPago = "Tarjeta"
    detalles = @(
        @{
            productoId = $producto.id
            cantidad = 1
            precioUnitario = $producto.precio
        }
    )
} | ConvertTo-Json -Depth 10

Write-Host "Datos de la venta:" -ForegroundColor Gray
Write-Host $ventaBody -ForegroundColor DarkGray
Write-Host ""

try {
    $ventaResponse = Invoke-RestMethod -Uri "$apiUrl/api/ventas" -Method POST -Body $ventaBody -Headers $headers
    Write-Host "✅ Venta creada exitosamente!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Detalles de la respuesta:" -ForegroundColor Cyan
    Write-Host ($ventaResponse | ConvertTo-Json -Depth 5) -ForegroundColor White
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "⚠️  VERIFICA TU CORREO ELECTRÓNICO" -ForegroundColor Yellow
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "El comprobante debería llegar en unos momentos" -ForegroundColor White
    Write-Host "Si no llega, revisa la carpeta de spam" -ForegroundColor White
    Write-Host ""
} catch {
    Write-Host "❌ Error al crear venta:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    if ($_.ErrorDetails.Message) {
        Write-Host "Detalles del error:" -ForegroundColor Yellow
        Write-Host $_.ErrorDetails.Message -ForegroundColor Red
    }
    exit 1
}

