Write-Host "================================" -ForegroundColor Cyan
Write-Host "PRUEBA DE COMPRA CON CLIENTE" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"

# Verificar que la API esté corriendo
Write-Host "Verificando API..." -ForegroundColor Yellow
try {
    $test = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET -TimeoutSec 3 -ErrorAction Stop
    Write-Host "OK - API corriendo en puerto 5090" -ForegroundColor Green
} catch {
    Write-Host "ERROR - La API no esta corriendo" -ForegroundColor Red
    exit
}
Write-Host ""

# 1. Registrar un nuevo cliente
Write-Host "1. Registrando nuevo cliente..." -ForegroundColor Yellow
$timestamp = Get-Date -Format "HHmmss"
$emailCliente = "cliente_$timestamp@test.com"

$registerBody = @{
    email = $emailCliente
    password = "Test123`$"
    confirmPassword = "Test123`$"
    nombre = "Cliente"
    apellido = "Prueba"
    telefono = "3001234567"
    documento = "123456789"
    direccion = "Calle Test 123"
    ciudad = "Bogota"
    pais = "Colombia"
} | ConvertTo-Json

try {
    $registro = Invoke-RestMethod -Uri "$baseUrl/Auth/register" -Method POST -Body $registerBody -ContentType "application/json"
    $token = $registro.token
    Write-Host "OK - Cliente registrado exitosamente" -ForegroundColor Green
    Write-Host "   Email: $emailCliente" -ForegroundColor Gray
    Write-Host "   Nombre: $($registro.nombreCompleto)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - No se pudo registrar el cliente" -ForegroundColor Red
    Write-Host "   $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. Verificar perfil del cliente
Write-Host "2. Verificando perfil del cliente..." -ForegroundColor Yellow
try {
    $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headers
    Write-Host "OK - Perfil obtenido" -ForegroundColor Green
    Write-Host "   ID: $($perfil.id)" -ForegroundColor Gray
    Write-Host "   Nombre: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Gray
    Write-Host "   Email: $($perfil.email)" -ForegroundColor Gray
    Write-Host "   Ciudad: $($perfil.ciudad)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - No se pudo obtener el perfil" -ForegroundColor Red
    Write-Host "   $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

# 3. Obtener productos disponibles
Write-Host "3. Obteniendo productos..." -ForegroundColor Yellow
try {
    $productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
    $productosConStock = $productos | Where-Object { $_.stock -gt 0 }
    
    Write-Host "OK - $($productos.Count) productos disponibles" -ForegroundColor Green
    Write-Host "   Productos con stock: $($productosConStock.Count)" -ForegroundColor Gray
    
    if ($productosConStock.Count -eq 0) {
        Write-Host "ERROR - No hay productos con stock" -ForegroundColor Red
        exit
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

# 4. Crear carrito de compras
Write-Host "4. Creando carrito de compras..." -ForegroundColor Yellow
$carrito = @()

# Agregar primer producto
$producto1 = $productosConStock[0]
$carrito += @{
    productoId = $producto1.id
    cantidad = 2
    precioUnitario = $producto1.precio
}

Write-Host "OK - Carrito creado con:" -ForegroundColor Green
Write-Host "   Producto: $($producto1.nombre)" -ForegroundColor Gray
Write-Host "   Precio unitario: `$$($producto1.precio)" -ForegroundColor Gray
Write-Host "   Cantidad: 2" -ForegroundColor Gray
Write-Host "   Stock disponible: $($producto1.stock)" -ForegroundColor Gray
Write-Host "   Subtotal: `$$($producto1.precio * 2)" -ForegroundColor Gray

# Agregar segundo producto si hay más
if ($productosConStock.Count -gt 1) {
    $producto2 = $productosConStock[1]
    $carrito += @{
        productoId = $producto2.id
        cantidad = 1
        precioUnitario = $producto2.precio
    }
    Write-Host "   Producto: $($producto2.nombre)" -ForegroundColor Gray
    Write-Host "   Precio unitario: `$$($producto2.precio)" -ForegroundColor Gray
    Write-Host "   Cantidad: 1" -ForegroundColor Gray
    Write-Host "   Subtotal: `$$($producto2.precio)" -ForegroundColor Gray
}

$totalCarrito = ($carrito | ForEach-Object { $_.cantidad * $_.precioUnitario } | Measure-Object -Sum).Sum
Write-Host "   TOTAL CARRITO: `$$totalCarrito" -ForegroundColor Cyan
Write-Host ""

# 5. Finalizar compra
Write-Host "5. Finalizando compra..." -ForegroundColor Yellow
$ventaBody = @{
    metodoPago = "Tarjeta"
    detalles = $carrito
} | ConvertTo-Json -Depth 10

Write-Host "   Enviando datos de la venta..." -ForegroundColor DarkGray

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headers -ErrorAction Stop
    
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "COMPRA REALIZADA EXITOSAMENTE!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Detalles de la compra:" -ForegroundColor Cyan
    Write-Host "   ID de Venta: #$($venta.id)" -ForegroundColor White
    Write-Host "   Cliente: $($venta.clienteNombre)" -ForegroundColor White
    Write-Host "   Fecha: $($venta.fecha)" -ForegroundColor White
    Write-Host "   Metodo de pago: Tarjeta" -ForegroundColor White
    Write-Host "   Total: `$$($venta.total)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Productos comprados:" -ForegroundColor Cyan
    foreach ($detalle in $venta.detalles) {
        Write-Host "   - $($detalle.productoNombre)" -ForegroundColor White
        Write-Host "     Cantidad: $($detalle.cantidad) x `$$($detalle.precioUnitario) = `$$($detalle.subtotal)" -ForegroundColor Gray
    }
    Write-Host ""
    
    $ventaId = $venta.id
} catch {
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "ERROR AL PROCESAR LA COMPRA" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.ErrorDetails -and $_.ErrorDetails.Message) {
        Write-Host ""
        Write-Host "Detalles del servidor:" -ForegroundColor Red
        try {
            $errorObj = $_.ErrorDetails.Message | ConvertFrom-Json
            if ($errorObj.message) {
                Write-Host "   $($errorObj.message)" -ForegroundColor Yellow
            }
            if ($errorObj.error) {
                Write-Host "   Error: $($errorObj.error)" -ForegroundColor Yellow
            }
        } catch {
            Write-Host "   $($_.ErrorDetails.Message)" -ForegroundColor Yellow
        }
    }
    
    Write-Host ""
    Write-Host "Datos enviados:" -ForegroundColor Gray
    Write-Host $ventaBody -ForegroundColor DarkGray
    exit
}

# 6. Verificar que la compra aparezca en "Mis Compras"
Write-Host "6. Verificando historial de compras..." -ForegroundColor Yellow
try {
    $misCompras = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headers
    
    Write-Host "OK - Historial obtenido" -ForegroundColor Green
    Write-Host "   Total de compras: $($misCompras.Count)" -ForegroundColor Gray
    
    $compraEncontrada = $misCompras | Where-Object { $_.id -eq $ventaId }
    if ($compraEncontrada) {
        Write-Host "   La compra SI aparece en el historial" -ForegroundColor Green
    } else {
        Write-Host "   ADVERTENCIA - La compra NO aparece en el historial" -ForegroundColor Yellow
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# 7. Verificar actualización de stock
Write-Host "7. Verificando actualizacion de stock..." -ForegroundColor Yellow
try {
    $productosActualizados = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
    
    $prod1Actualizado = $productosActualizados | Where-Object { $_.id -eq $producto1.id }
    $stockAntes1 = $producto1.stock
    $stockDespues1 = $prod1Actualizado.stock
    $diferencia1 = $stockAntes1 - $stockDespues1
    
    Write-Host "OK - Stock actualizado" -ForegroundColor Green
    Write-Host "   $($producto1.nombre):" -ForegroundColor Gray
    Write-Host "      Stock antes: $stockAntes1" -ForegroundColor DarkGray
    Write-Host "      Stock despues: $stockDespues1" -ForegroundColor DarkGray
    Write-Host "      Unidades vendidas: $diferencia1" -ForegroundColor $(if ($diferencia1 -eq 2) { 'Green' } else { 'Yellow' })
    
    if ($productosConStock.Count -gt 1) {
        $prod2Actualizado = $productosActualizados | Where-Object { $_.id -eq $producto2.id }
        $stockAntes2 = $producto2.stock
        $stockDespues2 = $prod2Actualizado.stock
        $diferencia2 = $stockAntes2 - $stockDespues2
        
        Write-Host "   $($producto2.nombre):" -ForegroundColor Gray
        Write-Host "      Stock antes: $stockAntes2" -ForegroundColor DarkGray
        Write-Host "      Stock despues: $stockDespues2" -ForegroundColor DarkGray
        Write-Host "      Unidades vendidas: $diferencia2" -ForegroundColor $(if ($diferencia2 -eq 1) { 'Green' } else { 'Yellow' })
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "RESUMEN FINAL" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Credenciales del cliente de prueba:" -ForegroundColor White
Write-Host "   Email: $emailCliente" -ForegroundColor Yellow
Write-Host "   Password: Test123`$" -ForegroundColor Yellow
Write-Host ""
Write-Host "Ahora puedes probar en el frontend:" -ForegroundColor Cyan
Write-Host "1. Abre http://localhost:3000" -ForegroundColor Gray
Write-Host "2. Haz login con las credenciales de arriba" -ForegroundColor Gray
Write-Host "3. Ve a la tienda y agrega productos al carrito" -ForegroundColor Gray
Write-Host "4. Ve al carrito y finaliza la compra" -ForegroundColor Gray
Write-Host "5. Verifica en 'Mis Compras' que aparezca tu pedido" -ForegroundColor Gray
Write-Host ""
Write-Host "O registra un nuevo cliente en:" -ForegroundColor Cyan
Write-Host "http://localhost:3000/auth/register" -ForegroundColor Yellow
Write-Host ""

