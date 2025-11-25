# 🔧 SOLUCIÓN AL ERROR 401 FORBIDDEN - ROL INCORRECTO

## 📋 Problema Identificado

El token JWT contiene el rol **"Administrador"** pero la API espera el rol **"Admin"**.

Esto ocurrió porque:
- El proyecto `Firmeza.Web` usaba el rol "Administrador"
- El proyecto `ApiFirmeza.Web` usa el rol "Admin"
- Ambos comparten la misma base de datos

## ✅ Cambios Realizados

### 1. Estandarización de Roles
He actualizado todos los controladores del área Admin en `Firmeza.Web` para usar **"Admin"** en lugar de "Administrador":
- ✅ `DashboardController.cs`
- ✅ `ClientesController.cs`
- ✅ `ImportacionController.cs`
- ✅ `ProductosController.cs`
- ✅ `VentasController.cs`

### 2. Migración Automática en la API
He modificado `ApiFirmeza.Web\Program.cs` para que al iniciar:
- Detecte si existe el rol "Administrador"
- Migre todos los usuarios de "Administrador" a "Admin"
- Elimine el rol viejo

## 🚀 Pasos a Seguir

### Opción 1: Reiniciar la API (RECOMENDADO)

1. **Detén la API actual** si está corriendo (Ctrl+C o cerrar terminal)

2. **Inicia la API nuevamente** desde el directorio `ApiFirmeza.Web`:
   ```bash
   cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
   dotnet run
   ```

3. **Observa la consola** - deberías ver mensajes como:
   ```
   🔄 Migrando rol 'Administrador' a 'Admin'...
   ✅ Usuario admin@firmeza.com migrado al rol 'Admin'
   ✅ Rol 'Administrador' eliminado
   ```

4. **Inicia sesión nuevamente** en Swagger:
   - POST `/api/Auth/login`
   - Email: `admin@firmeza.com`
   - Password: `Admin123!`

5. **Copia el nuevo token** y úsalo en el botón "Authorize"

6. **Prueba los endpoints** `/api/Ventas` y `/api/Clientes` - ¡deberían funcionar! ✅

### Opción 2: Actualización Manual de Base de Datos

Si por alguna razón la migración automática no funciona, ejecuta este SQL en PostgreSQL:

```sql
-- Actualizar el rol Administrador a Admin
UPDATE "AspNetRoles" 
SET "Name" = 'Admin', "NormalizedName" = 'ADMIN' 
WHERE "Name" = 'Administrador';
```

Luego:
1. Reinicia la API
2. Inicia sesión nuevamente
3. Usa el nuevo token

## 🔍 Verificar que Funcionó

Después de iniciar sesión con el nuevo token, decodifica el JWT en [jwt.io](https://jwt.io) y verifica que contenga:

```json
{
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin"
}
```

✅ Si dice "Admin" (no "Administrador"), ¡el problema está resuelto!

## 📝 Notas Adicionales

- Ambos proyectos ahora usan consistentemente el rol **"Admin"**
- La contraseña del admin puede ser `Admin123!` o `Admin123$` (verifica cuál funciona)
- Todos los cambios ya están aplicados en el código
- Solo necesitas reiniciar la API y obtener un nuevo token

---

**¡La migración es automática! Solo reinicia la API y vuelve a iniciar sesión.**

