# 🚀 Sistema Firmeza - Gestión de Clientes y Ventas

Sistema completo de gestión con API REST en .NET 9, Frontend en Next.js 14, y base de datos PostgreSQL.

---

## ✅ ESTADO ACTUAL: **COMPLETAMENTE OPERATIVO**

| Componente | Estado | Puerto | URL |
|------------|--------|--------|-----|
| **API Backend** | ✅ Corriendo | 5090 | http://localhost:5090 |
| **Swagger UI** | ✅ Disponible | 5090 | http://localhost:5090/swagger |
| **Frontend** | ✅ Corriendo | 3000 | http://localhost:3000 |
| **Base de Datos** | ✅ Conectada | 5432 | PostgreSQL/Supabase |

---

## 🎯 INICIO RÁPIDO

### Opción 1: Usar Script Automático (Recomendado)

```cmd
INICIAR_TODO.bat
```

Este script iniciará automáticamente la API y el Frontend en ventanas separadas.

### Opción 2: Inicio Manual

**Terminal 1 - API:**
```cmd
cd ApiFirmeza.Web
dotnet run
```

**Terminal 2 - Frontend:**
```cmd
cd firmeza-client
npm run dev
```

### Detener Todo

```cmd
DETENER_TODO.bat
```

---

## 🌐 URLs DEL SISTEMA

### Frontend (Cliente)
- **Inicio:** http://localhost:3000
- **Login:** http://localhost:3000/auth/login
- **Registro:** http://localhost:3000/auth/register
- **Tienda:** http://localhost:3000/clientes/tienda

### Frontend (Admin)
- **Dashboard:** http://localhost:3000/admin/dashboard
- **Clientes:** http://localhost:3000/admin/clientes
- **Productos:** http://localhost:3000/admin/productos
- **Ventas:** http://localhost:3000/admin/ventas

### API
- **Base URL:** http://localhost:5090/api
- **Swagger:** http://localhost:5090/swagger
- **Health:** http://localhost:5090/api/health

---

## 🔐 CREDENCIALES

### Administrador
```
Email: admin@firmeza.com
Contraseña: Admin123$
```

### Cliente de Prueba (Créalo en el registro)
Navega a: http://localhost:3000/auth/register

---

## 📁 ESTRUCTURA DEL PROYECTO

```
Firmeza/
├── ApiFirmeza.Web/              # API Backend (.NET 9)
│   ├── Controllers/             # Controladores REST
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Properties/              # Configuración (launchSettings.json)
│   └── Program.cs               # Punto de entrada
│
├── firmeza-client/              # Frontend (Next.js 14)
│   ├── app/                     # Rutas y páginas
│   ├── components/              # Componentes React
│   ├── services/                # Servicios API
│   ├── types/                   # Tipos TypeScript
│   └── .env.local               # Variables de entorno
│
├── Firmeza.Web/                 # Aplicación MVC (legacy)
│
├── INICIAR_TODO.bat            # ✅ Script para iniciar todo
├── DETENER_TODO.bat            # ⛔ Script para detener todo
└── SISTEMA_LISTO_PARA_USAR.md  # 📖 Guía completa
```

---

## 🛠️ TECNOLOGÍAS

### Backend
- **.NET 9** - Framework principal
- **ASP.NET Core** - API REST
- **Entity Framework Core** - ORM
- **PostgreSQL** - Base de datos
- **Identity** - Autenticación y autorización
- **JWT** - Tokens de autenticación
- **Swagger/OpenAPI** - Documentación de API
- **AutoMapper** - Mapeo de objetos

### Frontend
- **Next.js 14** - Framework React
- **TypeScript** - Tipado estático
- **Tailwind CSS** - Estilos
- **Axios** - Cliente HTTP
- **React Hooks** - Gestión de estado

---

## 📡 API ENDPOINTS

### Autenticación (Sin autenticación requerida)
- `POST /api/Auth/register` - Registrar cliente
- `POST /api/Auth/login` - Iniciar sesión

### Categorías (Público)
- `GET /api/Categorias` - Listar categorías
- `GET /api/Categorias/{id}` - Ver categoría

### Productos (Público para lectura)
- `GET /api/Productos` - Listar productos
- `GET /api/Productos/{id}` - Ver producto
- `POST /api/Productos` - Crear producto (Admin)
- `PUT /api/Productos/{id}` - Actualizar producto (Admin)
- `DELETE /api/Productos/{id}` - Eliminar producto (Admin)

### Clientes (Autenticación requerida)
- `GET /api/Clientes` - Listar clientes (Admin)
- `GET /api/Clientes/{id}` - Ver cliente
- `PUT /api/Clientes/{id}` - Actualizar cliente
- `DELETE /api/Clientes/{id}` - Eliminar cliente (Admin)

### Ventas (Autenticación requerida)
- `GET /api/Ventas` - Listar ventas
- `GET /api/Ventas/{id}` - Ver venta
- `POST /api/Ventas` - Crear venta
- `GET /api/Ventas/{id}/pdf` - Generar PDF (Admin)

---

## 🧪 PROBAR EL SISTEMA

### 1. Registrar un Cliente
1. Ve a: http://localhost:3000/auth/register
2. Completa el formulario:
   ```
   Nombre: Juan
   Apellido: Pérez
   Email: juan.perez@test.com
   Teléfono: 3001234567
   Contraseña: Test123$
   ```
3. Click en "Registrar"

### 2. Hacer Login
1. Ve a: http://localhost:3000/auth/login
2. Ingresa las credenciales
3. Serás redirigido según tu rol (Cliente o Admin)

### 3. Ver Productos (Cliente)
1. Navega a: http://localhost:3000/clientes/tienda
2. Explora los productos disponibles

### 4. Panel de Administración
1. Login como admin (admin@firmeza.com / Admin123$)
2. Ve a: http://localhost:3000/admin/dashboard
3. Gestiona clientes, productos, categorías y ventas

### 5. Probar con Swagger
1. Abre: http://localhost:5090/swagger
2. Haz login en `/api/Auth/login`
3. Copia el token
4. Click en "Authorize" 🔒
5. Ingresa: `Bearer [tu-token]`
6. Prueba cualquier endpoint

---

## 🔧 CONFIGURACIÓN

### Variables de Entorno

#### API (User Secrets)
```json
{
  "ConnectionStrings:DefaultConnection": "Host=...;Database=...;Username=...;Password=...",
  "JwtSettings:SecretKey": "...",
  "JwtSettings:Issuer": "FirmezaAPI",
  "JwtSettings:Audience": "FirmezaClients",
  "JwtSettings:ExpirationMinutes": 120
}
```

Ver secrets actuales:
```cmd
cd ApiFirmeza.Web
dotnet user-secrets list
```

#### Frontend (.env.local)
```env
NEXT_PUBLIC_API_URL=http://localhost:5090
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### La API no inicia
1. Verifica que el puerto 5090 esté libre:
   ```cmd
   netstat -ano | findstr ":5090"
   ```
2. Verifica los secrets:
   ```cmd
   dotnet user-secrets list --project ApiFirmeza.Web\ApiFirmeza.Web.csproj
   ```
3. Reinicia la API

### El Frontend no conecta a la API
1. Verifica que `.env.local` existe y tiene `NEXT_PUBLIC_API_URL=http://localhost:5090`
2. Reinicia el frontend (las variables de entorno se leen al iniciar)
3. Limpia caché del navegador (Ctrl+Shift+R)

### Error 401 Unauthorized
- El token expiró (dura 2 horas)
- Haz logout y login nuevamente
- Verifica que el header Authorization tenga el formato: `Bearer [token]`

### Error de Base de Datos
1. Verifica la conexión a PostgreSQL
2. Verifica que los secrets tengan la cadena de conexión correcta
3. Ejecuta las migraciones si es necesario:
   ```cmd
   dotnet ef database update
   ```

---

## 📊 COMANDOS ÚTILES

### Ver Procesos Corriendo
```powershell
# Ver todos los procesos de .NET y Node
Get-Process | Where-Object {$_.ProcessName -match "node|dotnet"}

# Ver puertos en uso
netstat -ano | findstr "3000 5090"
```

### Detener Procesos Manualmente
```powershell
# Detener API
Get-Process -Name dotnet | Stop-Process -Force

# Detener Frontend
Get-Process -Name node | Stop-Process -Force
```

### Limpiar y Reconstruir
```cmd
# API
cd ApiFirmeza.Web
dotnet clean
dotnet build

# Frontend
cd firmeza-client
npm run build
```

---

## 📚 DOCUMENTACIÓN ADICIONAL

- **[SISTEMA_LISTO_PARA_USAR.md](SISTEMA_LISTO_PARA_USAR.md)** - Guía completa de uso
- **[CONFIGURACION_PUERTOS_CORRECTA.md](CONFIGURACION_PUERTOS_CORRECTA.md)** - Configuración de puertos
- **[ESTADO_ACTUAL_SISTEMA.md](ESTADO_ACTUAL_SISTEMA.md)** - Estado del sistema

---

## ✅ CHECKLIST DE VERIFICACIÓN

- [x] API corriendo en puerto 5090
- [x] Frontend corriendo en puerto 3000
- [x] Base de datos conectada
- [x] CORS habilitado
- [x] JWT configurado
- [x] Swagger disponible
- [x] Variables de entorno configuradas
- [x] Scripts de inicio creados

---

## 👥 ROLES Y PERMISOS

### Cliente
- ✅ Ver productos
- ✅ Ver su perfil
- ✅ Ver su historial de compras
- ✅ Realizar compras
- ❌ Gestionar otros clientes
- ❌ Gestionar productos
- ❌ Ver ventas de otros

### Admin
- ✅ Todo lo que puede hacer un Cliente
- ✅ Gestionar clientes (CRUD)
- ✅ Gestionar productos (CRUD)
- ✅ Gestionar categorías (CRUD)
- ✅ Ver todas las ventas
- ✅ Generar reportes PDF
- ✅ Registrar otros administradores

---

## 🎉 ¡SISTEMA LISTO!

El sistema está completamente configurado y operativo. 

**Para comenzar:**
1. Ejecuta `INICIAR_TODO.bat` (o inicia manualmente)
2. Abre http://localhost:3000
3. Registra un cliente o inicia sesión como admin
4. ¡Comienza a usar el sistema!

**¿Problemas?** Revisa la sección de Solución de Problemas o la documentación en `SISTEMA_LISTO_PARA_USAR.md`.

---

**Desarrollado con ❤️ usando .NET 9 + Next.js 14**

