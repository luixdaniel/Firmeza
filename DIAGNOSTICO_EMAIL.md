# DIAGNÓSTICO Y SOLUCIÓN: Correos Electrónicos No Llegan

## 📋 Resumen del Problema
El sistema muestra la alerta de que se envía un correo, pero el correo no llega al destinatario.

## ✅ Cambios Realizados

### 1. Configuración de Email Actualizada
Se ha agregado la configuración de email en `ApiFirmeza.Web\appsettings.Development.json`:

```json
"EmailSettings": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderEmail": "ceraluis4@gmail.com",
  "SenderPassword": "thmp svtw ntvm yceu",
  "SenderName": "Firmeza - Tienda"
}
```

### 2. Logging Mejorado en EmailService
Se ha mejorado el servicio de email (`ApiFirmeza.Web\Services\EmailService.cs`) para:
- ✅ Mostrar más información de diagnóstico en los logs
- ✅ Capturar errores de autenticación SMTP específicamente
- ✅ Mostrar cada paso del proceso de envío (conexión, autenticación, envío)

### 3. Endpoint de Prueba Creado
Se ha creado un nuevo controlador `TestEmailController.cs` que permite probar el envío de correos de forma directa.

## 🚀 PASOS PARA RESOLVER EL PROBLEMA

### Paso 1: Reiniciar la API
**IMPORTANTE:** Debes reiniciar la API para que cargue la nueva configuración de email.

1. Si tienes la API corriendo, **deténla** (Ctrl+C en la consola)
2. Inicia la API nuevamente con:
   ```cmd
   cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
   dotnet run
   ```

### Paso 2: Probar el Envío de Email Directamente

Opción A - Usando el script de prueba:
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza
test-email-directo.bat
```

Opción B - Usando PowerShell:
```powershell
Invoke-RestMethod -Uri "http://localhost:5090/api/testemail/send-test" `
  -Method POST `
  -Body '{"email":"muyguapoluisguapo@gmail.com"}' `
  -ContentType "application/json"
```

Opción C - Usando curl:
```cmd
curl -X POST "http://localhost:5090/api/testemail/send-test" ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"muyguapoluisguapo@gmail.com\"}"
```

### Paso 3: Revisar los Logs

Cuando hagas la prueba, observa los logs de la API. Deberías ver:

✅ **Si todo está bien:**
```
📧 Iniciando envío de comprobante de compra a muyguapoluisguapo@gmail.com
🔧 Configuración SMTP: Host=smtp.gmail.com, Port=587, From=ceraluis4@gmail.com
🔌 Conectando al servidor SMTP smtp.gmail.com:587...
✅ Conectado al servidor SMTP
🔐 Autenticando con ceraluis4@gmail.com...
✅ Autenticación exitosa
📤 Enviando mensaje...
✅ Mensaje enviado
✅ Correo enviado exitosamente
```

❌ **Si hay problemas:**
```
❌ Error de autenticación SMTP
❌ Configuración de email incompleta
```

## 🔍 POSIBLES CAUSAS Y SOLUCIONES

### Causa 1: Contraseña de Aplicación Incorrecta
**Síntoma:** Error de autenticación SMTP

**Solución:** 
1. Ve a tu cuenta de Google: https://myaccount.google.com/
2. Seguridad → Verificación en 2 pasos → Contraseñas de aplicaciones
3. Genera una nueva contraseña de aplicación para "Correo"
4. Actualiza `appsettings.Development.json` con la nueva contraseña
5. Reinicia la API

### Causa 2: API No Reiniciada
**Síntoma:** El correo sigue sin llegar después de actualizar la configuración

**Solución:** 
Debes reiniciar la API para que cargue la nueva configuración de `appsettings.json`

### Causa 3: Firewall o Antivirus
**Síntoma:** Timeout al conectar con el servidor SMTP

**Solución:**
- Verifica que tu firewall permita conexiones salientes al puerto 587
- Temporalmente desactiva el antivirus para probar

### Causa 4: Gmail Bloqueó el Acceso
**Síntoma:** Error de autenticación incluso con contraseña correcta

**Solución:**
- Revisa tu email para ver si Gmail envió una alerta de seguridad
- Ve a: https://myaccount.google.com/security
- Verifica que "Acceso de aplicaciones menos seguras" esté configurado correctamente
- Mejor aún: Usa una contraseña de aplicación

## 📧 VERIFICAR CORREO RECIBIDO

Cuando realices una compra, deberías recibir un email con:
- ✅ Asunto: "Comprobante de Compra - Factura [NÚMERO]"
- ✅ Remitente: Firmeza - Tienda (ceraluis4@gmail.com)
- ✅ Cuerpo HTML con detalles de la compra
- ✅ Archivo PDF adjunto con el comprobante

**No olvides revisar la carpeta de SPAM**

## 🧪 SCRIPTS DE PRUEBA DISPONIBLES

1. **test-email-directo.bat** - Prueba rápida de envío de email
2. **test-email.ps1** - Prueba completa con login y creación de venta

## 📝 NOTAS IMPORTANTES

1. **Contraseña de Aplicación vs Contraseña Normal:**
   - ✅ USAR: Contraseña de aplicación de Gmail
   - ❌ NO USAR: Tu contraseña normal de Gmail
   
2. **La configuración actual usa:**
   - Correo: ceraluis4@gmail.com
   - Contraseña: thmp svtw ntvm yceu (contraseña de aplicación)
   
3. **El email se envía de forma asíncrona:**
   - La venta se crea inmediatamente
   - El correo se envía en segundo plano
   - Si falla el envío, la venta ya está creada

## 📞 SIGUIENTE PASO

**Por favor, realiza estos pasos:**

1. ✅ Reinicia la API si está corriendo
2. ✅ Ejecuta el script: `test-email-directo.bat`
3. ✅ Observa los logs de la API
4. ✅ Revisa tu correo (incluyendo spam)
5. ✅ Comparte los logs si hay algún error

---

**Fecha de Actualización:** 2025-01-29
**Archivos Modificados:**
- ApiFirmeza.Web/appsettings.Development.json
- ApiFirmeza.Web/Services/EmailService.cs
- ApiFirmeza.Web/Controllers/TestEmailController.cs (NUEVO)

