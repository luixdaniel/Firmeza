$baseUrl = "http://localhost:5090/api"

Write-Host "=== TEST API COMPLETO ===" -ForegroundColor Cyan
Write-Host ""

# 1. Login como Admin
Write-Host "1. Login como Admin..." -ForegroundColor Yellow
$loginBody = '{"email":"admin@firmeza.com","password":"Admin123$"}'
$login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"
$token = $login.token
Write-Host "OK - Token obtenido" -ForegroundColor Green
Write-Host ""

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. Crear cliente manualmente (como Admin)
Write-Host "2. Creando cliente como Admin..." -ForegroundColor Yellow
$timestamp = Get-Date -Format "HHmmss"
$emailCliente = "cliente_$timestamp@test.com"

$clienteBody = @{
    nombre = "Cliente"
    apellido = "Test"
    email = $emailCliente
    telefono = "3001234567"
    documento = "123456789"
    direccion = "Calle Test 123"
    ciudad = "Bogota"
    pais = "Colombia"
} | ConvertTo-Json

try {
    $cliente = Invoke-RestMethod -Uri "$baseUrl/Clientes" -Method POST -Body $clienteBody -Headers $headers
    Write-Host "OK - Cliente creado con ID: $($cliente.id)" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails) { Write-Host $_.ErrorDetails.Message -ForegroundColor Red }
    exit
}
Write-Host ""

# 3. Crear usuario para el cliente
Write-Host "3. Registrando usuario para el cliente..." -ForegroundColor Yellow
$regBody = @{
    email = $emailCliente
    password = "Test123`$"
    confirmPassword = "Test123`$"
    nombre = "Cliente"
    apellido = "Test"
    telefono = "3001234567"
    documento = "123456789"
    direccion = "Calle Test 123"
    ciudad = "Bogota"
    pais = "Colombia"
} | ConvertTo-Json

try {
    $reg = Invoke-RestMethod -Uri "$baseUrl/Auth/register" -Method POST -Body $regBody -ContentType "application/json"
    Write-Host "OK - Usuario registrado" -ForegroundColor Green
    $tokenCliente = $reg.token
} catch {
    Write-Host "FALLO - Pero continuaremos con login manual" -ForegroundColor Yellow
    # Intentar login si el usuario ya existe
    $loginClienteBody = @{
        email = $emailCliente
        password = "Test123`$"
    } | ConvertTo-Json
    
    try {
        $loginCliente = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginClienteBody -ContentType "application/json"
        $tokenCliente = $loginCliente.token
        Write-Host "OK - Login exitoso con usuario existente" -ForegroundColor Green
    } catch {
        Write-Host "ERROR - No se pudo obtener token del cliente" -ForegroundColor Red
        Write-Host "Continuaremos usando el token de admin..." -ForegroundColor Yellow
        $tokenCliente = $token
    }
}
Write-Host ""

$headersCliente = @{
    "Authorization" = "Bearer $tokenCliente"
    "Content-Type" = "application/json"
}

# 4. Obtener perfil
Write-Host "4. Obteniendo perfil..." -ForegroundColor Yellow
try {
    $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headersCliente
    Write-Host "OK - Perfil: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Green
    $clienteId = $perfil.id
} catch {
    Write-Host "ADVERTENCIA - No se pudo obtener perfil, usaremos el cliente creado" -ForegroundColor Yellow
    $clienteId = $cliente.id
}
Write-Host ""

# 5. Obtener productos
Write-Host "5. Obteniendo productos..." -ForegroundColor Yellow
$productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET
Write-Host "OK - $($productos.Count) productos disponibles" -ForegroundColor Green
Write-Host ""

# 6. Crear venta
Write-Host "6. Creando venta..." -ForegroundColor Yellow
Write-Host "Producto: $($productos[0].nombre) - Precio: $($productos[0].precio)" -ForegroundColor DarkGray

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

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headersCliente
    Write-Host "OK - Venta creada!" -ForegroundColor Green
    Write-Host "   ID: $($venta.id)" -ForegroundColor Gray
    Write-Host "   Total: $($venta.total)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails) { Write-Host $_.ErrorDetails.Message -ForegroundColor Red }
}
Write-Host ""

# 7. Obtener mis compras
Write-Host "7. Obteniendo mis compras..." -ForegroundColor Yellow
try {
    $compras = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headersCliente
    Write-Host "OK - $($compras.Count) compras en historial" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "=== PRUEBAS COMPLETADAS ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Cliente de prueba:" -ForegroundColor White
Write-Host "Email: $emailCliente" -ForegroundColor Yellow
Write-Host "Password: Test123`$" -ForegroundColor Yellow

