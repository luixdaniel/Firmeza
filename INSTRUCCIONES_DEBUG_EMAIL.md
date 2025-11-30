# 🔍 INSTRUCCIONES PARA DEBUGGEAR EL PROBLEMA DEL EMAIL

## 📋 SITUACIÓN ACTUAL

- ✅ El test directo funciona (el email llega)
- ❌ Desde el frontend NO llega el email

## 🎯 LO QUE NECESITAMOS VERIFICAR

### PASO 1: Hacer una Compra desde el Frontend

1. Abre el frontend (Next.js)
2. Inicia sesión con: **muyguapoluisguapo@gmail.com**
3. Realiza una compra de cualquier producto
4. **NO CIERRES** la consola donde está corriendo la API

### PASO 2: Observar los Logs de la API

Cuando hagas la compra, busca en la consola de la API los siguientes mensajes:

#### ✅ Lo que DEBE aparecer:

```
🛒 Creando venta - Método de pago: [MÉTODO], Detalles: [CANTIDAD]
✅ Create Venta - Cliente autenticado: ID=[ID], Nombre=[NOMBRE]
📦 Create Venta - Venta mapeada: ClienteId=[ID], Cliente=[NOMBRE]...
✅ Create Venta - Venta creada exitosamente: VentaId=[ID]...
📧 Preparando envío de email a: [EMAIL], Cliente: [NOMBRE]
📧 [BACKGROUND] Iniciando envío de comprobante por email para Venta ID: [ID]
📄 [BACKGROUND] Generando PDF del comprobante para Venta ID: [ID]
📤 [BACKGROUND] Enviando email a: [EMAIL]
🔧 Configuración SMTP: Host=smtp.gmail.com, Port=587, From=ceraluis4@gmail.com
🔌 Conectando al servidor SMTP smtp.gmail.com:587...
✅ Conectado al servidor SMTP
🔐 Autenticando con ceraluis4@gmail.com...
✅ Autenticación exitosa
📤 Enviando mensaje...
✅ Mensaje enviado
✅ [BACKGROUND] Comprobante enviado exitosamente a [EMAIL]
```

#### ❌ Lo que puede FALLAR:

1. **No aparece "Preparando envío de email"**
   - Significa que el cliente.Email está vacío o es null

2. **No aparece los logs de [BACKGROUND]**
   - El Task.Run no se está ejecutando

3. **Aparece "Error al enviar comprobante por email"**
   - Hay un error en el proceso de envío

4. **No aparece "Configuración SMTP"**
   - El EmailService no se está ejecutando

### PASO 3: Verificar el Email del Cliente en la Base de Datos

Ejecuta este comando en PowerShell para verificar que el cliente tiene un email:

```powershell
$apiUrl = "http://localhost:5090"

# Login
$loginBody = @{
    email = "muyguapoluisguapo@gmail.com"
    password = "Luis1206$"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
$token = $loginResponse.token

# Obtener información del cliente
$headers = @{
    "Authorization" = "Bearer $token"
}

$clientes = Invoke-RestMethod -Uri "$apiUrl/api/clientes" -Method GET -Headers $headers
$miCliente = $clientes | Where-Object { $_.email -eq "muyguapoluisguapo@gmail.com" }

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "INFORMACIÓN DEL CLIENTE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "ID: $($miCliente.id)" -ForegroundColor White
Write-Host "Nombre: $($miCliente.nombreCompleto)" -ForegroundColor White
Write-Host "Email: $($miCliente.email)" -ForegroundColor Yellow
Write-Host "Activo: $($miCliente.activo)" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan
```

### PASO 4: Copiar y Compartir los Logs

1. Haz la compra desde el frontend
2. Copia TODOS los logs que aparezcan en la consola de la API
3. Busca específicamente estas líneas:
   - "📧 Preparando envío de email a:"
   - "📧 [BACKGROUND] Iniciando envío..."
   - Cualquier línea que diga "❌" o "Error"

---

## 🔍 POSIBLES CAUSAS

### Causa 1: El cliente no tiene Email en la BD
**Síntoma:** No aparece el log "📧 Preparando envío de email"

**Solución:** Verificar que el registro del usuario creó correctamente el cliente con email

### Causa 2: Task.Run no se ejecuta
**Síntoma:** Aparece "Preparando envío" pero NO aparecen logs de [BACKGROUND]

**Solución:** Problema con el contexto asíncrono

### Causa 3: Error en el envío
**Síntoma:** Aparecen logs de [BACKGROUND] pero termina con error

**Solución:** Ver el mensaje de error específico en los logs

---

## 📝 SCRIPT RÁPIDO PARA VERIFICAR

Guarda esto como `verificar-cliente-email.ps1`:

```powershell
$apiUrl = "http://localhost:5090"

$loginBody = @{
    email = "muyguapoluisguapo@gmail.com"
    password = "Luis1206$"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token

    $headers = @{
        "Authorization" = "Bearer $token"
    }

    $clientes = Invoke-RestMethod -Uri "$apiUrl/api/clientes" -Method GET -Headers $headers
    $miCliente = $clientes | Where-Object { $_.email -eq "muyguapoluisguapo@gmail.com" }

    if ($miCliente) {
        Write-Host "[OK] Cliente encontrado" -ForegroundColor Green
        Write-Host "Email: $($miCliente.email)" -ForegroundColor Yellow
        
        if ([string]::IsNullOrEmpty($miCliente.email)) {
            Write-Host "[ERROR] El cliente NO TIENE EMAIL!" -ForegroundColor Red
        } else {
            Write-Host "[OK] El cliente tiene email configurado" -ForegroundColor Green
        }
    } else {
        Write-Host "[ERROR] Cliente no encontrado" -ForegroundColor Red
    }
} catch {
    Write-Host "[ERROR] $($_.Exception.Message)" -ForegroundColor Red
}
```

---

## ⚡ ACCIÓN INMEDIATA

**POR FAVOR:**

1. Haz una compra desde el frontend
2. Observa la consola de la API
3. Copia los logs que aparezcan
4. Compártelos conmigo

Esto me ayudará a identificar exactamente dónde está fallando el proceso.

