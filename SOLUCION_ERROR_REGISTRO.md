# 🔧 SOLUCIÓN: Error al Registrar Usuario

## ❌ Problema Detectado

**Error mostrado:** "Error al registrar usuario. Por favor intenta nuevamente."

**Causa:** La API no está corriendo o no es accesible en `http://localhost:5000`

---

## ✅ Solución Paso a Paso

### Paso 1: Verificar si la API está corriendo

```bash
# Verificar si hay algo escuchando en puerto 5000
curl http://localhost:5000/health
```

**Si ves un error:** La API no está corriendo → Continúa al Paso 2

**Si ves una respuesta JSON:** La API está corriendo → Continúa al Paso 4

---

### Paso 2: Iniciar la API

Abre una **nueva terminal** y ejecuta:

```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

**Espera a ver estos mensajes:**
```
✅ Rol 'Admin' creado
✅ Rol 'Cliente' creado
✅ Usuario administrador creado: admin@firmeza.com / Admin123$
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

---

### Paso 3: Verificar que la API esté funcionando

En otra terminal:

```bash
curl http://localhost:5000/health
```

**Debes ver algo como:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-26T...",
  "environment": "Development"
}
```

---

### Paso 4: Verificar configuración de la API

Si la API no inicia correctamente, verifica:

#### 4.1. Cadena de conexión

```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
cat secrets.json 2>/dev/null || echo "Archivo secrets.json no encontrado"
```

**Debe contener:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=firmeza_db;Username=postgres;Password=TU_PASSWORD"
  },
  "JwtSettings": {
    "SecretKey": "tu_clave_secreta_muy_larga_minimo_32_caracteres"
  }
}
```

#### 4.2. PostgreSQL corriendo

```bash
sudo systemctl status postgresql
# o
pg_isready
```

---

### Paso 5: Reintentar el Registro

1. **Asegúrate de que la API esté corriendo** (debes ver "Now listening on: http://localhost:5000")

2. **Ve al frontend de registro:**
   ```
   http://localhost:3000/registro
   ```

3. **Completa el formulario:**
   - Nombre: Luis
   - Apellido: Cera
   - Email: ceraluis4@gmail.com
   - Teléfono: +57 300 123 4567 (opcional)
   - Contraseña: Minimo6caracteres123$
   - Confirmar: Minimo6caracteres123$

4. **Click en "Crear Cuenta"**

5. **Si aún hay error:** Abre la consola del navegador (F12) y busca el error exacto

---

## 🔍 Diagnóstico Avanzado

Si el problema persiste, ejecuta este comando para ver el error exacto:

```bash
cd /home/Coder/Escritorio/Firmeza

# Test de registro directo
curl -X POST http://localhost:5000/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@test.com",
    "password": "Test123$",
    "confirmPassword": "Test123$",
    "nombre": "Test",
    "apellido": "Usuario",
    "telefono": "+57 300 123 4567"
  }' -v
```

---

## 🐛 Errores Comunes y Soluciones

### Error 1: "Connection refused"
**Causa:** API no está corriendo
**Solución:** Ejecutar `dotnet run` en ApiFirmeza.Web

### Error 2: "Role 'Cliente' does not exist"
**Causa:** Los roles no se crearon en la base de datos
**Solución:** 
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet ef database drop --force
dotnet ef database update
dotnet run
```

### Error 3: "Connection string not found"
**Causa:** secrets.json no configurado
**Solución:**
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=firmeza_db;Username=postgres;Password=TU_PASSWORD"
dotnet user-secrets set "JwtSettings:SecretKey" "clave_secreta_minimo_32_caracteres_para_jwt_token_seguro"
```

### Error 4: "Password requires: digit, uppercase, lowercase"
**Causa:** La contraseña no cumple los requisitos
**Solución:** Usar contraseña como: `MiPassword123$`
- Al menos 6 caracteres
- Al menos 1 dígito
- Al menos 1 mayúscula
- Al menos 1 minúscula

---

## 📋 Checklist de Verificación

Antes de intentar registrarte:

- [ ] PostgreSQL está corriendo
- [ ] La API está corriendo en puerto 5000
- [ ] `curl http://localhost:5000/health` responde
- [ ] El frontend está corriendo en puerto 3000
- [ ] Has configurado secrets.json
- [ ] La contraseña cumple requisitos (6+ chars, mayúscula, minúscula, número)

---

## 🎯 Comando Rápido para Iniciar Todo

**Terminal 1 - API:**
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

**Terminal 2 - Frontend:**
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

**Espera a ver:**
- API: "Now listening on: http://localhost:5000"
- Frontend: "ready - started server on 0.0.0.0:3000"

**Luego ve a:**
```
http://localhost:3000/registro
```

---

## 💡 Mensaje de Error Mejorado

He actualizado el código para que ahora muestre un mensaje más claro cuando no puede conectarse a la API:

**Antes:**
```
Error al registrar usuario. Por favor intenta nuevamente.
```

**Ahora:**
```
No se puede conectar con el servidor. Verifica que la API esté corriendo en http://localhost:5000
```

---

## 📝 Resumen

**El problema principal:** La API no está accesible en http://localhost:5000

**La solución:** Iniciar la API con `dotnet run` en el directorio ApiFirmeza.Web

**Verificación:** `curl http://localhost:5000/health` debe responder

---

**Fecha:** 2025-11-26
**Estado:** Código actualizado para mejor diagnóstico

