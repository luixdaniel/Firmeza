# 🏛️ ARCHITECTURE.md - Arquitectura del Sistema Firmeza

## 📋 Visión General

Firmeza es un sistema de gestión de ventas para insumos de construcción desarrollado con arquitectura de **microservicios**, separando la API REST, el portal administrativo y el cliente web en aplicaciones independientes que se comunican entre sí.

---

## 🎯 Principios de Arquitectura

### 1. Separación de Responsabilidades (SoC)
- **API REST**: Lógica de negocio y acceso a datos
- **Portal Admin**: Interfaz administrativa
- **Cliente Web**: Interfaz de usuario final

### 2. Arquitectura en Capas
Cada proyecto sigue una arquitectura en capas:
- **Presentación** (Controllers/Pages)
- **Aplicación** (Services)
- **Dominio** (Entities/Models)
- **Infraestructura** (Repositories/DbContext)

### 3. Clean Architecture
- Dependencias apuntan hacia adentro
- El dominio no conoce la infraestructura
- Uso de interfaces para inversión de dependencias

### 4. API First
- La API es la única fuente de verdad
- Clientes consumen la API vía HTTP
- Documentación con OpenAPI/Swagger

---

## 🏗️ Arquitectura General

```
┌─────────────────────────────────────────────────────────────────┐
│                         USUARIOS                                │
├──────────────┬────────────────────┬────────────────────────────┤
│   Clientes   │   Administradores  │      Desarrolladores       │
└──────┬───────┴─────────┬──────────┴────────────┬───────────────┘
       │                 │                       │
       ▼                 ▼                       ▼
┌──────────────┐  ┌──────────────┐      ┌──────────────┐
│              │  │              │      │              │
│   Cliente    │  │    Admin     │      │   Swagger    │
│   Next.js    │  │   ASP.NET    │      │   /swagger   │
│              │  │     MVC      │      │              │
│  Port: 3000  │  │  Port: 5000  │      │  Port: 5090  │
└──────┬───────┘  └──────┬───────┘      └──────┬───────┘
       │                 │                     │
       │                 │                     │
       └─────────────────┼─────────────────────┘
                         │
                         ▼
                 ┌───────────────┐
                 │               │
                 │   API REST    │
                 │  ASP.NET Core │
                 │               │
                 │  Port: 5090   │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │               │
                 │  PostgreSQL   │
                 │   (Supabase)  │
                 │               │
                 │  Port: 5432   │
                 └───────────────┘
```

---

## 📦 Componentes del Sistema

### 1. ApiFirmeza.Web (API REST)

**Responsabilidad:** Proveer endpoints HTTP para todas las operaciones del sistema.

**Tecnologías:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger/OpenAPI

**Patrones:**
- Repository Pattern
- Service Layer Pattern
- DTO Pattern
- Dependency Injection

**Endpoints:**
- `/api/Auth` - Autenticación
- `/api/Productos` - Gestión de productos
- `/api/Categorias` - Gestión de categorías
- `/api/Clientes` - Gestión de clientes
- `/api/Ventas` - Gestión de ventas

**Puerto:** 5090 (Docker: 8080 interno)

---

### 2. Firmeza.Web (Portal Administrativo)

**Responsabilidad:** Interfaz web para administradores del sistema.

**Tecnologías:**
- ASP.NET Core 8.0 MVC
- Razor Pages
- ASP.NET Identity
- Bootstrap 5
- jQuery/DataTables

**Funcionalidades:**
- Dashboard con estadísticas
- CRUD de productos, categorías, clientes
- Gestión de ventas
- Generación de reportes
- Importación masiva de Excel

**Puerto:** 5000 (Docker: 8080 interno)

---

### 3. firmeza-client (Cliente Web)

**Responsabilidad:** Interfaz web para clientes finales.

**Tecnologías:**
- Next.js 14 (App Router)
- TypeScript
- Tailwind CSS
- Context API

**Funcionalidades:**
- Catálogo de productos
- Carrito de compras
- Checkout
- Historial de compras
- Perfil de usuario

**Puerto:** 3000

---

### 4. Firmeza.Tests (Suite de Pruebas)

**Responsabilidad:** Tests automatizados del sistema.

**Tecnologías:**
- xUnit
- Moq
- Entity Framework InMemory

**Tipos de Tests:**
- Tests unitarios
- Tests de integración
- Tests de controladores

---

### 5. Base de Datos (PostgreSQL)

**Responsabilidad:** Almacenamiento persistente de datos.

**Provider:** Supabase (PostgreSQL en la nube)

**Tablas Principales:**
- `AspNetUsers` - Usuarios del sistema
- `AspNetRoles` - Roles (Admin, Cliente)
- `Clientes` - Datos de clientes
- `Productos` - Catálogo de productos
- `Categorias` - Categorías de productos
- `Ventas` - Ventas realizadas
- `DetallesVenta` - Items de cada venta

**Puerto:** 5432

---

## 🔄 Flujo de Datos

### Flujo de Autenticación

```
┌─────────┐         ┌─────────┐         ┌──────────┐
│ Cliente │  POST   │   API   │  Query  │    DB    │
│  Web    ├────────>│  /Auth  ├────────>│   User   │
└────┬────┘  login  └────┬────┘         └────┬─────┘
     │                   │                    │
     │                   │<───────────────────┘
     │                   │  User + Roles
     │                   │
     │                   │  Generate JWT
     │                   ├──────────┐
     │                   │          │
     │                   │<─────────┘
     │<──────────────────┤
     │   200 OK + Token  │
     │                   │
     └───────────────────┘
```

### Flujo de Compra

```
┌─────────┐         ┌─────────┐         ┌──────────┐
│ Cliente │         │   API   │         │    DB    │
└────┬────┘         └────┬────┘         └────┬─────┘
     │                   │                    │
     │  POST /Ventas     │                    │
     ├──────────────────>│                    │
     │  + JWT Token      │                    │
     │                   │  Validate Token    │
     │                   │  Validate Stock    │
     │                   ├───────────────────>│
     │                   │                    │
     │                   │  Create Venta      │
     │                   ├───────────────────>│
     │                   │                    │
     │                   │  Update Stock      │
     │                   ├───────────────────>│
     │                   │                    │
     │                   │  Generate PDF      │
     │                   ├──────────┐         │
     │                   │          │         │
     │                   │<─────────┘         │
     │                   │                    │
     │                   │  Send Email        │
     │                   ├──────────────────> │
     │                   │                   (SMTP)
     │<──────────────────┤                    │
     │  201 Created      │                    │
     └───────────────────┴────────────────────┘
```

---

## 🔐 Seguridad

### Autenticación

**API REST:**
- JWT Bearer Tokens
- Secret key configurado en appsettings
- Expiración configurable (default: 120 minutos)

**Portal Admin:**
- ASP.NET Identity
- Cookies de autenticación
- Role-based authorization

### Autorización

**Roles:**
- `Admin`: Acceso completo
- `Cliente`: Acceso limitado a sus datos

**Políticas:**
```csharp
[Authorize(Roles = "Admin")]
public class ProductosController : ControllerBase
{
    // Solo admins pueden gestionar productos
}

[Authorize]
public async Task<IActionResult> MisCompras()
{
    // Solo usuarios autenticados
    // Acceso a sus propias compras
}
```

### CORS

**Configurado en la API:**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins(
            "http://localhost:3000",  // Cliente
            "http://localhost:5000"   // Admin
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
```

---

## 💾 Base de Datos

### Esquema de Datos

```sql
-- Usuarios y Roles (ASP.NET Identity)
AspNetUsers (Id, Email, PasswordHash, ...)
AspNetRoles (Id, Name)
AspNetUserRoles (UserId, RoleId)

-- Dominio del Negocio
Clientes (
    Id PK,
    UserId FK -> AspNetUsers,
    Nombre,
    Apellido,
    Email UNIQUE,
    Telefono,
    Direccion,
    Ciudad,
    CodigoPostal
)

Categorias (
    Id PK,
    Nombre UNIQUE,
    Descripcion,
    Activo
)

Productos (
    Id PK,
    Nombre,
    Descripcion,
    Precio,
    Stock,
    CategoriaId FK -> Categorias,
    Activo
)

Ventas (
    Id PK,
    ClienteId FK -> Clientes,
    Fecha,
    Total,
    MetodoPago,
    Vendedor
)

DetallesVenta (
    Id PK,
    VentaId FK -> Ventas,
    ProductoId FK -> Productos,
    Cantidad,
    PrecioUnitario,
    Subtotal
)
```

### Migraciones

**Entity Framework Core Migrations:**
```bash
# Crear migración
dotnet ef migrations add MigrationName

# Aplicar migraciones
dotnet ef database update

# Script SQL
dotnet ef migrations script
```

---

## 🐳 Infraestructura Docker

### docker-compose.yml

```yaml
services:
  # Tests (se ejecutan primero)
  tests:
    build: Firmeza.Tests/
    depends_on: []
    
  # API REST
  api:
    build: ApiFirmeza.Web/
    ports: ["5090:8080"]
    depends_on: [tests]
    environment:
      - ConnectionStrings__DefaultConnection
      - JwtSettings__SecretKey
      
  # Portal Admin
  admin:
    build: Firmeza.Web/
    ports: ["5000:8080"]
    depends_on: [tests]
    environment:
      - ConnectionStrings__DefaultConnection
      
  # Cliente Web
  client:
    build: firmeza-client/
    ports: ["3000:3000"]
    depends_on: [api]
    environment:
      - NEXT_PUBLIC_API_URL=http://localhost:5090
```

### Networking

Todos los servicios están en la misma red Docker:
```yaml
networks:
  firmeza-network:
    driver: bridge
```

Esto permite comunicación entre contenedores usando nombres de servicio:
- `http://api:8080`
- `http://admin:8080`
- `http://client:3000`

---

## 📊 Patrones de Diseño

### Repository Pattern

**Ventajas:**
- Abstracción del acceso a datos
- Facilita testing (mocks)
- Centraliza queries

**Implementación:**
```csharp
public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task<Producto> GetByIdAsync(int id);
    Task<Producto> CreateAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(int id);
}

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;
    
    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // Implementación...
}
```

### Service Layer Pattern

**Ventajas:**
- Lógica de negocio separada de controladores
- Reutilización de código
- Facilita testing

**Implementación:**
```csharp
public interface IVentaService
{
    Task<Venta> CreateVentaAsync(CreateVentaDto dto);
    Task<byte[]> GenerarReciboPdfAsync(int ventaId);
}

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepo;
    private readonly IProductoRepository _productoRepo;
    private readonly IEmailService _emailService;
    
    // Implementación con validaciones de negocio
}
```

### DTO Pattern

**Ventajas:**
- Separación entre entidades y datos transferidos
- Control sobre qué se expone
- Validaciones específicas por operación

**Implementación:**
```csharp
public class CreateProductoDto
{
    [Required]
    public string Nombre { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}

// AutoMapper para conversión
CreateMap<CreateProductoDto, Producto>();
```

---

## 🔄 Ciclo de Vida de una Request

### 1. Cliente hace petición
```typescript
// firmeza-client
const response = await fetch('http://localhost:5090/api/Productos', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

### 2. Middleware de ASP.NET Core
- **Authentication**: Valida JWT token
- **Authorization**: Verifica roles/claims
- **CORS**: Valida origen
- **Routing**: Determina controlador y acción

### 3. Controller recibe request
```csharp
[HttpGet]
[Authorize(Roles = "Admin,Cliente")]
public async Task<IActionResult> GetAll()
{
    var productos = await _service.GetAllAsync();
    return Ok(productos);
}
```

### 4. Service ejecuta lógica
```csharp
public async Task<IEnumerable<Producto>> GetAllAsync()
{
    return await _repository.GetActivosAsync();
}
```

### 5. Repository consulta BD
```csharp
public async Task<IEnumerable<Producto>> GetActivosAsync()
{
    return await _context.Productos
        .Where(p => p.Activo)
        .Include(p => p.Categoria)
        .ToListAsync();
}
```

### 6. Response al cliente
```csharp
return Ok(productos); // 200 OK con JSON
```

---

## 📈 Escalabilidad

### Horizontal Scaling

**API REST:**
- Stateless (JWT, no sesiones)
- Puede replicarse detrás de un load balancer
- `docker-compose scale api=3`

**Portal Admin:**
- Sesiones en cookies (puede usar Redis para sesiones distribuidas)
- Puede replicarse con sesión compartida

**Cliente:**
- Completamente stateless
- Fácil de escalar

### Vertical Scaling

- Incrementar recursos de contenedores
- Configurar límites en docker-compose:
```yaml
deploy:
  resources:
    limits:
      cpus: '2'
      memory: 2G
```

### Caching (Futuro)

- Redis para cachear productos
- Output caching en ASP.NET Core
- CDN para assets estáticos

---

## 🔍 Monitoreo y Logs

### Logs Estructurados

**Configuración:**
```csharp
builder.Logging.AddConsole();
builder.Logging.AddDebug();
```

**Uso:**
```csharp
_logger.LogInformation("Venta creada: {VentaId}", venta.Id);
_logger.LogError(ex, "Error al procesar venta");
```

### Health Checks

**Endpoints:**
- `GET /health` - Estado general
- `GET /health/ready` - Listo para recibir tráfico (futuro)
- `GET /health/live` - Aplicación viva (futuro)

### Métricas (Futuro)

- Prometheus para métricas
- Grafana para visualización
- Application Insights para Azure

---

## 🚀 Deployment

### Desarrollo Local
```bash
docker-compose up --build
```

### Producción

**Opciones:**
1. **Azure Container Instances**
2. **AWS ECS/Fargate**
3. **Google Cloud Run**
4. **Kubernetes** (para mayor escala)
5. **VPS tradicional** con Docker Compose

**Consideraciones:**
- Variables de entorno en secretos
- Base de datos en servicio administrado
- CDN para assets estáticos
- SSL/TLS con Let's Encrypt
- Backup automático de BD

---

## 📚 Referencias

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Next.js Documentation](https://nextjs.org/docs)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Docker Documentation](https://docs.docker.com/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 📝 Notas de Arquitectura

### Decisiones Técnicas

**¿Por qué separar API y Admin?**
- Separación de responsabilidades
- Independencia de despliegue
- Admin puede usar Identity, API usa JWT
- Diferentes ciclos de actualización

**¿Por qué Next.js para el cliente?**
- SSR/SSG para mejor SEO
- React moderno con TypeScript
- Excelente DX (Developer Experience)
- Deployable como contenedor

**¿Por qué PostgreSQL?**
- Open source y robusto
- Excelente integración con EF Core
- Supabase ofrece hosting gratuito
- Escalable y confiable

---

Este documento evoluciona con el proyecto. Última actualización: 2025-12-01

