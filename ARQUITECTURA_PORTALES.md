# 🏗️ Arquitectura de Portales - Firmeza

## ⚠️ IMPORTANTE: Portales Separados

Este proyecto tiene **TRES componentes independientes** que NO deben mezclarse:

---

## 📊 Componentes del Sistema

### 1️⃣ **ApiFirmeza.Web** - API REST
- **Tecnología**: ASP.NET Core Web API
- **Puerto**: 5000 / 5001 (https)
- **Propósito**: Backend API para el portal de clientes
- **Autenticación**: JWT (JSON Web Tokens)
- **Ubicación**: `/ApiFirmeza.Web/`

**Endpoints principales:**
- `POST /Auth/login` - Login con JWT
- `GET /Productos` - Listar productos
- `GET /Ventas` - Listar ventas
- `GET /Clientes` - Listar clientes
- `GET /Categorias` - Listar categorías

**Cómo ejecutar:**
```bash
cd ApiFirmeza.Web
dotnet run
```

---

### 2️⃣ **Firmeza.Web** - Portal de ADMINISTRADORES (Razor)
- **Tecnología**: ASP.NET Core MVC + Razor Pages + Identity
- **Puerto**: 5002 / 5003 (o diferente al de la API)
- **Propósito**: Portal web SOLO para administradores
- **Autenticación**: ASP.NET Core Identity (Cookies)
- **Ubicación**: `/Firmeza.Web/`

**Rutas principales:**
- `/` - Página de inicio (redirige a login o dashboard)
- `/Identity/Account/Login` - Login de administradores
- `/Admin/Dashboard` - Panel de administración
- `/Admin/Productos` - Gestión de productos
- `/Admin/Clientes` - Gestión de clientes
- `/Admin/Ventas` - Gestión de ventas

**Usuarios:**
- ✅ Administradores con rol "Admin"
- ❌ NO es para clientes

**Cómo ejecutar:**
```bash
cd Firmeza.Web
dotnet run
```

---

### 3️⃣ **firmeza-client** - Portal de CLIENTES (Next.js)
- **Tecnología**: Next.js 14 + TypeScript + TailwindCSS
- **Puerto**: 3000
- **Propósito**: Portal web SOLO para clientes
- **Autenticación**: JWT (consume ApiFirmeza.Web)
- **Ubicación**: `/firmeza-client/`

**Rutas principales:**
- `/` - Página de inicio
- `/login` - Login de clientes
- `/cliente` - Panel del cliente
- `/clientes` - Listado de clientes
- `/productos` - Catálogo de productos

**Usuarios:**
- ✅ Clientes normales
- ❌ NO es para administradores

**Cómo ejecutar:**
```bash
cd firmeza-client
npm run dev
```

---

## 🔐 Diferencias en Autenticación

| Aspecto | Portal Admin (Firmeza.Web) | Portal Cliente (firmeza-client) |
|---------|---------------------------|--------------------------------|
| **Tecnología** | ASP.NET Core Identity | JWT + API REST |
| **Almacenamiento** | Cookies de sesión | LocalStorage (token) |
| **Backend** | Base de datos directa | API REST |
| **Login** | `/Identity/Account/Login` | `/login` → API |
| **Usuarios** | Administradores | Clientes |

---

## 🚫 ERRORES COMUNES A EVITAR

### ❌ **NO HACER:**

1. **NO intentar hacer login de admin en el portal de clientes**
   - El portal Next.js (`firmeza-client`) NO debe tener acceso de administradores
   - Los admins solo deben usar `Firmeza.Web`

2. **NO mezclar los sistemas de autenticación**
   - Identity (Cookies) es para `Firmeza.Web`
   - JWT es para `firmeza-client` + `ApiFirmeza.Web`

3. **NO usar el mismo puerto para API y portal Razor**
   - La API debe estar en un puerto (ej: 5000/5001)
   - El portal Razor en otro puerto (ej: 5002/5003)

4. **NO hacer que firmeza-client llame directamente a la base de datos**
   - Siempre debe pasar por la API

---

## ✅ FLUJO CORRECTO DE TRABAJO

### Para Administradores:
1. Acceder a `http://localhost:5002` (o puerto de Firmeza.Web)
2. Login con Identity en `/Identity/Account/Login`
3. Redirigir a `/Admin/Dashboard`
4. Gestionar productos, clientes, ventas desde Razor

### Para Clientes:
1. Acceder a `http://localhost:3000` (firmeza-client)
2. Login con email/password en `/login`
3. El frontend llama a `ApiFirmeza.Web` para login
4. Recibe JWT y lo guarda en localStorage
5. Navegar por el portal con el token

---

## 🔧 Configuración de Puertos

### Archivo: `ApiFirmeza.Web/Properties/launchSettings.json`
```json
"applicationUrl": "https://localhost:5001;http://localhost:5000"
```

### Archivo: `Firmeza.Web/Properties/launchSettings.json`
```json
"applicationUrl": "https://localhost:5003;http://localhost:5002"
```

### Archivo: `firmeza-client/lib/axios.ts`
```typescript
baseURL: 'http://localhost:5000/api'  // Apunta a la API
```

---

## 📝 Resumen Ejecutivo

```
┌─────────────────────────────────────────────────────────┐
│                    SISTEMA FIRMEZA                      │
└─────────────────────────────────────────────────────────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
┌───────────────┐  ┌───────────────┐  ┌──────────────┐
│ ApiFirmeza.Web│  │ Firmeza.Web   │  │firmeza-client│
│   (API REST)  │  │ (Portal Admin)│  │(Portal Cliente)
│               │  │               │  │              │
│ Puerto: 5000  │  │ Puerto: 5002  │  │ Puerto: 3000 │
│ Auth: JWT     │  │ Auth: Identity│  │ Auth: JWT    │
│               │  │               │  │              │
│ Para: Clients │  │ Para: ADMINS  │  │ Para: CLIENTES│
└───────────────┘  └───────────────┘  └──────────────┘
        ▲                                     │
        │                                     │
        └─────────────────────────────────────┘
              firmeza-client consume la API
```

---

## 🎯 Casos de Uso

### Caso 1: Soy administrador
- ✅ Uso **Firmeza.Web** en `localhost:5002`
- ✅ Login con Identity
- ✅ Gestiono todo desde el panel admin
- ❌ NO uso firmeza-client

### Caso 2: Soy cliente
- ✅ Uso **firmeza-client** en `localhost:3000`
- ✅ Login con JWT
- ✅ Veo mis datos y compras
- ❌ NO tengo acceso a Firmeza.Web

### Caso 3: Desarrollo la API
- ✅ Trabajo en **ApiFirmeza.Web**
- ✅ La consumen los clientes desde firmeza-client
- ✅ Los admins NO la usan (tienen acceso directo a BD)

---

## 🛠️ Comandos para Iniciar Todo

```bash
# Terminal 1 - API (para portal de clientes)
cd ApiFirmeza.Web
dotnet run

# Terminal 2 - Portal Admin (Razor)
cd Firmeza.Web
dotnet run

# Terminal 3 - Portal Cliente (Next.js)
cd firmeza-client
npm run dev
```

**URLs resultantes:**
- API: `http://localhost:5000`
- Portal Admin: `http://localhost:5002`
- Portal Cliente: `http://localhost:3000`

---

## 📚 Referencias

- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [JWT en ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/introduction)
- [Next.js Authentication](https://nextjs.org/docs/authentication)

---

**Fecha de actualización:** 2025-01-26
**Versión:** 1.0

