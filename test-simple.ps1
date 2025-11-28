Write-Host "================================" -ForegroundColor Cyan
Write-Host "PRUEBA COMPLETA FLUJO CLIENTE" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"

# Generar email unico
$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$email = "cliente_$timestamp@test.com"

Write-Host "Cliente de prueba: $email" -ForegroundColor Yellow
Write-Host ""

# 1. REGISTRO
Write-Host "1. Registrando cliente..." -ForegroundColor Yellow
$regBody = @{
    email = $email
    password = "Test123`$"
    confirmPassword = "Test123`$"
    nombre = "Cliente"
    apellido = "Prueba"
    telefono = "3001234567"
    documento = "1234567890"
    direccion = "Calle 123"
    ciudad = "Bogota"
    pais = "Colombia"
} | ConvertTo-Json

try {
    $reg = Invoke-RestMethod -Uri "$baseUrl/Auth/register" -Method POST -Body $regBody -ContentType "application/json"
    Write-Host "OK - Cliente registrado" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}

# 2. LOGIN
Write-Host "2. Login..." -ForegroundColor Yellow
$loginBody = @{
    email = $email
    password = "Test123`$"
} | ConvertTo-Json

try {
    $login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $login.token
    Write-Host "OK - Token obtenido" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 3. PERFIL
Write-Host "3. Obteniendo perfil..." -ForegroundColor Yellow
try {
    $perfil = Invoke-RestMethod -Uri "$baseUrl/Clientes/perfil" -Method GET -Headers $headers
    Write-Host "OK - Perfil obtenido: $($perfil.nombre) $($perfil.apellido)" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}

# 4. PRODUCTOS
Write-Host "4. Obteniendo productos..." -ForegroundColor Yellow
try {
    $productos = Invoke-RestMethod -Uri "$baseUrl/Productos" -Method GET -Headers $headers
    Write-Host "OK - $($productos.Count) productos encontrados" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    exit
}

# 5. COMPRAS INICIAL
Write-Host "5. Verificando historial inicial..." -ForegroundColor Yellow
try {
    $compras1 = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headers
    Write-Host "OK - $($compras1.Count) compras en historial" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}

# 6. CREAR VENTA
Write-Host "6. Creando venta..." -ForegroundColor Yellow
Write-Host "Producto: $($productos[0].nombre) - Precio: $($productos[0].precio) - Stock: $($productos[0].stock)" -ForegroundColor DarkGray
$ventaBody = @{
    metodoPago = "Efectivo"
    detalles = @(
        @{
            productoId = $productos[0].id
            cantidad = 2
            precioUnitario = $productos[0].precio
        }
    )
} | ConvertTo-Json -Depth 10

Write-Host "Body: $ventaBody" -ForegroundColor DarkGray

try {
    $venta = Invoke-RestMethod -Uri "$baseUrl/Ventas" -Method POST -Body $ventaBody -Headers $headers
    Write-Host "OK - Venta creada ID: $($venta.id) Total: $($venta.total)" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails.Message) {
        Write-Host "Detalles JSON: $($_.ErrorDetails.Message)" -ForegroundColor Red
    }
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response Body: $responseBody" -ForegroundColor Red
    }
}

# 7. COMPRAS FINAL
Write-Host "7. Verificando historial actualizado..." -ForegroundColor Yellow
try {
    $compras2 = Invoke-RestMethod -Uri "$baseUrl/Ventas/mis-compras" -Method GET -Headers $headers
    Write-Host "OK - $($compras2.Count) compras en historial" -ForegroundColor Green
} catch {
    Write-Host "ERROR - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "PRUEBAS COMPLETADAS" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Credenciales del cliente:" -ForegroundColor White
Write-Host "Email: $email" -ForegroundColor Yellow
Write-Host "Password: Test123`$" -ForegroundColor Yellow
Write-Host ""

