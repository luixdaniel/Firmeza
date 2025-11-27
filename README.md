# 🛡️ Firmeza - Sistema de Gestión de Ventas

Sistema completo de gestión con **tres portales separados**: API REST, Portal Admin (Razor) y Portal Cliente (Next.js)

## ⚠️ IMPORTANTE: Tres Portales Separados

Este proyecto tiene **3 componentes independientes** que **NO deben mezclarse**:

```
┌─────────────────────────────────────────────────────┐
│  1. API REST         → Puerto 5000 (Backend)       │
│  2. Portal Admin     → Puerto 5002 (Admins)        │
│  3. Portal Clientes  → Puerto 3000 (Clientes)      │
└─────────────────────────────────────────────────────┘
```

| Componente | Puerto | Tecnología | Propósito | Usuarios |
|-----------|--------|------------|-----------|----------|
| **API REST** | 5000 | ASP.NET Core Web API | Backend | Sistema |
| **Portal Admin** | 5002 | ASP.NET MVC + Razor | Gestión administrativa | Administradores |
| **Portal Cliente** | 3000 | Next.js + TypeScript | Portal web | Clientes |

---

## 🚀 Inicio Rápido

### Opción 1: Script Automático (Recomendado)

```bash
cd /home/Coder/Escritorio/Firmeza
./iniciar-portales.sh
```

### Opción 2: Manual (3 Terminales)

```bash
# Terminal 1: API REST
cd ApiFirmeza.Web
dotnet run
# → http://localhost:5000

# Terminal 2: Portal Admin
cd Firmeza.Web
dotnet run
# → http://localhost:5002

# Terminal 3: Portal Cliente
cd firmeza-client
npm install  # Solo la primera vez
npm run dev
# → http://localhost:3000
```

---

## 🔐 Credenciales de Acceso

### 👨‍💼 Portal de Administradores (Puerto 5002)
```
URL: http://localhost:5002
Login: http://localhost:5002/Identity/Account/Login
Email: admin@firmeza.com
Password: Admin123$
Rol: Admin
```

### 👥 Portal de Clientes (Puerto 3000)
```
URL: http://localhost:3000
Login: http://localhost:3000/login
Email: cliente@firmeza.com
Password: Cliente123$
Rol: Cliente
```

---

## 🎯 ¿Qué Portal Usar?

### Si eres ADMINISTRADOR:
✅ Usa: **Firmeza.Web** (Puerto 5002)
- Gestión completa de productos, clientes y ventas
- Importación/Exportación masiva
- Reportes y estadísticas

### Si eres CLIENTE:
✅ Usa: **firmeza-client** (Puerto 3000)
- Ver catálogo de productos
- Realizar compras
- Ver historial de compras

---

## 📁 Estructura del Proyecto

```
Firmeza/
├── ApiFirmeza.Web/          # API REST (Puerto 5000)
│   ├── Controllers/         # Endpoints de la API
│   ├── DTOs/               # Data Transfer Objects
│   └── Mappings/           # AutoMapper profiles
│
├── Firmeza.Web/             # Portal Admin (Puerto 5002)
│   ├── Areas/
│   │   ├── Admin/          # Área administrativa
│   │   └── Identity/       # Login con Identity
│   ├── Controllers/        # Controladores MVC
│   ├── Views/             # Vistas Razor
│   └── Services/          # Lógica de negocio
│
└── firmeza-client/          # Portal Cliente (Puerto 3000)
    ├── app/               # Páginas de Next.js
    ├── services/          # Servicios de API
    └── types/             # Tipos TypeScript
```

---

## 🛠️ Tecnologías

### Backend (API y Portal Admin)
- .NET 8.0
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Supabase)
- ASP.NET Core Identity
- AutoMapper
- JWT Authentication

### Frontend (Portal Cliente)
- Next.js 14
- TypeScript
- TailwindCSS
- Axios
- React

---

## 📚 Documentación Completa

### Guías Principales
- **[INICIO_RAPIDO.md](INICIO_RAPIDO.md)** - Guía de inicio rápido
- **[ARQUITECTURA_PORTALES.md](ARQUITECTURA_PORTALES.md)** - Arquitectura completa del sistema
- **[GUIA_VISUAL_PORTALES.md](GUIA_VISUAL_PORTALES.md)** - Guía visual con diagramas
- **[SOLUCION_SEPARACION_PORTALES.md](SOLUCION_SEPARACION_PORTALES.md)** - Solución implementada

### Guías Técnicas
- **[CONFIGURAR_SECRETS_JWT.md](CONFIGURAR_SECRETS_JWT.md)** - Configuración de JWT
- **[GUIA_PROBAR_SWAGGER.md](GUIA_PROBAR_SWAGGER.md)** - Probar la API con Swagger
- **[IMPORTACION_MASIVA.md](IMPORTACION_MASIVA.md)** - Importación de datos
- **[EXPORTACION_PDF.md](EXPORTACION_PDF.md)** - Exportación a PDF

---

## 🔒 Autenticación y Roles

### Portal Admin (Identity)
- **Autenticación:** ASP.NET Core Identity (Cookies)
- **Roles:** Admin, Cliente
- **Usuario semilla:** admin@firmeza.com / Admin123$
- **Protección:** Área Admin protegida por rol

### Portal Cliente (JWT)
- **Autenticación:** JWT (JSON Web Tokens)
- **Almacenamiento:** LocalStorage del navegador
- **API:** Consume ApiFirmeza.Web en puerto 5000
- **Usuario semilla:** cliente@firmeza.com / Cliente123$

---

## 🗄️ Base de Datos

### PostgreSQL (Supabase)
- **Migraciones:** Se aplican automáticamente al arrancar
- **Seed data:** Usuarios y roles se crean automáticamente

### Comandos de Migraciones

```bash
cd Firmeza.Web
dotnet ef migrations add NombreMigracion
dotnet ef database update
```

---

## 🚫 Errores Comunes

### ❌ "No sale el portal de admin"
**Solución:** Accede a `http://localhost:5002` (NO 5000)

### ❌ "No puedo hacer login de admin en el puerto 3000"
**Solución:** Los portales están separados. Admins usan puerto 5002.

### ❌ "Puerto en uso"
**Solución:**
```bash
lsof -ti:5000 | xargs kill -9
lsof -ti:5002 | xargs kill -9
lsof -ti:3000 | xargs kill -9
```

---

## ✅ Checklist de Verificación

Después de iniciar, verifica:

- [ ] API REST responde en `http://localhost:5000/swagger`
- [ ] Portal Admin carga en `http://localhost:5002`
- [ ] Puedes hacer login como admin en puerto 5002
- [ ] Portal Cliente carga en `http://localhost:3000`
- [ ] Puedes hacer login como cliente en puerto 3000
- [ ] Los portales están completamente separados

---

## 📞 Soporte

Para más información, revisa la documentación en los archivos:
- `ARQUITECTURA_PORTALES.md` - Entender la separación de portales
- `INICIO_RAPIDO.md` - Comenzar a usar el sistema
- `GUIA_VISUAL_PORTALES.md` - Ver diagramas y ejemplos visuales

---

## 📝 Notas

- Asegura que la cadena de conexión esté en `appsettings.json`
- En producción/Docker (Linux), respeta mayúsculas de carpetas de Views
- NO mezclar los portales - están diseñados para usuarios diferentes

---

**Versión:** 2.0  
**Fecha:** 2025-01-26  
**Estado:** ✅ Producción

