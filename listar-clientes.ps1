Write-Host "Listando clientes existentes..." -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090/api"

# Login como admin para obtener todos los clientes
$loginBody = '{"email":"admin@firmeza.com","password":"Admin123$"}'
$login = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method POST -Body $loginBody -ContentType "application/json"

$headers = @{
    "Authorization" = "Bearer $($login.token)"
    "Content-Type" = "application/json"
}

# Obtener clientes
$clientes = Invoke-RestMethod -Uri "$baseUrl/Clientes" -Method GET -Headers $headers

Write-Host "Total de clientes: $($clientes.Count)" -ForegroundColor Green
Write-Host ""

if ($clientes.Count -gt 0) {
    foreach ($cliente in $clientes) {
        Write-Host "ID: $($cliente.id) | Email: $($cliente.email)" -ForegroundColor Yellow
        Write-Host "   Nombre: $($cliente.nombre) $($cliente.apellido)" -ForegroundColor Gray
        Write-Host "   Telefono: $($cliente.telefono)" -ForegroundColor Gray
        Write-Host "   Ciudad: $($cliente.ciudad)" -ForegroundColor Gray
        Write-Host ""
    }
} else {
    Write-Host "No hay clientes registrados" -ForegroundColor Red
}

