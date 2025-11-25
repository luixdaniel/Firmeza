# 🔧 ESTADO ACTUAL Y SOLUCIÓN FINAL

## ✅ CAMBIOS COMPLETADOS

1. ✅ **secrets.json configurado correctamente** con estructura JSON anidada
2. ✅ **RoleClaimType agregado** en Program.cs
3. ✅ **Migración de rol** "Administrador" → "Admin"
4. ✅ **PdfService corregido** para manejar WebRootPath null
5. ✅ **Script de inicio** creado: `start-api.bat`

## 📋 CREDENCIALES DE ACCESO

```
Email: admin@firmeza.com
Password: Admin123$
```

⚠️ **IMPORTANTE:** La contraseña termina con `$` (signo de dólar), NO con `!`

## 🚀 PASOS PARA PROBAR LA API

### 1. Asegúrate que la API está corriendo

Abre una ventana CMD y ejecuta:
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
start-api.bat
```

O ejecuta directamente:
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

### 2. Verifica que la API responde

Abre otra terminal y ejecuta:
```powershell
curl http://localhost:5090/health
```

Deberías ver:
```json
{"status":"Healthy","timestamp":"...","environment":"Development"}
```

### 3. Abre Swagger

Ve a: **http://localhost:5090/swagger**

### 4. Haz Login

#### Opción A: Desde Swagger
1. Expande `POST /api/Auth/login`
2. Click en "Try it out"
3. Pega este JSON:
   ```json
   {
     "email": "admin@firmeza.com",
     "password": "Admin123$"
   }
   ```
4. Click en "Execute"

#### Opción B: Desde PowerShell
```powershell
$body = '{"email":"admin@firmeza.com","password":"Admin123$"}'
$response = Invoke-RestMethod -Uri "http://localhost:5090/api/Auth/login" -Method Post -Body $body -ContentType "application/json"
$response
```

### 5. Copia el Token

De la respuesta, copia el valor de `"token"`.

### 6. Autorízate en Swagger

1. Click en el botón 🔒 **"Authorize"** (arriba a la derecha)
2. Pega: `Bearer [tu-token-completo]`
3. Click en "Authorize"
4. Cierra el modal

### 7. Prueba los Endpoints

Ahora puedes probar:
- ✅ `GET /api/Clientes` - Debería funcionar
- ✅ `GET /api/Ventas` - Debería funcionar
- ✅ `GET /api/Productos` - Debería funcionar
- ✅ `GET /api/Categorias` - Debería funcionar

---

## ❌ SOLUCIÓN DE PROBLEMAS

### Si el Login da 401

**Posibles causas:**

1. **El usuario no se creó correctamente**
   - Revisa los logs de la API al iniciar
   - Deberías ver: `✅ Usuario administrador creado: admin@firmeza.com / Admin123!`

2. **La contraseña es incorrecta**
   - Usa exactamente: `Admin123$` (con mayúscula A y signo de dólar `$`)

3. **La base de datos no está accesible**
   - Verifica que PostgreSQL está corriendo
   - Verifica la conexión en `secrets.json`

### Si los Endpoints dan 401 después del Login

1. **Verifica que el token tiene el rol correcto**
   - Decodifica tu token en https://jwt.io
   - Busca: `"role": "Admin"` (no "Administrador")

2. **Asegúrate de usar "Bearer " antes del token**
   - Correcto: `Bearer eyJhbGciOi...`
   - Incorrecto: `eyJhbGciOi...`

3. **El token puede haber expirado**
   - Vuelve a hacer login y obtén un nuevo token

### Si los Endpoints dan 403 Forbidden

- El usuario está autenticado pero no tiene permisos
- Verifica que el token incluye el rol "Admin"

### Si la API no inicia

1. **Puerto 5090 ocupado**
   ```powershell
   Get-Process -Name dotnet | Stop-Process -Force
   ```

2. **Error de compilación**
   ```cmd
   cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
   dotnet clean
   dotnet build
   ```

3. **Error de base de datos**
   - Verifica el `secrets.json`
   - Verifica que PostgreSQL está corriendo

---

## 🎯 PRUEBA RÁPIDA CON CURL

```bash
# 1. Login
curl -X POST "http://localhost:5090/api/Auth/login" -H "Content-Type: application/json" -d "{\"email\":\"admin@firmeza.com\",\"password\":\"Admin123$\"}"

# 2. Copiar el token de la respuesta y usarlo:
curl -X GET "http://localhost:5090/api/Clientes" -H "Authorization: Bearer TU_TOKEN_AQUI"
```

---

## 📝 ARCHIVOS IMPORTANTES

- **Configuración JWT**: `c:\Users\luisc\AppData\Roaming\Microsoft\UserSecrets\4c7ae222-4756-4709-b673-f9b14d7db826\secrets.json`
- **Script inicio**: `C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web\start-api.bat`
- **Swagger**: http://localhost:5090/swagger
- **Health Check**: http://localhost:5090/health

---

## ✅ VERIFICACIÓN FINAL

Para confirmar que todo funciona:

1. ✅ API inicia sin errores
2. ✅ Health endpoint responde
3. ✅ Login devuelve un token con rol "Admin"
4. ✅ Endpoints de Clientes/Ventas/Productos responden 200 OK

**¡La API está lista para usar!** 🚀

