# Script para probar autenticación y endpoints
$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   PRUEBA DE AUTENTICACIÓN Y ENDPOINTS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090"

# 1. Login
Write-Host "1️⃣ HACIENDO LOGIN..." -ForegroundColor Yellow
$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/Auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    
    Write-Host "✅ Login exitoso!" -ForegroundColor Green
    Write-Host "📧 Email: $($loginResponse.email)" -ForegroundColor Gray
    Write-Host "👤 Nombre: $($loginResponse.nombreCompleto)" -ForegroundColor Gray
    Write-Host "🔑 Roles: $($loginResponse.roles -join ', ')" -ForegroundColor Gray
    Write-Host "🕒 Expira: $($loginResponse.expiration)" -ForegroundColor Gray
    Write-Host ""
    
    $token = $loginResponse.token
    Write-Host "🔐 Token JWT (primeros 50 caracteres):" -ForegroundColor Gray
    Write-Host $token.Substring(0, [Math]::Min(50, $token.Length)) -ForegroundColor DarkGray
    Write-Host ""
    
    # Headers para las siguientes peticiones
    $headers = @{
        "Authorization" = "Bearer $token"
        "Accept" = "application/json"
    }
    
    # 2. Probar endpoint de Clientes
    Write-Host "2️⃣ CONSULTANDO CLIENTES..." -ForegroundColor Yellow
    try {
        $clientesResponse = Invoke-RestMethod -Uri "$baseUrl/api/Clientes" -Method Get -Headers $headers
        Write-Host "✅ Endpoint de Clientes funciona!" -ForegroundColor Green
        Write-Host "📊 Total de clientes: $($clientesResponse.Count)" -ForegroundColor Gray
        Write-Host ""
    } catch {
        Write-Host "❌ Error en endpoint de Clientes" -ForegroundColor Red
        Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
    }
    
    # 3. Probar endpoint de Ventas
    Write-Host "3️⃣ CONSULTANDO VENTAS..." -ForegroundColor Yellow
    try {
        $ventasResponse = Invoke-RestMethod -Uri "$baseUrl/api/Ventas" -Method Get -Headers $headers
        Write-Host "✅ Endpoint de Ventas funciona!" -ForegroundColor Green
        Write-Host "📊 Total de ventas: $($ventasResponse.Count)" -ForegroundColor Gray
        Write-Host ""
    } catch {
        Write-Host "❌ Error en endpoint de Ventas" -ForegroundColor Red
        Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
    }
    
    # 4. Probar endpoint de Productos
    Write-Host "4️⃣ CONSULTANDO PRODUCTOS..." -ForegroundColor Yellow
    try {
        $productosResponse = Invoke-RestMethod -Uri "$baseUrl/api/Productos" -Method Get -Headers $headers
        Write-Host "✅ Endpoint de Productos funciona!" -ForegroundColor Green
        Write-Host "📊 Total de productos: $($productosResponse.Count)" -ForegroundColor Gray
        Write-Host ""
    } catch {
        Write-Host "❌ Error en endpoint de Productos" -ForegroundColor Red
        Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
    }
    
    # 5. Probar endpoint de Categorías
    Write-Host "5️⃣ CONSULTANDO CATEGORÍAS..." -ForegroundColor Yellow
    try {
        $categoriasResponse = Invoke-RestMethod -Uri "$baseUrl/api/Categorias" -Method Get -Headers $headers
        Write-Host "✅ Endpoint de Categorías funciona!" -ForegroundColor Green
        Write-Host "📊 Total de categorías: $($categoriasResponse.Count)" -ForegroundColor Gray
        Write-Host ""
    } catch {
        Write-Host "❌ Error en endpoint de Categorías" -ForegroundColor Red
        Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Mensaje: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
    }
    
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "          ✅ PRUEBA COMPLETADA" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    
} catch {
    Write-Host "❌ ERROR EN LOGIN:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

Write-Host ""
Write-Host "Presiona Enter para salir..."
Read-Host

