# Script que simula el flujo completo de Swagger
$ErrorActionPreference = "Continue"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  PRUEBA COMPLETA: LOGIN + ENDPOINTS (SWAGGER)" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5090"

# Paso 1: Verificar que la API está corriendo
Write-Host "📡 1. Verificando que la API está corriendo..." -ForegroundColor Yellow
try {
    $health = Invoke-RestMethod -Uri "$baseUrl/health" -ErrorAction Stop
    Write-Host "   ✅ API está corriendo: $($health.status)" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "   ❌ ERROR: La API no está respondiendo" -ForegroundColor Red
    Write-Host "   Asegúrate de que la API esté corriendo en http://localhost:5090" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Presiona Enter para salir"
    exit
}

# Paso 2: Login
Write-Host "🔐 2. Haciendo LOGIN..." -ForegroundColor Yellow
Write-Host "   Email: admin@firmeza.com" -ForegroundColor Gray
Write-Host "   Password: Admin123$" -ForegroundColor Gray

$loginBody = @{
    email = "admin@firmeza.com"
    password = "Admin123$"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/Auth/login" -Method Post -Body $loginBody -ContentType "application/json" -ErrorAction Stop
    
    Write-Host "   ✅ LOGIN EXITOSO" -ForegroundColor Green
    Write-Host "   Rol: $($loginResponse.roles -join ', ')" -ForegroundColor Gray
    Write-Host "   Token obtenido (primeros 50 chars): $($loginResponse.token.Substring(0,50))..." -ForegroundColor DarkGray
    Write-Host ""
    
    $token = $loginResponse.token
} catch {
    Write-Host "   ❌ ERROR EN LOGIN" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host "   Mensaje: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Read-Host "Presiona Enter para salir"
    exit
}

# Configurar headers con el token
$headers = @{
    "Authorization" = "Bearer $token"
    "Accept" = "application/json"
}

# Paso 3: Probar GET /api/Clientes
Write-Host "👥 3. Probando GET /api/Clientes..." -ForegroundColor Yellow
try {
    $clientes = Invoke-RestMethod -Uri "$baseUrl/api/Clientes" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ EXITOSO - Status: 200 OK" -ForegroundColor Green
    Write-Host "   Total de clientes: $($clientes.Count)" -ForegroundColor Gray
    
    if ($clientes.Count -gt 0) {
        Write-Host "   Primer cliente: $($clientes[0].nombreCompleto)" -ForegroundColor Gray
    }
    Write-Host ""
} catch {
    Write-Host "   ❌ ERROR" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    Write-Host "   Mensaje: $($_.Exception.Message)" -ForegroundColor Red
    
    # Intentar leer el cuerpo de la respuesta
    if ($_.Exception.Response) {
        try {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $errorBody = $reader.ReadToEnd()
            if ($errorBody) {
                Write-Host "   Detalle: $errorBody" -ForegroundColor Red
            }
        } catch {}
    }
    Write-Host ""
}

# Paso 4: Probar GET /api/Ventas
Write-Host "💰 4. Probando GET /api/Ventas..." -ForegroundColor Yellow
try {
    $ventas = Invoke-RestMethod -Uri "$baseUrl/api/Ventas" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ EXITOSO - Status: 200 OK" -ForegroundColor Green
    Write-Host "   Total de ventas: $($ventas.Count)" -ForegroundColor Gray
    
    if ($ventas.Count -gt 0) {
        Write-Host "   Primera venta: ID=$($ventas[0].id), Total=$($ventas[0].total)" -ForegroundColor Gray
    }
    Write-Host ""
} catch {
    Write-Host "   ❌ ERROR" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    Write-Host "   Mensaje: $($_.Exception.Message)" -ForegroundColor Red
    
    # Intentar leer el cuerpo de la respuesta
    if ($_.Exception.Response) {
        try {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $errorBody = $reader.ReadToEnd()
            if ($errorBody) {
                Write-Host "   Detalle: $errorBody" -ForegroundColor Red
            }
        } catch {}
    }
    Write-Host ""
}

# Paso 5: Probar GET /api/Productos
Write-Host "📦 5. Probando GET /api/Productos..." -ForegroundColor Yellow
try {
    $productos = Invoke-RestMethod -Uri "$baseUrl/api/Productos" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ EXITOSO - Status: 200 OK" -ForegroundColor Green
    Write-Host "   Total de productos: $($productos.Count)" -ForegroundColor Gray
    Write-Host ""
} catch {
    Write-Host "   ❌ ERROR" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    Write-Host ""
}

# Paso 6: Probar GET /api/Categorias
Write-Host "🏷️  6. Probando GET /api/Categorias..." -ForegroundColor Yellow
try {
    $categorias = Invoke-RestMethod -Uri "$baseUrl/api/Categorias" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ EXITOSO - Status: 200 OK" -ForegroundColor Green
    Write-Host "   Total de categorías: $($categorias.Count)" -ForegroundColor Gray
    Write-Host ""
} catch {
    Write-Host "   ❌ ERROR" -ForegroundColor Red
    Write-Host "   Status: $($_.Exception.Response.StatusCode.value__) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    Write-Host ""
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "          PRUEBA COMPLETADA" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "================================================" -ForegroundColor Yellow
Write-Host "INSTRUCCIONES PARA SWAGGER:" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Yellow
Write-Host "1. Abre http://localhost:5090/swagger" -ForegroundColor White
Write-Host "2. POST /api/Auth/login con el JSON" -ForegroundColor White
Write-Host "3. Copia el TOKEN de la respuesta" -ForegroundColor White
Write-Host "4. Click en el boton Authorize" -ForegroundColor White
Write-Host "5. Pega: Bearer [tu-token]" -ForegroundColor White
Write-Host "6. Prueba los endpoints" -ForegroundColor White
Write-Host ""

Read-Host "Presiona Enter para salir"

