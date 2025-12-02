# 🔐 Configuración de Secrets para ApiFirmeza.Web

## ⚠️ IMPORTANTE: NO subir credenciales reales a GitHub

Este archivo explica cómo configurar los secretos necesarios para ejecutar la aplicación.

## 📋 Configuración Local (Desarrollo)

### 1. Configurar User Secrets

Los secretos sensibles se guardan en `secrets.json` usando .NET User Secrets.

```bash
cd ApiFirmeza.Web

# Configurar email (Gmail o SendGrid)
dotnet user-secrets set "EmailSettings:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:SenderEmail" "tu-email@gmail.com"
dotnet user-secrets set "EmailSettings:SenderPassword" "tu_password_sin_espacios"
dotnet user-secrets set "EmailSettings:SenderName" "Firmeza - Tienda Online"

# Configurar base de datos
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=firmeza;Username=postgres;Password=tu_password"
```

### 2. Verificar Secrets Configurados

```bash
cd ApiFirmeza.Web
dotnet user-secrets list
```

## 📧 Configuración de Email

### Opción 1: Gmail (Recomendado para desarrollo)

1. **Activa la verificación en 2 pasos:**
   - Ve a: https://myaccount.google.com/signinoptions/two-step-verification

2. **Genera una Contraseña de Aplicación:**
   - Ve a: https://myaccount.google.com/apppasswords
   - Aplicación: **Correo**
   - Dispositivo: **Otro** → "Firmeza Linux" o "Firmeza Windows"
   - Copia la contraseña **SIN ESPACIOS**: `abcdefghijklmnop`

3. **Configura los secrets:**
   ```bash
   dotnet user-secrets set "EmailSettings:SmtpHost" "smtp.gmail.com"
   dotnet user-secrets set "EmailSettings:SmtpPort" "587"
   dotnet user-secrets set "EmailSettings:SenderEmail" "tu-email@gmail.com"
   dotnet user-secrets set "EmailSettings:SenderPassword" "tu_password_sin_espacios"
   ```

### Opción 2: SendGrid (Recomendado para producción)

1. **Regístrate en SendGrid:**
   - https://signup.sendgrid.com/

2. **Crea una API Key:**
   - Ve a Settings → API Keys
   - Create API Key con "Full Access"

3. **Configura los secrets:**
   ```bash
   dotnet user-secrets set "EmailSettings:SmtpHost" "smtp.sendgrid.net"
   dotnet user-secrets set "EmailSettings:SmtpPort" "587"
   dotnet user-secrets set "EmailSettings:SenderEmail" "tu-email@gmail.com"
   dotnet user-secrets set "EmailSettings:SenderPassword" "tu_api_key_de_sendgrid"
   ```

## 🗄️ Configuración de Base de Datos

### Desarrollo Local (PostgreSQL)

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=firmeza;Username=postgres;Password=tu_password"
```

### Supabase (Producción/Desarrollo en la nube)

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=tu-proyecto.supabase.co;Port=5432;Database=postgres;Username=postgres.xxx;Password=tu_password;SSL Mode=Require;Trust Server Certificate=true"
```

## 🔑 JWT Configuration

La clave secreta de JWT está en `appsettings.json` para desarrollo, pero en producción debe estar en variables de entorno:

```bash
# Para producción, cambiar y configurar en el servidor
dotnet user-secrets set "JwtSettings:SecretKey" "TU_CLAVE_SUPER_SECRETA_DE_AL_MENOS_32_CARACTERES"
```

## 🚀 Producción (Docker/Servidor)

En producción, usa **variables de entorno** en lugar de User Secrets:

### Docker Compose

```yaml
environment:
  - EmailSettings__SmtpHost=smtp.sendgrid.net
  - EmailSettings__SmtpPort=587
  - EmailSettings__SenderEmail=noreply@tudominio.com
  - EmailSettings__SenderPassword=${SENDGRID_API_KEY}
  - ConnectionStrings__DefaultConnection=${DATABASE_URL}
  - JwtSettings__SecretKey=${JWT_SECRET}
```

### Variables de Entorno en Linux

```bash
export EmailSettings__SmtpHost="smtp.sendgrid.net"
export EmailSettings__SmtpPort="587"
export EmailSettings__SenderEmail="noreply@tudominio.com"
export EmailSettings__SenderPassword="tu_api_key"
export ConnectionStrings__DefaultConnection="tu_connection_string"
```

## ✅ Verificar Configuración

```bash
# Ejecutar la aplicación
cd ApiFirmeza.Web
dotnet run

# Probar endpoint de email
curl http://localhost:5000/api/testemail/test-credentials
```

## 📝 Estructura de secrets.json (Referencia)

**Ubicación**: `~/.microsoft/usersecrets/4c7ae222-4756-4709-b673-f9b14d7db826/secrets.json`

```json
{
  "EmailSettings:SmtpPort": "587",
  "EmailSettings:SmtpHost": "smtp.gmail.com",
  "EmailSettings:SenderPassword": "tu_password_sin_espacios",
  "EmailSettings:SenderName": "Firmeza - Tienda Online",
  "EmailSettings:SenderEmail": "tu-email@gmail.com",
  "ConnectionStrings:DefaultConnection": "tu_connection_string"
}
```

## 🆘 Solución de Problemas

### Email no envía - Error 535

**Problema**: Gmail rechaza la autenticación

**Solución**:
1. Verifica que la contraseña NO tenga espacios
2. Genera una NUEVA contraseña de aplicación
3. Considera usar SendGrid en su lugar

### Error de conexión a base de datos

**Problema**: No puede conectarse a PostgreSQL

**Solución**:
1. Verifica que PostgreSQL esté corriendo
2. Verifica el connection string
3. Para Supabase, asegúrate de incluir `SSL Mode=Require`

## 📚 Más Información

- [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Gmail App Passwords](https://support.google.com/accounts/answer/185833)
- [SendGrid Documentation](https://docs.sendgrid.com/)

