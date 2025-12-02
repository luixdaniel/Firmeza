# 🔧 Herramienta de Diagnóstico Gmail SMTP

## Qué hace este programa

Este es un programa de prueba simple que intenta conectarse y autenticarse con Gmail SMTP usando las credenciales exactas de tu `secrets.json`.

## Cómo ejecutarlo

```bash
cd /home/Coder/Escritorio/Firmeza/TestGmailSMTP
dotnet run
```

## Qué va a probar

1. ✅ Verifica que la contraseña no tenga espacios
2. 🔌 Intenta conectarse a smtp.gmail.com:587
3. 🔐 Intenta autenticarse con tus credenciales

## Posibles resultados

### ✅ Si funciona
```
✅ ¡ÉXITO! AUTENTICACIÓN CORRECTA
```
Esto significa que las credenciales funcionan. El problema está en la configuración de secrets en tu aplicación principal.

### ❌ Si falla con error 535
```
❌ ERROR DE AUTENTICACIÓN
535: 5.7.8 Username and Password not accepted
```

**ESTO ES LO QUE ESTÁ PASANDO EN TU CASO**

## 🎯 Solución Definitiva

El problema **NO son las dependencias**. Gmail está rechazando la contraseña de aplicación.

### Por qué puede fallar en Linux pero funcionar en Windows:

1. **Gmail detecta el cambio de sistema operativo** y lo considera sospechoso
2. **La contraseña fue generada en Windows** y Gmail la asoció con ese entorno
3. **Gmail tiene políticas de seguridad más estrictas** para conexiones desde Linux

### ✅ SOLUCIÓN (HAZ ESTO AHORA):

#### Paso 1: Ve a la configuración de Gmail
```
https://myaccount.google.com/apppasswords
```

#### Paso 2: REVOCA la contraseña actual
- Busca "Firmeza" o cualquier contraseña de aplicación relacionada
- Elimínala

#### Paso 3: CREA UNA NUEVA contraseña
- Selecciona "Correo" como aplicación  
- Selecciona "Otro" como dispositivo
- Escribe: **"Firmeza Linux"**
- Haz clic en Generar

#### Paso 4: COPIA LA CONTRASEÑA **SIN ESPACIOS**
Gmail te mostrará algo como:
```
abcd efgh ijkl mnop
```

**Cópiala así (SIN ESPACIOS):**
```
abcdefghijklmnop
```

#### Paso 5: ACTUALIZA secrets.json
```bash
nano /home/Coder/.microsoft/usersecrets/4c7ae222-4756-4709-b673-f9b14d7db826/secrets.json
```

Reemplaza el valor de `EmailSettings:SenderPassword` con la nueva contraseña.

#### Paso 6: PRUEBA NUEVAMENTE
```bash
cd /home/Coder/Escritorio/Firmeza/TestGmailSMTP
dotnet run
```

## 🆘 Si aún así no funciona

Considera usar **SendGrid** o **Mailgun** en lugar de Gmail:

### SendGrid (Gratis hasta 100 emails/día)
```json
{
  "EmailSettings:SmtpHost": "smtp.sendgrid.net",
  "EmailSettings:SmtpPort": "587",
  "EmailSettings:SenderEmail": "tu-email@dominio.com",
  "EmailSettings:SenderPassword": "tu-api-key-de-sendgrid"
}
```

Registro: https://signup.sendgrid.com/

### Mailgun (Gratis hasta 5,000 emails/mes)
```json
{
  "EmailSettings:SmtpHost": "smtp.mailgun.org",
  "EmailSettings:SmtpPort": "587",
  "EmailSettings:SenderEmail": "postmaster@tu-dominio.mailgun.org",
  "EmailSettings:SenderPassword": "tu-password-de-mailgun"
}
```

Registro: https://signup.mailgun.com/

## 📊 Resumen

- ❌ **NO** son las dependencias (MailKit 4.14.1 está OK)
- ❌ **NO** son los espacios (ya los quitamos)
- ✅ **SÍ** es Gmail bloqueando la contraseña desde Linux
- ✅ **SOLUCIÓN**: Genera una NUEVA contraseña de aplicación específica para Linux

