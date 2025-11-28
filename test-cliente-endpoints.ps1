# Script para probar los endpoints del área de cliente
# Asegúrate de que la API esté corriendo en http://localhost:5090

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "PRUEBA DE ENDPOINTS - ÁREA CLIENTE" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"
$token = ""

# Función para hacer peticiones
function Invoke-ApiRequest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [string]$Token = ""
    )
    
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    if ($Token) {
        $headers["Authorization"] = "Bearer $Token"
    }
    
    $url = "$baseUrl$Endpoint"
    
    try {
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $url -Method $Method -Headers $headers -Body $jsonBody
        } else {
            $response = Invoke-RestMethod -Uri $url -Method $Method -Headers $headers
        }
        return $response
    } catch {
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Response: $($_.Exception.Response)" -ForegroundColor Red
        return $null
    }
}

# 1. Login como Admin
Write-Host "1. Probando Login como Admin..." -ForegroundColor Yellow
$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123$"
}

$loginResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/Auth/login" -Body $loginBody

if ($loginResponse) {
    $token = $loginResponse.token
    Write-Host "✅ Login exitoso" -ForegroundColor Green
    Write-Host "Token recibido: $($token.Substring(0, 20))..." -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "❌ Login falló" -ForegroundColor Red
    exit
}

# 2. Verificar productos disponibles
Write-Host "2. Obteniendo lista de productos..." -ForegroundColor Yellow
$productos = Invoke-ApiRequest -Method "GET" -Endpoint "/Productos" -Token $token

if ($productos) {
    Write-Host "✅ Productos obtenidos: $($productos.Count) productos encontrados" -ForegroundColor Green
    if ($productos.Count -gt 0) {
        Write-Host "   Primer producto: $($productos[0].nombre) - $($productos[0].precio)" -ForegroundColor Gray
    }
    Write-Host ""
} else {
    Write-Host "❌ No se pudieron obtener productos" -ForegroundColor Red
}

# 3. Obtener perfil del usuario autenticado
Write-Host "3. Obteniendo perfil del cliente..." -ForegroundColor Yellow
$perfil = Invoke-ApiRequest -Method "GET" -Endpoint "/Clientes/perfil" -Token $token

if ($perfil) {
    Write-Host "✅ Perfil obtenido exitosamente" -ForegroundColor Green
    Write-Host "   Nombre: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Gray
    Write-Host "   Email: $($perfil.email)" -ForegroundColor Gray
    Write-Host "   Ciudad: $($perfil.ciudad)" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "⚠️  No se pudo obtener perfil (puede que el admin no esté en la tabla de clientes)" -ForegroundColor Yellow
    Write-Host ""
}

# 4. Obtener mis compras
Write-Host "4. Obteniendo historial de compras..." -ForegroundColor Yellow
$compras = Invoke-ApiRequest -Method "GET" -Endpoint "/Ventas/mis-compras" -Token $token

if ($compras) {
    Write-Host "✅ Compras obtenidas: $($compras.Count) compras encontradas" -ForegroundColor Green
    if ($compras.Count -gt 0) {
        Write-Host "   Primera compra: ID $($compras[0].id) - Total: $($compras[0].total)" -ForegroundColor Gray
    }
    Write-Host ""
} else {
    Write-Host "❌ No se pudieron obtener compras" -ForegroundColor Red
    Write-Host ""
}

# 5. Crear una venta de prueba (si hay productos)
if ($productos -and $productos.Count -gt 0 -and $perfil) {
    Write-Host "5. Creando venta de prueba..." -ForegroundColor Yellow
    
    $ventaBody = @{
        metodoPago = "Efectivo"
        detalles = @(
            @{
                productoId = $productos[0].id
                cantidad = 1
                precioUnitario = $productos[0].precio
            }
        )
    }
    
    $venta = Invoke-ApiRequest -Method "POST" -Endpoint "/Ventas" -Body $ventaBody -Token $token
    
    if ($venta) {
        Write-Host "✅ Venta creada exitosamente" -ForegroundColor Green
        Write-Host "   ID Venta: $($venta.id)" -ForegroundColor Gray
        Write-Host "   Total: $($venta.total)" -ForegroundColor Gray
        Write-Host "   Cliente: $($venta.clienteNombre)" -ForegroundColor Gray
        Write-Host ""
    } else {
        Write-Host "❌ No se pudo crear la venta" -ForegroundColor Red
        Write-Host ""
    }
} else {
    Write-Host "5. Saltando creación de venta (sin productos o sin perfil de cliente)" -ForegroundColor Yellow
    Write-Host ""
}

# 6. Verificar todas las ventas (solo Admin)
Write-Host "6. Obteniendo todas las ventas (Admin)..." -ForegroundColor Yellow
$todasVentas = Invoke-ApiRequest -Method "GET" -Endpoint "/Ventas" -Token $token

if ($todasVentas) {
    Write-Host "✅ Ventas obtenidas: $($todasVentas.Count) ventas en total" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "❌ No se pudieron obtener todas las ventas" -ForegroundColor Red
    Write-Host ""
}

# Resumen
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "RESUMEN DE PRUEBAS" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Endpoints probados:" -ForegroundColor White
Write-Host "✅ POST /api/Auth/login" -ForegroundColor Green
Write-Host "✅ GET  /api/Productos" -ForegroundColor Green
Write-Host "$(if ($perfil) { '✅' } else { '⚠️ ' }) GET  /api/Clientes/perfil" -ForegroundColor $(if ($perfil) { 'Green' } else { 'Yellow' })
Write-Host "$(if ($compras) { '✅' } else { '❌' }) GET  /api/Ventas/mis-compras" -ForegroundColor $(if ($compras) { 'Green' } else { 'Red' })
Write-Host "$(if ($productos -and $productos.Count -gt 0 -and $perfil) { if ($venta) { '✅' } else { '❌' } } else { '⏭️ ' }) POST /api/Ventas" -ForegroundColor $(if ($venta) { 'Green' } else { 'Yellow' })
Write-Host "$(if ($todasVentas) { '✅' } else { '❌' }) GET  /api/Ventas (Admin)" -ForegroundColor $(if ($todasVentas) { 'Green' } else { 'Red' })
Write-Host ""

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Pruebas completadas!" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan

