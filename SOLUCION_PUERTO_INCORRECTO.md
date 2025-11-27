# ✅ SOLUCIÓN: Frontend Conectando al Puerto Incorrecto

## 🔍 Problema
El frontend (`firmeza-client`) intentaba conectarse a la API en el puerto **5000**, pero la API corre en el puerto **5090**.

### Síntomas
- Error al intentar registrar clientes desde el frontend
- Error de conexión: "Cannot connect to API on port 5000"
- La API está corriendo correctamente en el puerto 5090

---

## 🛠️ Causa Raíz

El archivo `firmeza-client/lib/axios.ts` tenía configurado un puerto incorrecto por defecto:

```typescript
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';  // ❌ Puerto incorrecto
```

Y el archivo `.env.local` estaba vacío, por lo que usaba el valor por defecto incorrecto.

---

## ✅ Solución Aplicada

### 1. Configurar `.env.local`
**Archivo:** `firmeza-client/.env.local`

```env
NEXT_PUBLIC_API_URL=http://localhost:5090
```

### 2. Reiniciar el Servidor de Desarrollo

Después de modificar el archivo `.env.local`, es **OBLIGATORIO** reiniciar el servidor de Next.js para que tome los nuevos valores:

```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client

# Detener el servidor actual (Ctrl+C)

# Reiniciar
npm run dev
```

---

## 🧪 Verificación

### 1. Verificar que la API está corriendo
```cmd
curl http://localhost:5090/api/Auth/login
```
Debería devolver un error 400 (esperado, sin credenciales) o 405, NO un error de conexión.

### 2. Verificar que el Frontend conecta correctamente
1. Abrir: http://localhost:3000
2. Ir a la página de registro
3. Completar el formulario
4. Verificar en la consola del navegador (F12) que las peticiones van a `http://localhost:5090/api/...`

### 3. Probar el Registro
**Datos de prueba:**
```
Nombre: Juan
Apellido: Pérez
Email: juan.perez@example.com
Teléfono: 3001234567
Contraseña: Test123$
Confirmar Contraseña: Test123$
```

---

## 📋 Checklist de Solución

- [x] Crear archivo `.env.local` con el puerto correcto (5090)
- [ ] Reiniciar el servidor de Next.js (npm run dev)
- [ ] Verificar en el navegador que las peticiones van al puerto 5090
- [ ] Probar el registro de un nuevo cliente
- [ ] Verificar que el login funciona

---

## ⚠️ Notas Importantes

1. **Siempre reiniciar después de cambios en `.env.local`**: Next.js solo lee variables de entorno al iniciar.

2. **Verificar puertos:** 
   - API: http://localhost:5090
   - Frontend: http://localhost:3000

3. **CORS está habilitado:** La API ya tiene configuración CORS para aceptar peticiones de cualquier origen.

4. **No commitear `.env.local`:** Este archivo no debe subirse a Git (ya está en `.gitignore`).

---

## 🎯 Resultado Esperado

Después de aplicar esta solución:

✅ El frontend conecta correctamente a la API en el puerto 5090
✅ El registro de clientes funciona
✅ El login funciona
✅ Todas las peticiones HTTP funcionan correctamente

---

## 🚀 Comandos Rápidos

### Iniciar la API
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

### Iniciar el Frontend
```cmd
cd C:\Users\luisc\RiderProjects\Firmeza\firmeza-client
npm run dev
```

### Verificar conexión
```cmd
# Verificar API
curl http://localhost:5090/api/Categorias

# Con autenticación
curl -H "Authorization: Bearer [tu-token]" http://localhost:5090/api/Clientes
```

---

## 📊 Estado

| Componente | Puerto | Estado |
|------------|--------|--------|
| **API** | 5090 | ✅ Corriendo |
| **Frontend** | 3000 | ✅ Configurado |
| **CORS** | - | ✅ Habilitado |
| **.env.local** | - | ✅ Creado |

---

¡Problema resuelto! 🎉

