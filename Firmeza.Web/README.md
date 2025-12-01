# 🏢 Firmeza.Web - Portal Administrativo

## 📋 Descripción

Portal web administrativo desarrollado en **ASP.NET Core 8.0 MVC** con **Razor Pages**. Proporciona una interfaz completa para la gestión de productos, categorías, clientes, ventas y usuarios del sistema Firmeza.

---

## 🏗️ Tecnologías

- **Framework:** ASP.NET Core 8.0 MVC
- **UI:** Razor Pages + Bootstrap 5
- **ORM:** Entity Framework Core 8.0
- **Base de Datos:** PostgreSQL (Supabase)
- **Autenticación:** ASP.NET Core Identity
- **Autorización:** Role-based (Admin, Cliente)
- **PDF:** iTextSharp
- **Excel:** EPPlus (para importación masiva)

---

## 📁 Estructura del Proyecto

```
Firmeza.Web/
├── Areas/
│   └── Admin/                 # Área administrativa
│       ├── Controllers/       # Controladores del admin
│       │   ├── ClientesController.cs
│       │   ├── ProductosController.cs
│       │   ├── CategoriasController.cs
│       │   ├── VentasController.cs
│       │   └── DashboardController.cs
│       └── Views/             # Vistas Razor del admin
│           ├── Clientes/
│           ├── Productos/
│           ├── Categorias/
│           ├── Ventas/
│           └── Dashboard/
├── Controllers/               # Controladores públicos
│   ├── HomeController.cs      # Página principal
│   └── ErrorController.cs     # Manejo de errores
├── Data/                      # Capa de datos
│   ├── AppDbContext.cs        # Contexto de Entity Framework
│   ├── Entities/              # Modelos de datos
│   │   ├── Cliente.cs
│   │   ├── Producto.cs
│   │   ├── Categoria.cs
│   │   ├── Venta.cs
│   │   └── DetalleVenta.cs
│   └── Seed/                  # Datos iniciales
│       └── SeedData.cs
├── Identity/                  # Identidad de usuarios
│   └── ApplicationUser.cs     # Usuario extendido
├── Interfaces/                # Contratos
│   ├── Repositories/          # Interfaces de repositorios
│   └── Services/              # Interfaces de servicios
├── Repositories/              # Implementación de repositorios
│   ├── ClienteRepository.cs
│   ├── ProductoRepository.cs
│   ├── CategoriaRepository.cs
│   └── VentaRepository.cs
├── Services/                  # Servicios de negocio
│   ├── ProductoService.cs
│   ├── VentaService.cs
│   ├── ImportacionMasivaService.cs
│   └── PdfService.cs
├── Views/                     # Vistas Razor públicas
│   ├── Home/
│   ├── Shared/                # Layouts y parciales
│   └── _ViewStart.cshtml
├── wwwroot/                   # Archivos estáticos
│   ├── css/
│   ├── js/
│   ├── lib/                   # Librerías (Bootstrap, jQuery)
│   └── images/
├── appsettings.json           # Configuración
├── Program.cs                 # Configuración de la aplicación
└── Dockerfile                 # Contenedor Docker
```

---

## 🎨 Funcionalidades

### 📊 Dashboard
- Vista general de estadísticas
- Ventas recientes
- Productos más vendidos
- Alertas de stock bajo

### 👥 Gestión de Clientes
- ✅ Listar clientes con búsqueda y filtros
- ✅ Ver detalles del cliente
- ✅ Editar información del cliente
- ✅ Ver historial de compras
- ✅ Eliminar clientes
- ✅ Exportar a Excel

### 📦 Gestión de Productos
- ✅ CRUD completo de productos
- ✅ Asignación de categorías
- ✅ Control de stock
- ✅ Activar/Desactivar productos
- ✅ Importación masiva desde Excel
- ✅ Búsqueda y filtros avanzados
- ✅ Imágenes de productos (próximamente)

### 🏷️ Gestión de Categorías
- ✅ CRUD de categorías
- ✅ Activar/Desactivar categorías
- ✅ Ver productos por categoría

### 💰 Gestión de Ventas
- ✅ Registro de ventas manuales
- ✅ Listado de todas las ventas
- ✅ Detalles de venta con productos
- ✅ Generación de recibos PDF
- ✅ Búsqueda por cliente, fecha, método de pago
- ✅ Reporte de ventas (próximamente)

### 🔐 Gestión de Usuarios
- ✅ Autenticación con ASP.NET Identity
- ✅ Registro de usuarios Admin
- ✅ Roles (Admin, Cliente)
- ✅ Login/Logout
- ✅ Recuperación de contraseña (próximamente)

---

## 🚀 Acceso al Portal

### URL Local
```
http://localhost:5000
```

### URL Docker
```
http://localhost:5000
```

### Credenciales por Defecto

**Administrador:**
- **Email:** `admin@firmeza.com`
- **Contraseña:** `Admin123$`

---

## 📱 Áreas del Portal

### Área Pública (`/`)
- Página de inicio
- Información de la empresa
- Contacto

### Área Administrativa (`/Admin`)
Requiere autenticación con rol **Admin**

| Ruta | Descripción |
|------|-------------|
| `/Admin/Dashboard` | Dashboard con estadísticas |
| `/Admin/Productos` | Gestión de productos |
| `/Admin/Categorias` | Gestión de categorías |
| `/Admin/Clientes` | Gestión de clientes |
| `/Admin/Ventas` | Gestión de ventas |

---

## 🔧 Configuración

### Variables de Entorno

```env
# Base de Datos
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=firmeza;Username=postgres;Password=password

# Autenticación (Opcional - OAuth)
Authentication__Google__ClientId=tu-client-id
Authentication__Google__ClientSecret=tu-secret
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=firmeza;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## 🐳 Docker

### Build

Desde la **raíz de la solución**:

```bash
docker build -f Firmeza.Web/Dockerfile -t firmeza-admin .
```

### Run

```bash
docker run -d \
  -p 5000:8080 \
  -e ConnectionStrings__DefaultConnection="Host=..." \
  --name firmeza-admin \
  firmeza-admin
```

### Con Docker Compose

```bash
docker-compose up -d admin
```

---

## 🎨 Interfaz de Usuario

### Layout
- **Navbar:** Menú de navegación con logo
- **Sidebar:** Menú lateral en el área admin
- **Footer:** Información de copyright
- **Responsive:** Adaptado a móviles y tablets

### Componentes
- **Tablas:** Con búsqueda, paginación y ordenamiento
- **Formularios:** Validación client-side y server-side
- **Modales:** Para confirmaciones y acciones rápidas
- **Alertas:** Notificaciones de éxito/error con Toastr
- **Badges:** Indicadores de estado (stock, activo/inactivo)

### Tecnologías Frontend
- **Bootstrap 5:** Framework CSS
- **jQuery:** Manipulación del DOM
- **DataTables:** Tablas interactivas
- **Toastr:** Notificaciones elegantes
- **Font Awesome:** Iconos

---

## 📊 Modelos de Vista

### ProductoViewModel
```csharp
public class ProductoViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; }
    public bool Activo { get; set; }
}
```

### VentaViewModel
```csharp
public class VentaViewModel
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string ClienteNombre { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; }
    public List<DetalleVentaViewModel> Detalles { get; set; }
}
```

---

## 🔐 Autorización

### Protección de Áreas

```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductosController : Controller
{
    // Solo accesible para usuarios con rol Admin
}
```

### Verificación en Vistas

```razor
@if (User.IsInRole("Admin"))
{
    <a href="/Admin/Dashboard" class="btn btn-primary">
        Panel de Administración
    </a>
}
```

---

## 📥 Importación Masiva

### Formato Excel

| Nombre | Descripción | Precio | Stock | Categoría | Activo |
|--------|-------------|--------|-------|-----------|--------|
| Cemento | Cemento gris | 32500 | 100 | Cemento | Sí |
| Ladrillo | Ladrillo rojo | 850 | 5000 | Mampostería | Sí |

### Proceso
1. Descargar plantilla Excel
2. Rellenar datos
3. Subir archivo
4. Sistema valida y procesa
5. Notificación de resultados

---

## 📄 Generación de PDFs

### Recibos de Venta
- Logo de la empresa
- Información del cliente
- Detalles de productos
- Subtotal, IVA, Total
- Fecha y hora
- Método de pago

### Generación

```csharp
public async Task<byte[]> GenerarReciboPdf(int ventaId)
{
    var venta = await _ventaRepository.GetByIdAsync(ventaId);
    // Genera PDF con iTextSharp
    return pdfBytes;
}
```

---

## 🧪 Testing

### Health Check

```bash
curl http://localhost:5000/health
```

**Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-12-01T18:00:00Z",
  "environment": "Production",
  "application": "Firmeza.Web"
}
```

---

## 🚧 Roadmap

- [ ] Dashboard con gráficos avanzados
- [ ] Reportes de ventas en Excel/PDF
- [ ] Gestión de empleados/vendedores
- [ ] Control de inventario con alertas
- [ ] Integración con pasarelas de pago
- [ ] Notificaciones en tiempo real
- [ ] Auditoría de cambios
- [ ] Backup automático de BD

---

## 📝 Logs

Los logs se escriben en la consola y pueden verse con:

```bash
docker logs firmeza-admin -f
```

---

## 🤝 Contribuir

Ver [CONTRIBUTING.md](../CONTRIBUTING.md) en la raíz del proyecto.

---

## 📄 Licencia

Ver [LICENSE](../LICENSE) en la raíz del proyecto.

