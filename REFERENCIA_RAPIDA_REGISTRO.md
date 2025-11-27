# 🚀 Referencia Rápida - Registro de Clientes

## ⚡ Inicio Rápido

### Ejecutar el Sistema:
```bash
# Terminal 1 - API
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run

# Terminal 2 - Frontend
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

### Acceder:
- **Frontend**: http://localhost:3000
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

---

## 📍 URLs Importantes

| Página | URL | Descripción |
|--------|-----|-------------|
| Inicio | http://localhost:3000 | Landing page |
| Login | http://localhost:3000/login | Iniciar sesión |
| **Registro** | http://localhost:3000/registro | **Crear cuenta** ✨ |
| Tienda | http://localhost:3000/cliente/tienda | Catálogo |

---

## 🔑 Credenciales de Prueba

### Usuario Existente:
```
Email:    cliente@firmeza.com
Password: Cliente123$
```

### Nuevo Usuario (Registro):
```
Nombre:     [Tu nombre]
Apellido:   [Tu apellido]
Email:      [tu@email.com]
Teléfono:   [Opcional]
Password:   [Mínimo 6 caracteres]
```

---

## 🎯 Flujo Rápido

### Registrar Nuevo Cliente:
1. Ir a http://localhost:3000
2. Click "Registrarse"
3. Llenar formulario
4. Click "Crear Cuenta"
5. ✅ Auto-redirige a tienda

### Si Ya Tienes Cuenta:
1. Ir a http://localhost:3000
2. Click "Iniciar Sesión"
3. Ingresar credenciales
4. ✅ Acceso a tienda

---

## 📋 Checklist de Verificación

Después de iniciar el sistema:

- [ ] API corriendo en puerto 5000
- [ ] Frontend corriendo en puerto 3000
- [ ] Página principal muestra botón "Registrarse"
- [ ] Formulario de registro accesible
- [ ] Login tiene enlace a registro
- [ ] Tema verde en todo el portal de clientes

---

## 🔧 Solución de Problemas

### Error: "El email ya está registrado"
**Solución**: Usar un email diferente

### Error: "Las contraseñas no coinciden"
**Solución**: Verificar que ambos campos sean idénticos

### Error: "Connection refused"
**Solución**: Verificar que la API esté corriendo

### Error de CORS
**Solución**: Verificar configuración CORS en API para puerto 3000

---

## 📚 Documentación Completa

- **Detallada**: `FUNCIONALIDAD_REGISTRO_CLIENTES.md`
- **Resumen**: `RESUMEN_CAMBIOS_FRONTEND_CLIENTE.md`
- **Ejecución**: `GUIA_EJECUCION_PORTALES.md`

---

## ✅ Estado Actual

**Frontend de Cliente:**
- ✅ Página principal (sin admin)
- ✅ Login
- ✅ **Registro** ✨ NUEVO
- ✅ Tienda
- ✅ Tema verde

**Backend API:**
- ✅ Endpoint de registro
- ✅ Endpoint de login
- ✅ JWT tokens
- ✅ Rol "Cliente" automático

---

**Todo listo para usar! 🎉**

