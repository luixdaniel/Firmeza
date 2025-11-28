$baseUrl = "http://localhost:5090/api"

Write-Host "Probando registro de cliente..." -ForegroundColor Yellow

$body = @{
    email = "testuser$(Get-Date -Format 'HHmmss')@test.com"
    password = "Test123`$"
    confirmPassword = "Test123`$"
    nombre = "Test"
    apellido = "User"
    telefono = "3001234567"
    documento = "123456"
    direccion = "Calle 1"
    ciudad = "Bogota"
    pais = "Colombia"
} | ConvertTo-Json

Write-Host "Body:" -ForegroundColor Gray
Write-Host $body -ForegroundColor DarkGray

try {
    $response = Invoke-RestMethod -Uri "$baseUrl/Auth/register" -Method POST -Body $body -ContentType "application/json" -ErrorAction Stop
    Write-Host "SUCCESS!" -ForegroundColor Green
    Write-Host "Token: $($response.token.Substring(0, 30))..." -ForegroundColor Gray
    Write-Host "Email: $($response.email)" -ForegroundColor Gray
} catch {
    Write-Host "ERROR!" -ForegroundColor Red
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "Message: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.ErrorDetails.Message) {
        Write-Host "Details:" -ForegroundColor Red
        Write-Host $_.ErrorDetails.Message -ForegroundColor Red
    }
    
    if ($_.Exception.Response) {
        $result = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($result)
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        if ($responseBody) {
            Write-Host "Response Body:" -ForegroundColor Red
            Write-Host $responseBody -ForegroundColor Red
        }
    }
}

