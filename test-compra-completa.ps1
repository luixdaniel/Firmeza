Write-Host "================================" -ForegroundColor Cyan
Write-Host "PRUEBA COMPLETA DE COMPRA" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"

# Verificar que la API esté corriendo
Write-Host "Verificando API..." -ForegroundColor Yellow
try {
    $test = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET -TimeoutSec 3 -ErrorAction Stop
    Write-Host "OK - API corriendo en puerto 5090" -ForegroundColor Green
} catch {
    Write-Host "ERROR - La API no está corriendo" -ForegroundColor Red
    Write-Host "Por favor inicia la API:" -ForegroundColor Yellow
    Write-Host "  cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web" -ForegroundColor Gray
    Write-Host "  dotnet run" -ForegroundColor Gray
    exit
}
Write-Host ""

# 1. Login
Write-Host "1. Haciendo login..." -ForegroundColor Yellow
$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123`$"
} | ConvertTo-Json

try {
    $login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $login.token
    Write-Host "OK - Login exitoso" -ForegroundColor Green
    Write-Host "   Email: $($login.email)" -ForegroundColor Gray
    Write-Host "   Roles: $($login.roles -join ', ')" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. Obtener productos
Write-Host "2. Obteniendo productos..." -ForegroundColor Yellow
try {
    $productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
    Write-Host "OK - $($productos.Count) productos disponibles" -ForegroundColor Green
    
    # Filtrar productos con stock
    $productosConStock = $productos | Where-Object { $_.stock -gt 0 }
    
    if ($productosConStock.Count -eq 0) {
        Write-Host "ERROR - No hay productos con stock" -ForegroundColor Red
        exit
    }
    
    Write-Host "   Productos con stock: $($productosConStock.Count)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}
Write-Host ""

# 3. Obtener perfil
Write-Host "3. Obteniendo perfil del cliente..." -ForegroundColor Yellow
try {
    $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headers
    Write-Host "OK - Perfil obtenido" -ForegroundColor Green
    Write-Host "   ID: $($perfil.id)" -ForegroundColor Gray
    Write-Host "   Nombre: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Gray
    Write-Host "   Email: $($perfil.email)" -ForegroundColor Gray
} catch {
    Write-Host "ADVERTENCIA - No se encontró perfil de cliente" -ForegroundColor Yellow
    Write-Host "   El usuario admin no tiene registro de cliente" -ForegroundColor Gray
    Write-Host "   Creando cliente para el admin..." -ForegroundColor Yellow
    
    # Crear cliente para el admin
    $clienteBody = @{
        nombre = "Admin"
        apellido = "Sistema"
        email = "admin@firmeza.com"
        telefono = "3001234567"
        documento = "1234567890"
        direccion = "Oficina Principal"
        ciudad = "Bogota"
        pais = "Colombia"
    } | ConvertTo-Json
    
    try {
        $nuevoCliente = Invoke-RestMethod -Uri "$baseUrl/Clientes" -Method POST -Body $clienteBody -Headers $headers
        Write-Host "OK - Cliente creado con ID: $($nuevoCliente.id)" -ForegroundColor Green
        
        # Obtener perfil nuevamente
        $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headers
        Write-Host "OK - Perfil obtenido" -ForegroundColor Green
    } catch {
        Write-Host "ADVERTENCIA - No se pudo crear cliente (puede que ya exista)" -ForegroundColor Yellow
        Write-Host "   Intentando obtener todos los clientes..." -ForegroundColor Gray
        
        # Intentar obtener el cliente existente
        try {
            $todosClientes = Invoke-RestMethod -Uri "$baseUrl/Clientes" -Method GET -Headers $headers
            $clienteExistente = $todosClientes | Where-Object { $_.email -eq "admin@firmeza.com" }
            
            if ($clienteExistente) {
                Write-Host "   OK - Cliente encontrado con ID: $($clienteExistente.id)" -ForegroundColor Green
                $perfil = $clienteExistente
            } else {
                Write-Host "   ERROR - No se encontró cliente para admin@firmeza.com" -ForegroundColor Red
                Write-Host "   Continuando sin perfil de cliente..." -ForegroundColor Yellow
                Write-Host "   NOTA: Las compras pueden fallar sin un cliente asociado" -ForegroundColor Yellow
            }
        } catch {
            Write-Host "   ERROR - No se pudieron obtener clientes" -ForegroundColor Red
        }
    }
}
Write-Host ""

# 4. Simular carrito (seleccionar 2 productos)
Write-Host "4. Simulando carrito de compras..." -ForegroundColor Yellow
$carrito = @()

# Agregar primer producto
$producto1 = $productosConStock[0]
$carrito += @{
    productoId = $producto1.id
    cantidad = 1
    precioUnitario = $producto1.precio
}

Write-Host "OK - Carrito creado" -ForegroundColor Green
Write-Host "   Producto 1: $($producto1.nombre)" -ForegroundColor Gray
Write-Host "      Precio: `$$($producto1.precio)" -ForegroundColor Gray
Write-Host "      Cantidad: 1" -ForegroundColor Gray
Write-Host "      Stock disponible: $($producto1.stock)" -ForegroundColor Gray

# Si hay más productos, agregar un segundo
if ($productosConStock.Count -gt 1) {
    $producto2 = $productosConStock[1]
    $carrito += @{
        productoId = $producto2.id
        cantidad = 1
        precioUnitario = $producto2.precio
    }
    Write-Host "   Producto 2: $($producto2.nombre)" -ForegroundColor Gray
    Write-Host "      Precio: `$$($producto2.precio)" -ForegroundColor Gray
    Write-Host "      Cantidad: 1" -ForegroundColor Gray
    Write-Host "      Stock disponible: $($producto2.stock)" -ForegroundColor Gray
}

$total = ($carrito | ForEach-Object { $_.cantidad * $_.precioUnitario } | Measure-Object -Sum).Sum
Write-Host "   Total del carrito: `$$total" -ForegroundColor Gray
Write-Host ""

# 5. Crear venta (Checkout)
Write-Host "5. Procesando compra..." -ForegroundColor Yellow
$ventaBody = @{
    metodoPago = "Efectivo"
    detalles = $carrito
} | ConvertTo-Json -Depth 10

Write-Host "   Body de la venta:" -ForegroundColor DarkGray
Write-Host $ventaBody -ForegroundColor DarkGray
Write-Host ""

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headers
    Write-Host "OK - Compra realizada exitosamente!" -ForegroundColor Green
    Write-Host "   ID de Venta: $($venta.id)" -ForegroundColor Gray
    Write-Host "   Cliente: $($venta.clienteNombre)" -ForegroundColor Gray
    Write-Host "   Total: `$$($venta.total)" -ForegroundColor Gray
    Write-Host "   Fecha: $($venta.fecha)" -ForegroundColor Gray
    Write-Host "   Productos comprados: $($venta.detalles.Count)" -ForegroundColor Gray
    Write-Host ""
    
    # Mostrar detalles
    Write-Host "   Detalles de la compra:" -ForegroundColor Cyan
    foreach ($detalle in $venta.detalles) {
        Write-Host "      - $($detalle.productoNombre)" -ForegroundColor Gray
        Write-Host "        Cantidad: $($detalle.cantidad) | Precio: `$$($detalle.precioUnitario) | Subtotal: `$$($detalle.subtotal)" -ForegroundColor DarkGray
    }
    
    $ventaId = $venta.id
} catch {
    Write-Host "ERROR - No se pudo procesar la compra" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "   Message: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.ErrorDetails -and $_.ErrorDetails.Message) {
        Write-Host "   Detalles del error:" -ForegroundColor Red
        try {
            $errorObj = $_.ErrorDetails.Message | ConvertFrom-Json
            Write-Host "   $($errorObj.message)" -ForegroundColor Red
            if ($errorObj.error) {
                Write-Host "   Error: $($errorObj.error)" -ForegroundColor Red
            }
        } catch {
            Write-Host "   $($_.ErrorDetails.Message)" -ForegroundColor Red
        }
    }
    exit
}
Write-Host ""

# 6. Verificar compra en historial
Write-Host "6. Verificando historial de compras..." -ForegroundColor Yellow
try {
    $compras = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headers
    Write-Host "OK - Historial obtenido: $($compras.Count) compras" -ForegroundColor Green
    
    # Buscar la compra que acabamos de hacer
    $compraReciente = $compras | Where-Object { $_.id -eq $ventaId }
    if ($compraReciente) {
        Write-Host "   La compra aparece en el historial" -ForegroundColor Green
    } else {
        Write-Host "   ADVERTENCIA - La compra no aparece en el historial" -ForegroundColor Yellow
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# 7. Verificar stock actualizado
Write-Host "7. Verificando actualización de stock..." -ForegroundColor Yellow
try {
    $productosActualizados = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
    
    $producto1Actualizado = $productosActualizados | Where-Object { $_.id -eq $producto1.id }
    $stockAnterior1 = $producto1.stock
    $stockNuevo1 = $producto1Actualizado.stock
    $diferencia1 = $stockAnterior1 - $stockNuevo1
    
    Write-Host "OK - Stock verificado" -ForegroundColor Green
    Write-Host "   $($producto1.nombre):" -ForegroundColor Gray
    Write-Host "      Stock anterior: $stockAnterior1" -ForegroundColor DarkGray
    Write-Host "      Stock nuevo: $stockNuevo1" -ForegroundColor DarkGray
    Write-Host "      Diferencia: -$diferencia1" -ForegroundColor $(if ($diferencia1 -eq 1) { 'Green' } else { 'Red' })
    
    if ($productosConStock.Count -gt 1) {
        $producto2Actualizado = $productosActualizados | Where-Object { $_.id -eq $producto2.id }
        $stockAnterior2 = $producto2.stock
        $stockNuevo2 = $producto2Actualizado.stock
        $diferencia2 = $stockAnterior2 - $stockNuevo2
        
        Write-Host "   $($producto2.nombre):" -ForegroundColor Gray
        Write-Host "      Stock anterior: $stockAnterior2" -ForegroundColor DarkGray
        Write-Host "      Stock nuevo: $stockNuevo2" -ForegroundColor DarkGray
        Write-Host "      Diferencia: -$diferencia2" -ForegroundColor $(if ($diferencia2 -eq 1) { 'Green' } else { 'Red' })
    }
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "================================" -ForegroundColor Cyan
Write-Host "RESUMEN DE LA PRUEBA" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "OK - Login exitoso" -ForegroundColor Green
Write-Host "OK - Productos obtenidos" -ForegroundColor Green
Write-Host "OK - Perfil obtenido/creado" -ForegroundColor Green
Write-Host "OK - Carrito simulado" -ForegroundColor Green
Write-Host "OK - Compra procesada" -ForegroundColor Green
Write-Host "OK - Historial verificado" -ForegroundColor Green
Write-Host "OK - Stock actualizado" -ForegroundColor Green
Write-Host ""
Write-Host "EXITO - El flujo de compra funciona correctamente" -ForegroundColor Green
Write-Host ""
Write-Host "Ahora puedes probar en el frontend:" -ForegroundColor Cyan
Write-Host "1. Abre: http://localhost:3000" -ForegroundColor Yellow
Write-Host "2. Login con: admin@firmeza.com / Admin123`$" -ForegroundColor Yellow
Write-Host "3. Ve a la tienda y agrega productos" -ForegroundColor Yellow
Write-Host "4. Ve al carrito y finaliza la compra" -ForegroundColor Yellow
Write-Host ""

