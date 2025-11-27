# ✅ CORRECCIÓN DE PUERTOS - API y Frontend

## 🔍 Problema Encontrado
La API estaba configurada para correr en el puerto **5000**, pero necesitábamos que corriera en el puerto **5090** para que coincidiera con el frontend.

---

## ✅ Soluciones Aplicadas

### 1. Configuración de la API (Puerto 5090)

**Archivo modificado:** `ApiFirmeza.Web/Properties/launchSettings.json`

```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5090"  // ✅ Cambiado de 5000 a 5090
    },
    "https": {
      "applicationUrl": "https://localhost:5091;http://localhost:5090"  // ✅ Actualizado
    }
  }
}
```

### 2. Configuración del Frontend (Conecta a 5090)

**Archivo creado/modificado:** `firmeza-client/.env.local`

```env
NEXT_PUBLIC_API_URL=http://localhost:5090
```

---

## 🚀 Pasos para Aplicar los Cambios

### 1. Detener los Servicios Actuales
Si tienes la API o el frontend corriendo, deténlos (Ctrl+C en cada terminal).

### 2. Reiniciar la API en el Puerto 5090
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

**Salida esperada:**
```
Now listening on: http://localhost:5090
Application started. Press Ctrl+C to shut down.
```

### 3. Reiniciar el Frontend
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client
npm run dev
```

**Salida esperada:**
```
- Local:        http://localhost:3000
- Ready in X.Xs
```

---

## 🧪 Verificación

### 1. Verificar que la API corre en 5090
Abre tu navegador o usa curl:
```cmd
curl http://localhost:5090/swagger/index.html
```
Deberías ver la página de Swagger.

### 2. Verificar que el Frontend conecta correctamente
1. Abre http://localhost:3000
2. Abre las herramientas de desarrollador (F12)
3. Ve a la pestaña "Network" (Red)
4. Intenta hacer login o registro
5. Verifica que las peticiones van a: `http://localhost:5090/api/...`

### 3. Probar el Flujo Completo

#### A. Registrar un Cliente
**URL:** http://localhost:3000/auth/register

**Datos de prueba:**
```
Nombre: Carlos
Apellido: Mendoza
Email: carlos.mendoza@example.com
Teléfono: 3001234567
Contraseña: Test123$
Confirmar Contraseña: Test123$
```

#### B. Hacer Login
**URL:** http://localhost:3000/auth/login

**Credenciales:**
```
Email: carlos.mendoza@example.com
Contraseña: Test123$
```

#### C. Login como Admin (para probar Swagger)
**URL:** http://localhost:5090/swagger/index.html

**Credenciales Admin:**
```
Email: admin@firmeza.com
Contraseña: Admin123$
```

---

## 📋 Configuración de Puertos Final

| Servicio | Puerto | URL |
|----------|--------|-----|
| **API** | 5090 | http://localhost:5090 |
| **Swagger** | 5090 | http://localhost:5090/swagger |
| **Frontend** | 3000 | http://localhost:3000 |

---

## ⚠️ Notas Importantes

### 1. Si la API no inicia en 5090
Verifica que el puerto no esté en uso:
```cmd
netstat -ano | findstr :5090
```

Si está en uso, mata el proceso o cambia el puerto.

### 2. Si el Frontend sigue conectando al puerto 5000
- Asegúrate de haber creado el archivo `.env.local` con el contenido correcto
- **REINICIA** el servidor de Next.js (las variables de entorno solo se leen al iniciar)
- Limpia el caché del navegador (Ctrl+Shift+R)

### 3. Errores CORS
Si ves errores de CORS en la consola del navegador:
- Verifica que la API esté corriendo
- Verifica que CORS esté habilitado en `Program.cs` (ya está configurado)

---

## 🎯 Checklist Final

- [x] ✅ `launchSettings.json` actualizado con puerto 5090
- [x] ✅ `.env.local` creado con URL correcta
- [ ] 🔄 API reiniciada y corriendo en puerto 5090
- [ ] 🔄 Frontend reiniciado y corriendo en puerto 3000
- [ ] 🔄 Registro de cliente probado
- [ ] 🔄 Login probado
- [ ] 🔄 Peticiones HTTP verificadas en DevTools

---

## 🐛 Solución de Problemas

### Problema: "Port 5090 already in use"
```cmd
# Windows: Encontrar el proceso
netstat -ano | findstr :5090

# Matar el proceso (reemplaza PID con el número que aparece)
taskkill /PID [PID] /F
```

### Problema: Frontend sigue conectando a 5000
1. Detén el servidor de Next.js
2. Verifica que `.env.local` existe y tiene el contenido correcto:
   ```cmd
   type C:\Users\luisc\RiderProjects\Firmeza\firmeza-client\.env.local
   ```
3. Debería mostrar: `NEXT_PUBLIC_API_URL=http://localhost:5090`
4. Reinicia Next.js
5. Limpia caché del navegador

### Problema: Error 401 Unauthorized
Asegúrate de:
1. Estar enviando el token JWT en el header `Authorization: Bearer [token]`
2. El token no haya expirado
3. El usuario tenga el rol correcto para el endpoint

---

## 📊 Estado del Proyecto

| Componente | Estado | Notas |
|------------|--------|-------|
| API Puerto | ✅ Configurado | 5090 |
| Frontend Puerto | ✅ Configurado | 3000 |
| CORS | ✅ Habilitado | Permite todas las peticiones |
| JWT | ✅ Configurado | Expira en 24 horas |
| Base de Datos | ✅ Conectada | PostgreSQL en localhost |
| Swagger | ✅ Disponible | http://localhost:5090/swagger |

---

## 🚀 Comandos Rápidos de Inicio

### Iniciar Todo (2 terminales)

**Terminal 1 - API:**
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

**Terminal 2 - Frontend:**
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client
npm run dev
```

### Verificar que Todo Funciona
```cmd
# Verificar API
curl http://localhost:5090/api/Categorias

# Verificar Frontend (abre en navegador)
start http://localhost:3000
```

---

¡Configuración completada! 🎉

Ahora la API y el Frontend están correctamente sincronizados en los puertos 5090 y 3000 respectivamente.

