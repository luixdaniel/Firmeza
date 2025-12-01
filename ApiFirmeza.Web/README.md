# 🔌 ApiFirmeza.Web - API REST

## 📋 Descripción

API REST desarrollada en **ASP.NET Core 8.0** para el sistema Firmeza. Proporciona endpoints para la gestión de productos, categorías, clientes, ventas y autenticación.

---

## 🏗️ Tecnologías

- **Framework:** ASP.NET Core 8.0 Web API
- **ORM:** Entity Framework Core 8.0
- **Base de Datos:** PostgreSQL (Supabase)
- **Autenticación:** JWT Bearer Tokens
- **Documentación:** Swagger/OpenAPI
- **Mapeo:** AutoMapper
- **Email:** MailKit + MimeKit
- **PDF:** iTextSharp

---

## 📁 Estructura del Proyecto

```
ApiFirmeza.Web/
├── Controllers/           # Controladores de la API
│   ├── AuthController.cs           # Autenticación y registro
│   ├── CategoriasController.cs     # CRUD de categorías
│   ├── ClientesController.cs       # Gestión de clientes
│   ├── ProductosController.cs      # CRUD de productos
│   └── VentasController.cs         # Gestión de ventas y compras
├── DTOs/                  # Data Transfer Objects
│   ├── Auth/                       # DTOs de autenticación
│   ├── Categoria/                  # DTOs de categorías
│   ├── Cliente/                    # DTOs de clientes
│   ├── Producto/                   # DTOs de productos
│   └── Venta/                      # DTOs de ventas
├── Mappings/              # Perfiles de AutoMapper
│   └── MappingProfile.cs
├── Services/              # Servicios de negocio
│   └── EmailService.cs             # Envío de emails
├── appsettings.json       # Configuración
├── Program.cs             # Configuración de la aplicación
└── Dockerfile             # Contenedor Docker
```

---

## 🚀 Endpoints Principales

### 🔐 Autenticación (`/api/Auth`)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| POST | `/register` | Registrar nuevo cliente | No |
| POST | `/login` | Iniciar sesión | No |
| POST | `/register-admin` | Registrar administrador | Sí (Admin) |

### 📦 Productos (`/api/Productos`)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/` | Listar todos los productos | No |
| GET | `/{id}` | Obtener producto por ID | No |
| GET | `/categoria/{categoriaId}` | Productos por categoría | No |
| POST | `/` | Crear producto | Sí (Admin) |
| PUT | `/{id}` | Actualizar producto | Sí (Admin) |
| DELETE | `/{id}` | Eliminar producto | Sí (Admin) |

### 🏷️ Categorías (`/api/Categorias`)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/` | Listar categorías | No |
| GET | `/{id}` | Obtener categoría | No |
| POST | `/` | Crear categoría | Sí (Admin) |
| PUT | `/{id}` | Actualizar categoría | Sí (Admin) |
| DELETE | `/{id}` | Eliminar categoría | Sí (Admin) |

### 👥 Clientes (`/api/Clientes`)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/` | Listar clientes | Sí (Admin) |
| GET | `/{id}` | Obtener cliente | Sí (Admin) |
| GET | `/email/{email}` | Buscar por email | Sí (Admin) |
| PUT | `/{id}` | Actualizar cliente | Sí (Admin/Owner) |
| DELETE | `/{id}` | Eliminar cliente | Sí (Admin) |

### 💰 Ventas (`/api/Ventas`)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| GET | `/` | Listar todas las ventas | Sí (Admin) |
| GET | `/{id}` | Obtener venta | Sí (Admin/Owner) |
| GET | `/mis-compras` | Compras del cliente | Sí (Cliente) |
| POST | `/` | Crear venta | Sí (Cliente) |
| GET | `/{id}/recibo` | Descargar recibo PDF | Sí (Admin/Owner) |

---

## 🔑 Autenticación JWT

### Obtener Token

**Endpoint:** `POST /api/Auth/login`

**Request:**
```json
{
  "email": "admin@firmeza.com",
  "password": "Admin123$"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@firmeza.com",
  "nombre": "Admin",
  "apellido": "Sistema",
  "rol": "Admin"
}
```

### Usar Token

Incluir en el header de las peticiones:
```
Authorization: Bearer {token}
```

---

## 🔧 Configuración

### Variables de Entorno

```env
# Base de Datos
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=firmeza;Username=postgres;Password=password

# JWT
JwtSettings__SecretKey=tu_clave_secreta_muy_larga_y_segura
JwtSettings__Issuer=FirmezaAPI
JwtSettings__Audience=FirmezaClients
JwtSettings__ExpirationMinutes=120

# Email
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SenderEmail=tu-email@gmail.com
EmailSettings__SenderPassword=tu-password-de-app
EmailSettings__SenderName=Firmeza - Tienda

# CORS
CORS__AllowedOrigins__0=http://localhost:3000
CORS__AllowedOrigins__1=http://localhost:5000
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=firmeza;..."
  },
  "JwtSettings": {
    "SecretKey": "MiClaveSecreta...",
    "Issuer": "FirmezaAPI",
    "Audience": "FirmezaClients",
    "ExpirationMinutes": 120
  },
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "email@example.com",
    "SenderPassword": "password",
    "SenderName": "Firmeza"
  }
}
```

---

## 🐳 Docker

### Build

Desde la **raíz de la solución**:

```bash
docker build -f ApiFirmeza.Web/Dockerfile -t firmeza-api .
```

### Run

```bash
docker run -d \
  -p 5090:8080 \
  -e ConnectionStrings__DefaultConnection="Host=..." \
  -e JwtSettings__SecretKey="..." \
  --name firmeza-api \
  firmeza-api
```

### Con Docker Compose

```bash
docker-compose up -d api
```

---

## 🧪 Testing

### Swagger UI

Disponible en: **http://localhost:5090/swagger**

### Health Check

```bash
curl http://localhost:5090/health
```

**Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-12-01T18:00:00Z",
  "environment": "Production"
}
```

### Probar Autenticación

```bash
# Login
curl -X POST http://localhost:5090/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@firmeza.com","password":"Admin123$"}'

# Usar token en peticiones
curl -X GET http://localhost:5090/api/Productos \
  -H "Authorization: Bearer {token}"
```

---

## 📊 Modelos de Datos

### Cliente
```csharp
{
  "id": 1,
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan@example.com",
  "telefono": "3001234567",
  "direccion": "Calle 123",
  "ciudad": "Bogotá",
  "codigoPostal": "110111"
}
```

### Producto
```csharp
{
  "id": 1,
  "nombre": "Cemento Argos 50kg",
  "descripcion": "Cemento gris uso general",
  "precio": 32500.00,
  "stock": 100,
  "categoriaId": 1,
  "categoria": { "id": 1, "nombre": "Cemento" },
  "activo": true
}
```

### Venta
```csharp
{
  "id": 1,
  "clienteId": 1,
  "fecha": "2025-12-01T10:30:00",
  "total": 65000.00,
  "metodoPago": "Tarjeta",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 32500.00,
      "subtotal": 65000.00
    }
  ]
}
```

---

## 🔒 Roles y Permisos

### Roles Disponibles
- **Admin**: Acceso completo a todos los endpoints
- **Cliente**: Acceso a compras propias y catálogo

### Políticas de Autorización
- Endpoints públicos: Catálogo de productos y categorías
- Endpoints de cliente: Mis compras, crear ventas
- Endpoints de admin: Gestión completa (CRUD)

---

## 📧 Funcionalidades

### Envío de Emails
- ✅ Confirmación de registro
- ✅ Notificación de compra con recibo adjunto
- ✅ Recuperación de contraseña (próximamente)

### Generación de PDFs
- ✅ Recibos de venta con detalles
- ✅ Logo y formato profesional
- ✅ Almacenamiento en `/app/recibos`

### Validaciones
- ✅ Stock de productos
- ✅ Duplicación de emails
- ✅ Formato de datos (DataAnnotations)

---

## 🚧 Roadmap

- [ ] Recuperación de contraseña
- [ ] Cambio de contraseña
- [ ] Upload de imágenes de productos
- [ ] Filtros avanzados de búsqueda
- [ ] Paginación en listados
- [ ] Rate limiting
- [ ] Logs estructurados (Serilog)

---

## 📝 Logs

Los logs se escriben en la consola y pueden verse con:

```bash
docker logs firmeza-api -f
```

---

## 🤝 Contribuir

Ver [CONTRIBUTING.md](../CONTRIBUTING.md) en la raíz del proyecto.

---

## 📄 Licencia

Ver [LICENSE](../LICENSE) en la raíz del proyecto.

