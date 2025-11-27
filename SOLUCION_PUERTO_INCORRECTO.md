# ✅ PROBLEMA RESUELTO - Puerto Incorrecto en Frontend

## 🎯 Problema Identificado

**Error:** "Error al registrar usuario. Por favor intenta nuevamente."

**Causa Real:** El frontend estaba intentando conectarse al puerto **5090** pero la API está corriendo en el puerto **5000**.

---

## 🔧 Solución Aplicada

### Archivo Corregido: `/lib/axios.ts`

**ANTES (Incorrecto):**
```typescript
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5090';
```

**AHORA (Correcto):**
```typescript
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';
```

---

## ✅ Verificación Realizada

### Test de la API:
```bash
curl -X POST http://localhost:5000/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testrol@test.com",
    "password": "Test123$",
    "confirmPassword": "Test123$",
    "nombre": "Test",
    "apellido": "Rol"
  }'
```

**Resultado:** ✅ EXITOSO
- Usuario creado correctamente
- Rol "Cliente" asignado automáticamente
- Token JWT generado

---

## 🚀 Pasos para Probar

### 1. Reiniciar el Frontend

**IMPORTANTE:** Como cambiamos el código, necesitas reiniciar el servidor de desarrollo.

En la terminal donde corre el frontend (puerto 3000):

```bash
# Presiona Ctrl+C para detener

# Luego inicia de nuevo:
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

### 2. Limpiar Caché del Navegador (Opcional)

Si el problema persiste:
- Presiona **Ctrl+Shift+R** para forzar recarga
- O abre en ventana de incógnito

### 3. Registrarte Nuevamente

Ve a: http://localhost:3000/registro

Completa el formulario:
- **Nombre:** luis
- **Apellido:** cera
- **Email:** ceraluis4@gmail.com
- **Teléfono:** +57 300 123 4567
- **Contraseña:** MiPassword123$ (o cualquiera que cumpla requisitos)
- **Confirmar:** MiPassword123$

Click en **"Crear Cuenta"**

---

## ✅ Resultado Esperado

1. ✅ Usuario se crea en la base de datos
2. ✅ Se asigna rol "Cliente" automáticamente
3. ✅ Se genera token JWT
4. ✅ Auto-login
5. ✅ Redirección a `/cliente/tienda`

---

## 📊 Configuración de Puertos

| Componente | Puerto | URL |
|------------|--------|-----|
| API Backend | 5000 | http://localhost:5000 |
| Frontend | 3000 | http://localhost:3000 |
| Swagger | 5000 | http://localhost:5000/swagger |

---

## 🔍 Verificar Roles en la Base de Datos

Si quieres confirmar que los roles están bien, conecta a PostgreSQL:

```bash
psql -U postgres -d firmeza_db -c "SELECT * FROM \"AspNetRoles\";"
```

**Debes ver:**
```
Id | Name    | NormalizedName
---|---------|---------------
1  | Admin   | ADMIN
2  | Cliente | CLIENTE
```

---

## ⚠️ Requisitos de Contraseña

La contraseña debe tener:
- ✅ Mínimo 6 caracteres
- ✅ Al menos 1 mayúscula (A-Z)
- ✅ Al menos 1 minúscula (a-z)
- ✅ Al menos 1 número (0-9)

**Ejemplos válidos:**
- `Password123`
- `MiClave456`
- `Test123$`

---

## 📝 Resumen

### El Error NO era de Roles

Los roles están correctamente configurados:
- ✅ Rol "Admin" existe
- ✅ Rol "Cliente" existe
- ✅ Se asigna "Cliente" automáticamente al registrarse

### El Error Era de Puerto

- ❌ Frontend apuntaba a puerto 5090
- ✅ API corre en puerto 5000
- ✅ **Ahora corregido**

---

## 🎉 ¡Listo para Usar!

Después de **reiniciar el frontend**, el registro debería funcionar perfectamente.

**Pasos finales:**
1. Reiniciar frontend (Ctrl+C y `npm run dev`)
2. Ir a http://localhost:3000/registro
3. Registrarse
4. ¡Disfrutar! 🎊

---

**Fecha:** 2025-11-26
**Estado:** ✅ Problema resuelto
**Cambio:** Puerto 5090 → 5000

