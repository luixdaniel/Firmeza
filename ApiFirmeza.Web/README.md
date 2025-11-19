# 🚀 Firmeza API

API REST para el sistema de gestión Firmeza. Proporciona endpoints para gestionar productos, categorías, clientes y ventas.

## 📋 Características

- ✅ **CRUD completo** para Productos, Categorías, Clientes y Ventas
- ✅ **Documentación Swagger** integrada
- ✅ **CORS** configurado para desarrollo
- ✅ **Validaciones** de modelos
- ✅ **Manejo de errores** centralizado
- ✅ **Entity Framework Core** con SQL Server
- ✅ **DTOs** para separación de capas
- ✅ **Logging** configurado

## 🛠️ Tecnologías

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI
- Dependency Injection

## 📦 Instalación

1. **Restaurar paquetes:**
```bash
dotnet restore
```

2. **Configurar la base de datos:**
   - Editar `appsettings.json` con tu connection string
   - La base de datos se comparte con el proyecto Firmeza.Web

3. **Ejecutar la API:**
```bash
dotnet run
```

4. **Acceder a Swagger:**
   - Abrir navegador en: `https://localhost:5001` o `http://localhost:5000`

## 🌐 Endpoints

### Productos
- `GET /api/productos` - Obtener todos los productos
- `GET /api/productos/{id}` - Obtener producto por ID
- `GET /api/productos/buscar?termino={termino}` - Buscar productos
- `POST /api/productos` - Crear producto
- `PUT /api/productos/{id}` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto

### Categorías
- `GET /api/categorias` - Obtener todas las categorías
- `GET /api/categorias/{id}` - Obtener categoría por ID
- `POST /api/categorias` - Crear categoría
- `PUT /api/categorias/{id}` - Actualizar categoría
- `DELETE /api/categorias/{id}` - Eliminar categoría

### Clientes
- `GET /api/clientes` - Obtener todos los clientes
- `GET /api/clientes/{id}` - Obtener cliente por ID
- `GET /api/clientes/buscar?termino={termino}` - Buscar clientes
- `POST /api/clientes` - Crear cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Ventas
- `GET /api/ventas` - Obtener todas las ventas
- `GET /api/ventas/{id}` - Obtener venta por ID
- `GET /api/ventas/cliente/{clienteId}` - Obtener ventas de un cliente
- `POST /api/ventas` - Crear venta (incluye actualización de stock)

### Health Check
- `GET /health` - Verificar estado de la API

## 📝 Ejemplos de uso

### Crear un producto
```bash
POST /api/productos
Content-Type: application/json

{
  "nombre": "Laptop Dell XPS 15",
  "descripcion": "Laptop de alto rendimiento",
  "precio": 1299.99,
  "stock": 10,
  "categoriaId": 1
}
```

### Crear una venta
```bash
POST /api/ventas
Content-Type: application/json

{
  "cliente": "Juan Pérez",
  "clienteId": 1,
  "metodoPago": "Tarjeta",
  "vendedor": "Admin",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 1299.99
    }
  ]
}
```

## 🔧 Configuración

### Connection String
Editar en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FirmezaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### CORS
La configuración CORS actual permite todos los orígenes. Para producción, modificar en `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", policy =>
    {
        policy.WithOrigins("https://tudominio.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 📊 Estructura del Proyecto

```
ApiFirmeza.Web/
├── Controllers/          # Controladores API
│   ├── ProductosController.cs
│   ├── CategoriasController.cs
│   ├── ClientesController.cs
│   └── VentasController.cs
├── DTOs/                # Data Transfer Objects
│   ├── ProductoDto.cs
│   ├── CategoriaDto.cs
│   ├── ClienteDto.cs
│   └── VentaDto.cs
├── Program.cs           # Configuración principal
├── appsettings.json     # Configuración
└── README.md           # Este archivo
```

## 🔍 Testing con Swagger

1. Ejecutar la API
2. Abrir el navegador en la URL de la API
3. Usar la interfaz de Swagger para probar endpoints
4. Todos los endpoints incluyen documentación integrada

## 📄 Licencia

Este proyecto es parte del sistema Firmeza.

## 👥 Contribución

Para contribuir al proyecto:
1. Fork el repositorio
2. Crea una rama para tu feature
3. Commit tus cambios
4. Push a la rama
5. Crea un Pull Request

## 🆘 Soporte

Para reportar problemas o solicitar características, crear un issue en el repositorio.

