# 📧 Envío de Comprobantes por Email - Implementación Completa

## ✅ Funcionalidades Implementadas

### 1. **Servicio de Email (EmailService.cs)**
- ✅ Integración con SMTP de Gmail usando MailKit
- ✅ Envío de emails HTML con diseño profesional
- ✅ Adjunto de PDF del comprobante
- ✅ Logging detallado de todas las operaciones
- ✅ Manejo de errores robusto

### 2. **Servicio de Generación de Comprobantes (ComprobanteService.cs)**
- ✅ Generación de PDF profesional con iTextSharp
- ✅ Incluye logo, datos del cliente, detalles de productos
- ✅ Cálculo de subtotal, IVA y total
- ✅ Diseño limpio y profesional
- ✅ Formato A4 estándar

### 3. **Integración en el Flujo de Compra**
- ✅ Envío automático después de completar la compra
- ✅ Ejecución asíncrona (no bloquea la respuesta)
- ✅ Mensaje de confirmación al cliente
- ✅ Logging completo del proceso

### 4. **Frontend Actualizado**
- ✅ Mensaje de confirmación mejorado
- ✅ Información sobre envío del comprobante
- ✅ Integración transparente con el flujo existente

## 🔧 Configuración Requerida

### Paso 1: Configurar Gmail para usar App Password

1. Ve a tu cuenta de Google: https://myaccount.google.com/
2. Seguridad → Verificación en 2 pasos (debe estar activada)
3. Contraseñas de aplicaciones
4. Genera una contraseña para "Mail"
5. Copia la contraseña generada (16 caracteres)

### Paso 2: Actualizar appsettings.json o User Secrets

#### Opción A: appsettings.json (Solo para desarrollo local)

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SenderEmail": "tu-email@gmail.com",
    "SenderPassword": "tu-app-password-de-16-caracteres",
    "SenderName": "Firmeza - Tienda Online"
  }
}
```

#### Opción B: User Secrets (Recomendado)

```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web

dotnet user-secrets set "EmailSettings:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:SenderEmail" "tu-email@gmail.com"
dotnet user-secrets set "EmailSettings:SenderPassword" "tu-app-password"
dotnet user-secrets set "EmailSettings:SenderName" "Firmeza - Tienda Online"
```

## 🚀 Cómo Usar

### 1. Configura tus credenciales de Gmail

Edita el archivo `appsettings.json`:

```bash
nano /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web/appsettings.json
```

Reemplaza:
- `tu-email@gmail.com` con tu email real
- `tu-app-password` con la contraseña de aplicación de Gmail

### 2. Reinicia la API

```bash
# Detener la API actual
pkill -f ApiFirmeza

# Iniciar la API
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

### 3. Prueba desde el Frontend

1. Inicia el frontend:
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

2. Realiza una compra:
   - Login como cliente
   - Agrega productos al carrito
   - Completa la compra
   - Verás el mensaje: "Compra realizada exitosamente. El comprobante será enviado a tu correo electrónico."

3. Revisa tu email:
   - Deberías recibir un email con el comprobante en PDF adjunto
   - El email tiene un diseño profesional con todos los detalles

### 4. Verifica los Logs

En la consola de la API verás:
```
📧 Enviando comprobante de compra a cliente@email.com
📎 PDF adjunto: 12345 bytes
✅ Correo enviado exitosamente a cliente@email.com
```

## 📋 Estructura del Email

El email incluye:

### Encabezado
- Título: "¡Gracias por tu compra!"
- Subtítulo con confirmación

### Cuerpo
- Saludo personalizado con nombre del cliente
- Detalles de la compra:
  - Número de factura
  - ID de venta
  - Fecha y hora
  - Total pagado (destacado)
- Información sobre el PDF adjunto

### PDF Adjunto
- Logo y nombre de la empresa
- Número de factura y fecha
- Información del cliente
- Tabla detallada de productos
- Subtotal, IVA y Total
- Pie de página con información legal

## 🔍 Solución de Problemas

### Error: "Configuración de email incompleta"

**Solución**: Verifica que hayas configurado correctamente las credenciales en `appsettings.json`

### Error: "Authentication failed"

**Causa**: Credenciales incorrectas o no estás usando App Password

**Solución**:
1. Verifica que la verificación en 2 pasos esté activada en Gmail
2. Genera una nueva App Password
3. Usa esa contraseña de 16 caracteres (sin espacios)

### Email no llega

**Soluciones**:
1. Revisa la carpeta de Spam
2. Verifica que el email del cliente en la BD sea válido
3. Revisa los logs de la API para ver si hay errores
4. Verifica que Gmail no haya bloqueado el envío (revisa tu bandeja de seguridad de Gmail)

### El PDF está vacío o con errores

**Causa**: La venta no tiene detalles cargados

**Solución**: Asegúrate de que el método `GetByIdAsync` cargue los detalles con `.Include()`

## 🎨 Personalización

### Cambiar el diseño del email

Edita `/home/Coder/Escritorio/Firmeza/ApiFirmeza.Web/Services/EmailService.cs`, línea ~60:

```csharp
HtmlBody = $@"
    // Tu HTML personalizado aquí
"
```

### Cambiar el diseño del PDF

Edita `/home/Coder/Escritorio/Firmeza/ApiFirmeza.Web/Services/ComprobanteService.cs`

Puedes modificar:
- Fuentes (líneas 20-24)
- Colores
- Estructura de las tablas
- Contenido del encabezado y pie de página

### Usar otro proveedor de email (no Gmail)

En `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.tu-proveedor.com",
    "SmtpPort": "587",
    "SenderEmail": "tu-email@tu-dominio.com",
    "SenderPassword": "tu-contraseña",
    "SenderName": "Tu Nombre"
  }
}
```

Proveedores comunes:
- **Outlook/Hotmail**: `smtp.office365.com`, puerto `587`
- **SendGrid**: `smtp.sendgrid.net`, puerto `587`
- **Mailgun**: `smtp.mailgun.org`, puerto `587`

## 📊 Flujo Completo

```
1. Cliente completa la compra en el frontend
   ↓
2. POST /api/Ventas crea la venta en la BD
   ↓
3. VentaService guarda la venta y detalles
   ↓
4. Task asíncrono inicia:
   a. Obtiene venta completa con detalles
   b. ComprobanteService genera PDF
   c. EmailService envía email con PDF
   ↓
5. Frontend muestra: "Comprobante enviado a tu email"
   ↓
6. Cliente recibe email con PDF adjunto
```

## ✅ Checklist de Verificación

- [ ] MailKit y MimeKit instalados
- [ ] Servicios registrados en Program.cs
- [ ] EmailSettings configurado en appsettings.json
- [ ] App Password de Gmail generada
- [ ] API reiniciada después de configurar
- [ ] Frontend actualizado con nuevo mensaje
- [ ] Prueba realizada con compra real
- [ ] Email recibido correctamente
- [ ] PDF se puede abrir y leer

## 🎯 Próximos Pasos (Opcional)

1. **Plantillas de email personalizables**
   - Mover el HTML a archivos de plantilla
   - Permitir personalización sin recompilar

2. **Cola de envío de emails**
   - Implementar RabbitMQ o Azure Queue
   - Reintentos automáticos en caso de fallo

3. **Tracking de emails**
   - Registrar si el email fue abierto
   - Notificar al admin de fallos de envío

4. **Múltiples idiomas**
   - Detectar idioma del cliente
   - Enviar email en su idioma preferido

5. **Notificaciones adicionales**
   - Email de confirmación de registro
   - Email de recuperación de contraseña
   - Email de cambio de estado de pedido

---

💡 **Nota**: El envío de emails se realiza de forma asíncrona para no bloquear la respuesta al cliente. Si hay un error al enviar el email, no afectará la creación de la venta.

