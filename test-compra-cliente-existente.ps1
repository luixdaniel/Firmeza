Write-Host "================================" -ForegroundColor Cyan
Write-Host "PRUEBA DE COMPRA - CLIENTE EXISTENTE" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"

# Probar con un cliente existente
$emailCliente = "cliente@firmeza.com"
$passwordCliente = "Cliente123`$"

Write-Host "Usando cliente existente:" -ForegroundColor Yellow
Write-Host "   Email: $emailCliente" -ForegroundColor Gray
Write-Host ""

# 1. Login con el cliente
Write-Host "1. Haciendo login como cliente..." -ForegroundColor Yellow
$loginBody = @{
    email = $emailCliente
    password = $passwordCliente
} | ConvertTo-Json

try {
    $login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $login.token
    Write-Host "OK - Login exitoso" -ForegroundColor Green
    Write-Host "   Nombre: $($login.nombreCompleto)" -ForegroundColor Gray
    Write-Host "   Roles: $($login.roles -join ', ')" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - No se pudo hacer login" -ForegroundColor Red
    Write-Host "   $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "NOTA: Si este cliente no funciona, prueba con otro:" -ForegroundColor Yellow
    Write-Host "   cliente@firmeza.com" -ForegroundColor Gray
    Write-Host "   ceraluis4@gmail.com" -ForegroundColor Gray
    exit
}
Write-Host ""

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. Obtener perfil
Write-Host "2. Obteniendo perfil..." -ForegroundColor Yellow
try {
    $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headers
    Write-Host "OK - Perfil obtenido" -ForegroundColor Green
    Write-Host "   ID: $($perfil.id)" -ForegroundColor Gray
    Write-Host "   Nombre: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Gray
    Write-Host "   Email: $($perfil.email)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

# 3. Obtener productos
Write-Host "3. Obteniendo productos..." -ForegroundColor Yellow
try {
    $productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
    $productosConStock = $productos | Where-Object { $_.stock -gt 0 }
    Write-Host "OK - $($productosConStock.Count) productos con stock" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

# 4. Crear carrito
Write-Host "4. Simulando carrito..." -ForegroundColor Yellow
$producto = $productosConStock[0]
$carrito = @(
    @{
        productoId = $producto.id
        cantidad = 1
        precioUnitario = $producto.precio
    }
)

Write-Host "OK - Producto seleccionado:" -ForegroundColor Green
Write-Host "   Nombre: $($producto.nombre)" -ForegroundColor Gray
Write-Host "   Precio: `$$($producto.precio)" -ForegroundColor Gray
Write-Host "   Stock: $($producto.stock)" -ForegroundColor Gray
Write-Host ""

# 5. FINALIZAR COMPRA
Write-Host "5. FINALIZANDO COMPRA..." -ForegroundColor Yellow
Write-Host ""

$ventaBody = @{
    metodoPago = "Efectivo"
    detalles = $carrito
} | ConvertTo-Json -Depth 10

Write-Host "Datos enviados:" -ForegroundColor DarkGray
Write-Host $ventaBody -ForegroundColor DarkGray
Write-Host ""

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headers -ErrorAction Stop
    
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "   COMPRA EXITOSA!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "ID Venta: #$($venta.id)" -ForegroundColor Cyan
    Write-Host "Cliente: $($venta.clienteNombre)" -ForegroundColor White
    Write-Host "Total: `$$($venta.total)" -ForegroundColor Yellow
    Write-Host "Fecha: $($venta.fecha)" -ForegroundColor White
    Write-Host ""
    Write-Host "Productos:" -ForegroundColor Cyan
    foreach ($det in $venta.detalles) {
        Write-Host "   - $($det.productoNombre): $($det.cantidad) x `$$($det.precioUnitario) = `$$($det.subtotal)" -ForegroundColor Gray
    }
    Write-Host ""
    
} catch {
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "   ERROR EN LA COMPRA" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Status Code: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    
    if ($_.ErrorDetails) {
        Write-Host "Detalles del error:" -ForegroundColor Yellow
        try {
            $errObj = $_.ErrorDetails.Message | ConvertFrom-Json
            Write-Host "   Mensaje: $($errObj.message)" -ForegroundColor White
            if ($errObj.error) {
                Write-Host "   Error: $($errObj.error)" -ForegroundColor White
            }
        } catch {
            Write-Host "   $($_.ErrorDetails.Message)" -ForegroundColor White
        }
    }
    Write-Host ""
    exit
}

# 6. Verificar en Mis Compras
Write-Host "6. Verificando 'Mis Compras'..." -ForegroundColor Yellow
try {
    $misCompras = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headers
    Write-Host "OK - $($misCompras.Count) compras en el historial" -ForegroundColor Green
    
    $ultimaCompra = $misCompras | Sort-Object -Property id -Descending | Select-Object -First 1
    if ($ultimaCompra.id -eq $venta.id) {
        Write-Host "   La compra SI aparece en el historial" -ForegroundColor Green
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "PRUEBA COMPLETADA EXITOSAMENTE" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "El flujo de compra funciona correctamente!" -ForegroundColor Green
Write-Host ""
Write-Host "Ahora prueba en el frontend:" -ForegroundColor Yellow
Write-Host "1. Abre: http://localhost:3000" -ForegroundColor Gray
Write-Host "2. Login: $emailCliente / $passwordCliente" -ForegroundColor Gray
Write-Host "3. Ve a la tienda, agrega productos y compra" -ForegroundColor Gray
Write-Host ""

