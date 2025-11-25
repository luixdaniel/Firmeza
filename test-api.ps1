# Script para probar la autenticación y consulta de ventas

$baseUrl = "http://localhost:5090"

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "PRUEBA DE AUTENTICACIÓN Y VENTAS" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. Login
Write-Host "1. Intentando hacer login..." -ForegroundColor Yellow
$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/Auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    
    Write-Host "✅ Login exitoso!" -ForegroundColor Green
    Write-Host "Token: $($loginResponse.token.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host "Email: $($loginResponse.email)" -ForegroundColor Gray
    Write-Host "Roles: $($loginResponse.roles -join ', ')" -ForegroundColor Gray
    Write-Host ""
    
    $token = $loginResponse.token
    
    # 2. Consultar ventas
    Write-Host "2. Consultando ventas con el token..." -ForegroundColor Yellow
    
    $headers = @{
        "Authorization" = "Bearer $token"
        "Accept" = "application/json"
    }
    
    $ventasResponse = Invoke-RestMethod -Uri "$baseUrl/api/Ventas" -Method Get -Headers $headers
    
    Write-Host "✅ Consulta de ventas exitosa!" -ForegroundColor Green
    Write-Host "Cantidad de ventas: $($ventasResponse.Count)" -ForegroundColor Gray
    
    if ($ventasResponse.Count -gt 0) {
        Write-Host ""
        Write-Host "Primera venta:" -ForegroundColor Cyan
        $ventasResponse[0] | Format-List
    } else {
        Write-Host "⚠️ No hay ventas en la base de datos" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Cyan
    Write-Host "✅ TODAS LAS PRUEBAS EXITOSAS" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Cyan
    
} catch {
    Write-Host "❌ ERROR:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Respuesta del servidor: $responseBody" -ForegroundColor Red
    }
}

