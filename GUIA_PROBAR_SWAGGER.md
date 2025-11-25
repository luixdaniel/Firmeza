# 🎯 GUÍA COMPLETA: PROBAR AUTENTICACIÓN EN SWAGGER

## ✅ Estado Actual

- ✅ **secrets.json corregido** con la estructura JSON correcta
- ✅ **Configuración JWT completa** (SecretKey, Issuer, Audience, ExpirationMinutes)
- ✅ **RoleClaimType configurado** en Program.cs
- ✅ **Migración de roles** de "Administrador" a "Admin"
- ✅ **API compilada y corriendo** en http://localhost:5090

## 📋 PASOS PARA PROBAR

### 1️⃣ Abre Swagger

Ve a: **http://localhost:5090/swagger**

### 2️⃣ Haz Login

1. **Expande** el endpoint `POST /api/Auth/login`
2. **Haz clic** en "Try it out"
3. **Pega** este JSON en el body:
   ```json
   {
     "email": "admin@firmeza.com",
     "password": "Admin123!"
   }
   ```
4. **Haz clic** en "Execute"

**Deberías ver una respuesta 200 OK con:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-24T20:41:51Z",
  "email": "admin@firmeza.com",
  "nombreCompleto": "Admin Sistema",
  "roles": ["Admin"]
}
```

✅ **Verifica que `"roles": ["Admin"]`** (NO "Administrador")

### 3️⃣ Autorízate en Swagger

1. **Copia el token** completo (todo el string largo)
2. **Haz clic** en el botón 🔒 **"Authorize"** (arriba a la derecha en Swagger)
3. **Pega** en el campo: `Bearer [tu-token-aqui]`
   - Ejemplo: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
4. **Haz clic** en "Authorize"
5. **Cierra** el modal

### 4️⃣ Prueba GET /api/Ventas

1. **Expande** el endpoint `GET /api/Ventas`
2. **Haz clic** en "Try it out"
3. **Haz clic** en "Execute"

**Deberías ver:**
- ✅ **Response code: 200 OK**
- ✅ **Response body**: Un array de ventas (puede estar vacío `[]` si no hay ventas)

### 5️⃣ Prueba GET /api/Clientes

1. **Expande** el endpoint `GET /api/Clientes`
2. **Haz clic** en "Try it out"
3. **Haz clic** en "Execute"

**Deberías ver:**
- ✅ **Response code: 200 OK**
- ✅ **Response body**: Un array de clientes

---

## 🔍 SOLUCIÓN DE PROBLEMAS

### ❌ Si aún recibes 401 Unauthorized:

#### Problema 1: Token Expirado
- **Solución**: Vuelve a hacer login (Paso 2) y obtén un nuevo token

#### Problema 2: Token mal copiado
- **Solución**: Asegúrate de copiar TODO el token, incluyendo `Bearer ` al inicio

#### Problema 3: API reiniciada
- **Solución**: Si reiniciaste la API, debes hacer login nuevamente

#### Problema 4: Rol incorrecto en el token
- **Verifica**: Decodifica tu token en https://jwt.io
- **Busca**: `"http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin"`
- **Si dice "Administrador"**: Haz login nuevamente con el nuevo token

### ❌ Si recibes 403 Forbidden:

Esto significa que estás autenticado pero no tienes permisos. Verifica:
1. El token incluye el rol "Admin"
2. Decodifica en jwt.io y busca el claim de role

### ❌ Si recibes 500 Internal Server Error:

1. Revisa los logs en la consola de la API
2. Verifica la conexión a la base de datos
3. Asegúrate de que las migraciones estén aplicadas

---

## 🎯 VERIFICACIÓN FINAL

Para verificar que TODO está correcto, decodifica tu token en **https://jwt.io**

Deberías ver algo así:

```json
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "a267d193...",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "admin@firmeza.com",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Admin Sistema",
  "jti": "2f84c10a...",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin",
  "exp": 1732476113,
  "iss": "FirmezaAPI",
  "aud": "FirmezaClients"
}
```

✅ **Puntos clave:**
- `role: "Admin"` ✅ (no "Administrador")
- `iss: "FirmezaAPI"` ✅
- `aud: "FirmezaClients"` ✅

---

## 📝 RESUMEN DE CAMBIOS REALIZADOS

1. ✅ Corregido `secrets.json` con estructura JSON anidada correcta
2. ✅ Agregado `RoleClaimType = ClaimTypes.Role` en Program.cs
3. ✅ Migración automática de rol "Administrador" → "Admin"
4. ✅ Actualizado todos los controladores de Firmeza.Web a usar "Admin"
5. ✅ Configuración JWT completa en secrets.json

**¡Todo está listo! Solo sigue los pasos en Swagger y debería funcionar.**

