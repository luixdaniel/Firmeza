# Script para registrar un cliente de prueba y probar los endpoints

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "PRUEBA COMPLETA - REGISTRO Y COMPRA" -ForegroundColor Cyan
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
            Write-Host "   Request: $Method $url" -ForegroundColor DarkGray
            $response = Invoke-RestMethod -Uri $url -Method $Method -Headers $headers -Body $jsonBody
        } else {
            Write-Host "   Request: $METHOD $url" -ForegroundColor DarkGray
            $response = Invoke-RestMethod -Uri $url -Method $Method -Headers $headers
        }
        return $response
    } catch {
        Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails.Message) {
            Write-Host "   Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
        }
        return $null
    }
}

# Generar email único para el cliente de prueba
$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$clienteEmail = "cliente_$timestamp@test.com"

Write-Host "📝 Cliente de prueba:" -ForegroundColor White
Write-Host "   Email: $clienteEmail" -ForegroundColor Gray
Write-Host "   Password: Test123$" -ForegroundColor Gray
Write-Host ""

# 1. Registrar nuevo cliente
Write-Host "1️⃣  Registrando nuevo cliente..." -ForegroundColor Yellow
$registerBody = @{
    email = $clienteEmail
    password = "Test123$"
    confirmPassword = "Test123$"
    nombre = "Cliente"
    apellido = "Prueba"
    telefono = "3001234567"
    documento = "1234567890"
    direccion = "Calle 123 45-67"
    ciudad = "Bogota"
    pais = "Colombia"
}

$registerResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/Auth/register" -Body $registerBody

if ($registerResponse) {
    Write-Host "✅ Cliente registrado exitosamente" -ForegroundColor Green
    Write-Host "   Nombre: $($registerResponse.nombreCompleto)" -ForegroundColor Gray
    Write-Host "   Email: $($registerResponse.email)" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "❌ Error al registrar cliente" -ForegroundColor Red
    exit
}

# 2. Login con el cliente recién creado
Write-Host "2️⃣  Iniciando sesión con el cliente..." -ForegroundColor Yellow
$loginBody = @{
    email = $clienteEmail
    password = "Test123$"
}

$loginResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/Auth/login" -Body $loginBody

if ($loginResponse) {
    $token = $loginResponse.token
    Write-Host "✅ Login exitoso" -ForegroundColor Green
    Write-Host "   Token: $($token.Substring(0, 30))..." -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "❌ Login falló" -ForegroundColor Red
    exit
}

# 3. Obtener perfil del cliente
Write-Host "3️⃣  Obteniendo perfil del cliente..." -ForegroundColor Yellow
$perfil = Invoke-ApiRequest -Method "GET" -Endpoint "/Clientes/perfil" -Token $token

if ($perfil) {
    Write-Host "✅ Perfil obtenido exitosamente" -ForegroundColor Green
    Write-Host "   ID: $($perfil.id)" -ForegroundColor Gray
    Write-Host "   Nombre: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Gray
    Write-Host "   Email: $($perfil.email)" -ForegroundColor Gray
    Write-Host "   Teléfono: $($perfil.telefono)" -ForegroundColor Gray
    Write-Host "   Ciudad: $($perfil.ciudad)" -ForegroundColor Gray
    Write-Host "   Documento: $($perfil.documento)" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "❌ No se pudo obtener perfil" -ForegroundColor Red
    Write-Host ""
}

# 4. Obtener lista de productos
Write-Host "4️⃣  Obteniendo productos disponibles..." -ForegroundColor Yellow
$productos = Invoke-ApiRequest -Method "GET" -Endpoint "/Productos" -Token $token

if ($productos -and $productos.Count -gt 0) {
    Write-Host "✅ Productos obtenidos: $($productos.Count) disponibles" -ForegroundColor Green
    Write-Host "   Producto 1: $($productos[0].nombre) - `$$($productos[0].precio)" -ForegroundColor Gray
    if ($productos.Count -gt 1) {
        Write-Host "   Producto 2: $($productos[1].nombre) - `$$($productos[1].precio)" -ForegroundColor Gray
    }
    Write-Host ""
} else {
    Write-Host "❌ No hay productos disponibles" -ForegroundColor Red
    exit
}

# 5. Verificar historial de compras (debe estar vacío)
Write-Host "5️⃣  Verificando historial de compras (inicial)..." -ForegroundColor Yellow
$comprasInicial = Invoke-ApiRequest -Method "GET" -Endpoint "/Ventas/mis-compras" -Token $token

if ($comprasInicial -ne $null) {
    Write-Host "✅ Historial obtenido: $($comprasInicial.Count) compras" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "❌ Error al obtener historial" -ForegroundColor Red
    Write-Host ""
}

# 6. Crear una venta con múltiples productos
Write-Host "6️⃣  Creando venta de prueba..." -ForegroundColor Yellow

$detalles = @()
$detalles += @{
    productoId = $productos[0].id
    cantidad = 2
    precioUnitario = $productos[0].precio
}

if ($productos.Count -gt 1) {
    $detalles += @{
        productoId = $productos[1].id
        cantidad = 1
        precioUnitario = $productos[1].precio
    }
}

$ventaBody = @{
    metodoPago = "Tarjeta"
    detalles = $detalles
}

$venta = Invoke-ApiRequest -Method "POST" -Endpoint "/Ventas" -Body $ventaBody -Token $token

if ($venta) {
    Write-Host "✅ Venta creada exitosamente" -ForegroundColor Green
    Write-Host "   ID: $($venta.id)" -ForegroundColor Gray
    Write-Host "   Cliente: $($venta.clienteNombre)" -ForegroundColor Gray
    Write-Host "   Total: `$$($venta.total)" -ForegroundColor Gray
    Write-Host "   Productos: $($venta.detalles.Count)" -ForegroundColor Gray
    foreach ($detalle in $venta.detalles) {
        Write-Host "      - $($detalle.productoNombre) x$($detalle.cantidad) = `$$($detalle.subtotal)" -ForegroundColor DarkGray
    }
    Write-Host ""
} else {
    Write-Host "❌ Error al crear venta" -ForegroundColor Red
    Write-Host ""
}

# 7. Verificar historial de compras actualizado
Write-Host "7️⃣  Verificando historial actualizado..." -ForegroundColor Yellow
$comprasFinal = Invoke-ApiRequest -Method "GET" -Endpoint "/Ventas/mis-compras" -Token $token

if ($comprasFinal -and $comprasFinal.Count -gt 0) {
    Write-Host "✅ Historial actualizado: $($comprasFinal.Count) compras" -ForegroundColor Green
    Write-Host "   Última compra:" -ForegroundColor Gray
    $ultimaCompra = $comprasFinal[0]
    Write-Host "      ID: $($ultimaCompra.id)" -ForegroundColor DarkGray
    Write-Host "      Fecha: $($ultimaCompra.fecha)" -ForegroundColor DarkGray
    Write-Host "      Total: `$$($ultimaCompra.total)" -ForegroundColor DarkGray
    Write-Host "      Productos: $($ultimaCompra.detalles.Count)" -ForegroundColor DarkGray
    Write-Host ""
} else {
    Write-Host "❌ No se actualizó el historial" -ForegroundColor Red
    Write-Host ""
}

# 8. Obtener detalle de la venta creada
if ($venta) {
    Write-Host "8️⃣  Obteniendo detalle de la venta..." -ForegroundColor Yellow
    $ventaDetalle = Invoke-ApiRequest -Method "GET" -Endpoint "/Ventas/$($venta.id)" -Token $token
    
    if ($ventaDetalle) {
        Write-Host "✅ Detalle obtenido exitosamente" -ForegroundColor Green
        Write-Host "   ID: $($ventaDetalle.id)" -ForegroundColor Gray
        Write-Host "   Cliente: $($ventaDetalle.clienteNombre)" -ForegroundColor Gray
        Write-Host "   Fecha: $($ventaDetalle.fecha)" -ForegroundColor Gray
        Write-Host "   Total: `$$($ventaDetalle.total)" -ForegroundColor Gray
        Write-Host "   Detalles:" -ForegroundColor Gray
        foreach ($det in $ventaDetalle.detalles) {
            Write-Host "      • $($det.productoNombre)" -ForegroundColor DarkGray
            Write-Host "        Cantidad: $($det.cantidad) | Precio: `$$($det.precioUnitario) | Subtotal: `$$($det.subtotal)" -ForegroundColor DarkGray
        }
        Write-Host ""
    } else {
        Write-Host "❌ Error al obtener detalle" -ForegroundColor Red
        Write-Host ""
    }
}

# Resumen Final
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "           RESUMEN COMPLETO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📊 Resultados de las pruebas:" -ForegroundColor White
Write-Host ""
Write-Host "$(if ($registerResponse) { '✅' } else { '❌' }) Registro de cliente" -ForegroundColor $(if ($registerResponse) { 'Green' } else { 'Red' })
Write-Host "$(if ($loginResponse) { '✅' } else { '❌' }) Login del cliente" -ForegroundColor $(if ($loginResponse) { 'Green' } else { 'Red' })
Write-Host "$(if ($perfil) { '✅' } else { '❌' }) Obtener perfil del cliente" -ForegroundColor $(if ($perfil) { 'Green' } else { 'Red' })
Write-Host "$(if ($productos) { '✅' } else { '❌' }) Listar productos" -ForegroundColor $(if ($productos) { 'Green' } else { 'Red' })
Write-Host "$(if ($comprasInicial -ne $null) { '✅' } else { '❌' }) Obtener historial de compras (inicial)" -ForegroundColor $(if ($comprasInicial -ne $null) { 'Green' } else { 'Red' })
Write-Host "$(if ($venta) { '✅' } else { '❌' }) Crear nueva venta" -ForegroundColor $(if ($venta) { 'Green' } else { 'Red' })
Write-Host "$(if ($comprasFinal) { '✅' } else { '❌' }) Obtener historial actualizado" -ForegroundColor $(if ($comprasFinal) { 'Green' } else { 'Red' })
Write-Host "$(if ($ventaDetalle) { '✅' } else { '❌' }) Obtener detalle de venta" -ForegroundColor $(if ($ventaDetalle) { 'Green' } else { 'Red' })
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($registerResponse -and $loginResponse -and $perfil -and $venta -and $comprasFinal) {
    Write-Host "🎉 ¡TODAS LAS PRUEBAS PASARON EXITOSAMENTE!" -ForegroundColor Green
    Write-Host ""
    Write-Host "✨ El sistema está completamente funcional:" -ForegroundColor White
    Write-Host "   • Registro de clientes ✅" -ForegroundColor Gray
    Write-Host "   • Autenticación ✅" -ForegroundColor Gray
    Write-Host "   • Perfil de cliente ✅" -ForegroundColor Gray
    Write-Host "   • Listado de productos ✅" -ForegroundColor Gray
    Write-Host "   • Creación de ventas ✅" -ForegroundColor Gray
    Write-Host "   • Historial de compras ✅" -ForegroundColor Gray
    Write-Host ""
    Write-Host "🌐 Ahora puedes probar el frontend en:" -ForegroundColor Cyan
    Write-Host "   http://localhost:3000" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "🔐 Credenciales del cliente de prueba:" -ForegroundColor Cyan
    Write-Host "   Email: $clienteEmail" -ForegroundColor Yellow
    Write-Host "   Password: Test123$" -ForegroundColor Yellow
} else {
    Write-Host "⚠️  Algunas pruebas fallaron" -ForegroundColor Yellow
    Write-Host "Revisa los mensajes de error arriba para más detalles." -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan

