# 🔧 CONFIGURAR SECRETS.JSON PARA JWT

## Problema
El error `"invalid_token"` indica que la API no puede validar el token JWT porque **no encuentra la configuración JWT completa**.

## Solución

Necesitas agregar la configuración JWT a tu archivo `secrets.json`. 

### Paso 1: Inicializar secrets.json (si no existe)

Desde el directorio `ApiFirmeza.Web`, ejecuta:

```bash
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet user-secrets init
```

### Paso 2: Agregar la configuración JWT

Ejecuta estos comandos UNO POR UNO:

```bash
dotnet user-secrets set "JwtSettings:SecretKey" "MiClaveSecretaSuperSeguraParaJWT2024FirmezaAPI!@#$%"
dotnet user-secrets set "JwtSettings:Issuer" "FirmezaAPI"
dotnet user-secrets set "JwtSettings:Audience" "FirmezaClients"
dotnet user-secrets set "JwtSettings:ExpirationMinutes" "120"
```

### Paso 3: Agregar la conexión de base de datos

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=firmeza_db;Username=postgres;Password=TU_PASSWORD_AQUI"
```

**IMPORTANTE:** Reemplaza `TU_PASSWORD_AQUI` con tu contraseña real de PostgreSQL.

### Paso 4: Verificar secrets.json

Para ver el contenido de tu `secrets.json`:

```bash
dotnet user-secrets list
```

Deberías ver algo como:

```
ConnectionStrings:DefaultConnection = Host=localhost;Database=firmeza_db;Username=postgres;Password=...
JwtSettings:SecretKey = MiClaveSecretaSuperSeguraParaJWT2024FirmezaAPI!@#$%
JwtSettings:Issuer = FirmezaAPI
JwtSettings:Audience = FirmezaClients
JwtSettings:ExpirationMinutes = 120
```

### Paso 5: Reiniciar la API

1. Detén la API (Ctrl+C)
2. Inicia nuevamente:
   ```bash
   dotnet run
   ```

### Paso 6: Probar

1. Vuelve a hacer **login** en Swagger
2. Copia el **nuevo token**
3. Autorízate en Swagger
4. Prueba los endpoints

## ✅ Verificación

Si todo está bien configurado, verás en la consola de la API:

```
✅ Rol 'Admin' creado (si no existía)
✅ Usuario administrador creado: admin@firmeza.com / Admin123!
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5090
```

Y los endpoints deberían responder **200 OK** en lugar de **401 Unauthorized**.

---

## 📝 Nota sobre la estructura de secrets.json

El archivo `secrets.json` se encuentra en:
```
C:\Users\luisc\AppData\Roaming\Microsoft\UserSecrets\<user-secrets-id>\secrets.json
```

Y debería tener esta estructura:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=firmeza_db;Username=postgres;Password=tu_password"
  },
  "JwtSettings": {
    "SecretKey": "MiClaveSecretaSuperSeguraParaJWT2024FirmezaAPI!@#$%",
    "Issuer": "FirmezaAPI",
    "Audience": "FirmezaClients",
    "ExpirationMinutes": "120"
  }
}
```

**NO compartas este archivo ni lo subas a Git - contiene información sensible.**

