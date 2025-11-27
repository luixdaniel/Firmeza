# 🚀 Guía de Ejecución - Portales Separados

## 📋 Tabla de Contenidos
1. [Introducción](#introducción)
2. [Pre-requisitos](#pre-requisitos)
3. [Portal de Administración](#portal-de-administración)
4. [Portal de Clientes](#portal-de-clientes)
5. [API REST](#api-rest)
6. [Flujo de Trabajo](#flujo-de-trabajo)

---

## Introducción

Firmeza tiene **3 componentes independientes** que se ejecutan en **diferentes puertos**:

| Componente | Puerto | Usuarios | Tecnología |
|------------|--------|----------|------------|
| Portal Admin | 5002 | Administradores | ASP.NET Core MVC + Razor |
| Portal Cliente | 3000 | Clientes | Next.js 14 |
| API REST | 5000 | Backend | ASP.NET Core Web API |

---

## Pre-requisitos

### Software Necesario:
- ✅ .NET 8.0 SDK
- ✅ Node.js 18+ y npm
- ✅ PostgreSQL (o base de datos configurada)
- ✅ Editor de código (VS Code, Rider, etc.)

### Verificar Instalaciones:
```bash
# Verificar .NET
dotnet --version

# Verificar Node.js
node --version

# Verificar npm
npm --version
```

---

## Portal de Administración

### 🔵 Firmeza.Web - Puerto 5002

Este es el portal **SOLO para administradores** con interfaz Razor.

### 1. Configurar Base de Datos

Editar archivo de secretos:
```bash
cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=firmeza_db;Username=tu_usuario;Password=tu_password"
```

O editar `appsettings.json` (no recomendado para producción):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=firmeza_db;Username=postgres;Password=tu_password"
  }
}
```

### 2. Aplicar Migraciones

```bash
cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
dotnet ef database update
```

### 3. Ejecutar el Portal Admin

```bash
cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
dotnet run
```

### 4. Acceder al Portal

Abrir navegador en:
```
http://localhost:5002
```

### 5. Credenciales de Administrador

```
Email: admin@firmeza.com
Password: Admin123$
```

### ✅ ¿Qué Puedes Hacer?
- Gestionar clientes
- Gestionar productos
- Gestionar ventas
- Gestionar categorías
- Ver dashboard administrativo
- Crear/editar/eliminar registros

### ❌ ¿Qué NO Puedes Hacer?
- NO es para clientes finales
- NO usa JWT (usa Identity con cookies)

---

## Portal de Clientes

### 🟢 firmeza-client - Puerto 3000

Este es el portal **SOLO para clientes** con interfaz Next.js.

### 1. Instalar Dependencias

```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm install
```

### 2. Configurar Variables de Entorno (Opcional)

Crear archivo `.env.local` (si no existe):
```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

### 3. Ejecutar el Portal de Clientes

```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

### 4. Acceder al Portal

Abrir navegador en:
```
http://localhost:3000
```

### 5. Credenciales de Cliente

```
Email: cliente@firmeza.com
Password: Cliente123$
```

### ✅ ¿Qué Puedes Hacer?
- Ver catálogo de productos
- Agregar productos al carrito
- Realizar compras
- Ver historial de pedidos
- Gestionar perfil personal

### ❌ ¿Qué NO Puedes Hacer?
- NO es para administradores
- NO tiene funciones de gestión administrativa
- NO puede crear/editar productos

---

## API REST

### 🟡 ApiFirmeza.Web - Puerto 5000

Este es el **backend API** que consume el portal de clientes.

### 1. Configurar Base de Datos

Editar archivo de secretos:
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=firmeza_db;Username=tu_usuario;Password=tu_password"
dotnet user-secrets set "JwtSettings:SecretKey" "tu_clave_secreta_muy_larga_y_segura_minimo_32_caracteres"
```

### 2. Ejecutar la API

```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

### 3. Acceder a Swagger (Documentación)

Abrir navegador en:
```
http://localhost:5000/swagger
```

### ✅ ¿Qué Puedes Hacer?
- Ver todos los endpoints disponibles
- Probar endpoints directamente
- Ver modelos de datos
- Autenticarse con JWT

---

## Flujo de Trabajo

### Escenario 1: Administrador Gestiona el Sistema

1. **Iniciar Portal Admin**
   ```bash
   cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
   dotnet run
   ```

2. **Acceder a http://localhost:5002**

3. **Iniciar sesión con credenciales de admin**
   ```
   admin@firmeza.com / Admin123$
   ```

4. **Gestionar productos, clientes, ventas, etc.**

---

### Escenario 2: Cliente Realiza una Compra

1. **Iniciar API REST**
   ```bash
   cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
   dotnet run
   ```

2. **Iniciar Portal de Clientes**
   ```bash
   cd /home/Coder/Escritorio/Firmeza/firmeza-client
   npm run dev
   ```

3. **Acceder a http://localhost:3000**

4. **Iniciar sesión con credenciales de cliente**
   ```
   cliente@firmeza.com / Cliente123$
   ```

5. **Navegar por productos y realizar compras**

---

### Escenario 3: Desarrollo Full Stack

Para desarrollo, puedes ejecutar los 3 componentes simultáneamente:

**Terminal 1 - API:**
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

**Terminal 2 - Portal Admin:**
```bash
cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
dotnet run --urls "http://localhost:5002;https://localhost:5003"
```

**Terminal 3 - Portal Cliente:**
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

**Acceder a:**
- API: http://localhost:5000/swagger
- Admin: http://localhost:5002
- Cliente: http://localhost:3000

---

## 🔒 Tabla de Separación

| Característica | Portal Admin | Portal Cliente | API |
|----------------|--------------|----------------|-----|
| **Puerto** | 5002 | 3000 | 5000 |
| **Tecnología** | Razor Pages | Next.js | Web API |
| **Autenticación** | Identity (Cookies) | JWT | JWT |
| **Usuarios** | Administradores | Clientes | Backend |
| **Gestión CRUD** | ✅ Sí | ❌ No | ✅ Sí |
| **Compras** | ❌ No | ✅ Sí | - |
| **Dashboard Admin** | ✅ Sí | ❌ No | - |
| **Catálogo Productos** | ✅ Editar | ✅ Ver | ✅ API |

---

## 🚨 Errores Comunes

### Error: Puerto en uso
```
Error: Address already in use
```
**Solución:** Cambiar el puerto o detener el proceso que lo está usando.

### Error: No se puede conectar a la base de datos
```
Error: Connection refused
```
**Solución:** Verificar que PostgreSQL esté corriendo y las credenciales sean correctas.

### Error: JWT Secret no configurado
```
Error: Secret key not configured
```
**Solución:** Configurar la clave secreta en secrets.json o appsettings.

### Error: CORS en API
```
Error: CORS policy blocked
```
**Solución:** Verificar que la API tenga configurado CORS para http://localhost:3000

---

## 📝 Scripts Útiles

### Script para iniciar todo (Linux/Mac):

Crear archivo `iniciar-todo.sh`:
```bash
#!/bin/bash

# Iniciar API en background
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run &
API_PID=$!

# Iniciar Portal Admin en background
cd /home/Coder/Escritorio/Firmeza/Firmeza.Web
dotnet run --urls "http://localhost:5002;https://localhost:5003" &
ADMIN_PID=$!

# Iniciar Portal Cliente
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev

# Al cerrar, matar los procesos
kill $API_PID $ADMIN_PID
```

Dar permisos:
```bash
chmod +x iniciar-todo.sh
```

Ejecutar:
```bash
./iniciar-todo.sh
```

---

## ✅ Checklist de Inicio

Antes de empezar a trabajar:

- [ ] PostgreSQL está corriendo
- [ ] Cadenas de conexión configuradas
- [ ] Migraciones aplicadas
- [ ] JWT Secret configurado (para API)
- [ ] Dependencias de Node instaladas (para cliente)
- [ ] Puertos 3000, 5000 y 5002 disponibles

---

**Fecha**: 2025-11-26  
**Versión**: 1.0

