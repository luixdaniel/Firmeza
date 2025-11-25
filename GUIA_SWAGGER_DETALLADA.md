# 🎯 GUÍA PASO A PASO: USAR SWAGGER CORRECTAMENTE

## ✅ VERIFICACIÓN PREVIA

La API funciona perfectamente desde PowerShell:
- ✅ Login: OK
- ✅ Clientes: OK (4 clientes)
- ✅ Ventas: OK (8 ventas)

**Si falla en Swagger pero funciona en PowerShell, el problema es CÓMO estás usando Swagger.**

---

## 📋 CREDENCIALES

```
Email: admin@firmeza.com
Password: Admin123$
```

⚠️ **IMPORTANTE:** Es `Admin123$` con DÓLAR al final, NO exclamación.

---

## 🔴 ERRORES COMUNES EN SWAGGER

### ❌ Error 1: No autorizarse ANTES de probar los endpoints
**Síntoma:** Error 401 Unauthorized en Clientes y Ventas
**Solución:** DEBES hacer login Y autorizar ANTES de probar otros endpoints

### ❌ Error 2: Copiar mal el token
**Síntoma:** Error 401 invalid_token
**Solución:** Copia TODO el token, sin espacios extras

### ❌ Error 3: Olvidar poner "Bearer " antes del token
**Síntoma:** Error 401
**Solución:** En Swagger, el campo ya dice "Bearer", solo pega el token

### ❌ Error 4: Usar un token expirado
**Síntoma:** Error 401
**Solución:** Haz login nuevamente para obtener un nuevo token

---

## 📖 PASOS CORRECTOS (CON CAPTURAS MENTALES)

### PASO 1: Abrir Swagger
```
http://localhost:5090/swagger
```

Deberías ver una página con:
- ✅ Un título "Firmeza API v1"
- ✅ Secciones: Auth, Categorias, Clientes, Productos, Ventas
- ✅ Un botón 🔒 "Authorize" arriba a la derecha

---

### PASO 2: Hacer Login

1. **Busca la sección "Auth"** (es la primera)

2. **Click en "POST /api/Auth/login"** para expandirlo

3. **Click en el botón "Try it out"** (esquina derecha)
   - El cuadro de Request body se volverá editable

4. **BORRA todo** lo que hay en Request body

5. **PEGA exactamente esto:**
```json
{
  "email": "admin@firmeza.com",
  "password": "Admin123$"
}
```

6. **Verifica que:**
   - ✅ El email está entre comillas dobles
   - ✅ La contraseña termina con $ (dólar)
   - ✅ NO hay espacios extras
   - ✅ Las comillas son comillas rectas " NO comillas tipográficas ""

7. **Click en el botón azul "Execute"**

8. **Espera a ver la respuesta**

---

### PASO 3: Verificar la Respuesta del Login

Deberías ver en "Responses":

**Code: 200** (exitoso)

**Response body:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRw...",
  "expiration": "2025-11-25T...",
  "email": "admin@firmeza.com",
  "nombreCompleto": " ",
  "roles": ["Admin"]
}
```

⚠️ **CRUCIAL:** Verifica que `"roles": ["Admin"]` (NO "Administrador")

---

### PASO 4: Copiar el Token

1. **Busca la línea que dice** `"token": "eyJ..."`

2. **Selecciona SOLO el contenido** entre las comillas (sin incluir las comillas)
   - Empieza con: `eyJhbGc...`
   - Termina con: `...algo`
   - Es un string LARGO (más de 200 caracteres)

3. **Copia el token completo**
   - Usa Ctrl+C
   - O click derecho → Copy

⚠️ **NO copies:**
- ❌ Las comillas `"`
- ❌ La palabra "token":
- ❌ Espacios antes o después

✅ **SÍ copia:**
- El texto completo desde eyJ hasta el final

---

### PASO 5: Autorizar en Swagger

1. **Scroll hasta arriba de la página**

2. **Busca el botón** 🔒 **"Authorize"** (esquina superior derecha)

3. **Click en "Authorize"**
   - Se abre un modal/ventana emergente

4. **En el campo "Value":**
   - Ya debería decir: `Bearer `
   - **PEGA el token** después de "Bearer "
   - Debería quedar: `Bearer eyJhbGc...`

5. **Click en el botón "Authorize"** (en el modal)

6. **Click en "Close"** para cerrar el modal

7. **Verifica:** El botón 🔒 ahora debería estar en un color diferente (indicando que estás autenticado)

---

### PASO 6: Probar Endpoint de Clientes

1. **Busca la sección "Clientes"**

2. **Click en "GET /api/Clientes"**

3. **Click en "Try it out"**

4. **Click en "Execute"**

5. **Espera la respuesta:**

**✅ EXITOSO:**
```
Code: 200
Response body: [array con 4 clientes]
```

**❌ SI DA ERROR 401:**
- Vuelve al PASO 4 y copia bien el token
- Vuelve al PASO 5 y autoriza nuevamente
- Asegúrate que pegaste "Bearer " + token

---

### PASO 7: Probar Endpoint de Ventas

1. **Busca la sección "Ventas"**

2. **Click en "GET /api/Ventas"**

3. **Click en "Try it out"**

4. **Click en "Execute"**

5. **Espera la respuesta:**

**✅ EXITOSO:**
```
Code: 200
Response body: [array con 8 ventas]
```

---

## 🔍 DIAGNÓSTICO DE PROBLEMAS

### Si CLIENTES da 401 pero LOGIN funcionó:

**Causa 1:** No te autorizaste
- ✅ Solución: Ve al PASO 5 y autoriza

**Causa 2:** Token copiado mal
- ✅ Solución: Vuelve al PASO 4 y copia TODO el token

**Causa 3:** Token expirado
- ✅ Solución: Haz login nuevamente (PASO 2)

### Si VENTAS da 401:

- Mismo diagnóstico que Clientes
- Ambos endpoints requieren el mismo rol "Admin"

### Si LOGIN da 401:

**Causa 1:** Contraseña incorrecta
- ✅ Verifica: `Admin123$` (con $ al final)
- ✅ NO uses: `Admin123!` (con exclamación)

**Causa 2:** JSON mal formateado
- ✅ Usa exactamente el JSON del PASO 2

### Si ningún endpoint responde:

**Causa:** La API no está corriendo
- ✅ Solución: Abre una terminal:
  ```cmd
  cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
  dotnet run
  ```

---

## 🎯 CHECKLIST RÁPIDO

Antes de probar endpoints protegidos:

- [ ] API está corriendo (http://localhost:5090/health responde)
- [ ] Hice login con admin@firmeza.com / Admin123$
- [ ] El login devolvió código 200
- [ ] Copié TODO el token (empieza con eyJ...)
- [ ] Click en 🔒 Authorize
- [ ] Pegué: Bearer [token]
- [ ] Click en Authorize y luego Close
- [ ] El botón 🔒 cambió de color

Si todos están ✅, los endpoints DEBEN funcionar.

---

## 📸 EJEMPLO VISUAL DE TOKEN

**Token correcto:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImEyNjdkMTkzLTZkY2ItNDYxZS05OGEwLTZhNDJhMDUxZDExYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGZpcm1lemEuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IiAiLCJqdGkiOiJiMTdkNTU2MS03NDdiLTQ5ZTUtYTJlMC1mZTlhYTUzYzNmMDQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc2NDA0ODE4MSwiaXNzIjoiRmlybWV6YUFQSSIsImF1ZCI6IkZpcm1lemFDbGllbnRzIn0.bmnsMm4_J00CpBZcCpnS82IA-mTENx5zGTG_6uwiARQ
```

**En el campo Authorize debe quedar:**
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImEyNjdkMTkzLTZkY2ItNDYxZS05OGEwLTZhNDJhMDUxZDExYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGZpcm1lemEuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IiAiLCJqdGkiOiJiMTdkNTU2MS03NDdiLTQ5ZTUtYTJlMC1mZTlhYTUzYzNmMDQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc2NDA0ODE4MSwiaXNzIjoiRmlybWV6YUFQSSIsImF1ZCI6IkZpcm1lemFDbGllbnRzIn0.bmnsMm4_J00CpBZcCpnS82IA-mTENx5zGTG_6uwiARQ
```

---

## 💡 CONSEJO FINAL

Si sigues teniendo problemas después de seguir TODOS los pasos:

1. **Abre las herramientas de desarrollador del navegador** (F12)
2. **Ve a la pestaña "Network"**
3. **Intenta el endpoint que falla**
4. **Click en la petición que aparece**
5. **Ve a "Headers"**
6. **Busca "Authorization"**
7. **Verifica que diga:** `Bearer eyJ...`

Si no aparece el header Authorization, entonces Swagger no está enviando el token.

---

**¡Con estos pasos, DEBE funcionar!** 🚀

