# 🧪 Firmeza.Tests - Suite de Pruebas

## 📋 Descripción

Suite de pruebas unitarias y de integración desarrollada con **xUnit** para el proyecto Firmeza. Incluye tests para controladores, servicios, repositorios y validaciones del sistema.

---

## 🏗️ Tecnologías

- **Framework:** xUnit 2.4.2
- **Mocking:** Moq 4.18.4
- **Base de Datos:** Entity Framework Core InMemory
- **Coverage:** Coverlet
- **Assertions:** FluentAssertions (opcional)

---

## 📁 Estructura del Proyecto

```
Firmeza.Tests/
├── Controllers/               # Tests de controladores
│   ├── ProductosControllerTests.cs
│   ├── CategoriasControllerTests.cs
│   ├── ClientesControllerTests.cs
│   ├── VentasControllerTests.cs
│   └── AuthControllerTests.cs
├── Services/                  # Tests de servicios
│   ├── ProductoServiceTests.cs
│   ├── VentaServiceTests.cs
│   ├── EmailServiceTests.cs
│   └── ImportacionServiceTests.cs
├── Repositories/              # Tests de repositorios
│   ├── ProductoRepositoryTests.cs
│   ├── ClienteRepositoryTests.cs
│   └── VentaRepositoryTests.cs
├── Helpers/                   # Helpers para tests
│   ├── TestDbContext.cs       # Contexto de BD en memoria
│   └── MockData.cs            # Datos de prueba
├── Firmeza.Tests.csproj       # Archivo de proyecto
├── Dockerfile                 # Contenedor para ejecutar tests
└── README.md                  # Documentación (este archivo)
```

---

## 🎯 Cobertura de Tests

### Controladores (API)
- ✅ **ProductosController**: CRUD completo, validaciones
- ✅ **CategoriasController**: CRUD, productos por categoría
- ✅ **ClientesController**: Gestión de clientes, búsqueda
- ✅ **VentasController**: Crear ventas, historial, recibos
- ✅ **AuthController**: Login, registro, JWT

### Servicios
- ✅ **ProductoService**: Lógica de negocio de productos
- ✅ **VentaService**: Validación de stock, cálculos
- ✅ **EmailService**: Envío de emails (mocks)
- ✅ **ImportacionService**: Validación de Excel

### Repositorios
- ✅ **ProductoRepository**: Operaciones CRUD, filtros
- ✅ **ClienteRepository**: Búsqueda por email, listados
- ✅ **VentaRepository**: Consultas complejas, joins

---

## 🚀 Ejecutar Tests

### Localmente

```bash
# Ejecutar todos los tests
dotnet test

# Con detalles verbosos
dotnet test --verbosity detailed

# Filtrar por nombre
dotnet test --filter "FullyQualifiedName~Productos"

# Con cobertura de código
dotnet test /p:CollectCoverage=true
```

### Con Docker

```bash
# Build de la imagen de tests
docker build -f Firmeza.Tests/Dockerfile -t firmeza-tests .

# Ejecutar tests en contenedor
docker run --rm firmeza-tests
```

### Con Docker Compose

```bash
# Los tests se ejecutan automáticamente al hacer up
docker-compose up --build

# Solo ejecutar tests
docker-compose run tests
```

---

## 📊 Ejemplo de Tests

### Test de Controlador

```csharp
public class ProductosControllerTests
{
    private readonly Mock<IProductoService> _mockService;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _mockService = new Mock<IProductoService>();
        _controller = new ProductosController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Cemento" },
            new Producto { Id = 2, Nombre = "Ladrillo" }
        };
        _mockService.Setup(s => s.GetAllAsync())
            .ReturnsAsync(productos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Producto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Nombre", "Required");
        var producto = new Producto();

        // Act
        var result = await _controller.Create(producto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
```

### Test de Servicio

```csharp
public class VentaServiceTests
{
    private readonly Mock<IVentaRepository> _mockRepo;
    private readonly Mock<IProductoRepository> _mockProductoRepo;
    private readonly VentaService _service;

    public VentaServiceTests()
    {
        _mockRepo = new Mock<IVentaRepository>();
        _mockProductoRepo = new Mock<IProductoRepository>();
        _service = new VentaService(_mockRepo.Object, _mockProductoRepo.Object);
    }

    [Fact]
    public async Task CreateVenta_ThrowsException_WhenProductOutOfStock()
    {
        // Arrange
        var producto = new Producto { Id = 1, Stock = 0 };
        var venta = new Venta
        {
            Detalles = new List<DetalleVenta>
            {
                new DetalleVenta { ProductoId = 1, Cantidad = 1 }
            }
        };
        
        _mockProductoRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(producto);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(venta)
        );
    }
}
```

### Test de Repositorio (InMemory)

```csharp
public class ProductoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductoRepository _repository;

    public ProductoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new AppDbContext(options);
        _repository = new ProductoRepository(_context);
        
        // Seed data
        _context.Productos.AddRange(
            new Producto { Id = 1, Nombre = "Cemento", Activo = true },
            new Producto { Id = 2, Nombre = "Ladrillo", Activo = false }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetActivos_ReturnsOnlyActiveProducts()
    {
        // Act
        var result = await _repository.GetActivosAsync();

        // Assert
        Assert.Single(result);
        Assert.All(result, p => Assert.True(p.Activo));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
```

---

## 🎯 Categorías de Tests

### [Fact]
Tests unitarios individuales sin parámetros.

```csharp
[Fact]
public void Test_Suma_ReturnsCorrectValue()
{
    Assert.Equal(4, 2 + 2);
}
```

### [Theory] + [InlineData]
Tests parametrizados con múltiples casos.

```csharp
[Theory]
[InlineData(1, 1, 2)]
[InlineData(2, 3, 5)]
[InlineData(-1, 1, 0)]
public void Test_Suma_MultipleCases(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

### [Trait]
Clasificación de tests por categoría.

```csharp
[Fact]
[Trait("Category", "Integration")]
public async Task Test_DatabaseConnection()
{
    // Test de integración con BD real
}
```

---

## 📈 Cobertura de Código

### Generar Reporte

```bash
# Generar cobertura con Coverlet
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Generar reporte HTML con ReportGenerator
reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report
```

### Ver Reporte

```bash
# Abrir en navegador
start coverage-report/index.html  # Windows
open coverage-report/index.html   # Mac/Linux
```

### Objetivo de Cobertura

- **Mínimo:** 70%
- **Objetivo:** 80%
- **Ideal:** 90%+

---

## 🔍 Mejores Prácticas

### Nomenclatura

```csharp
// Patrón: [MethodName]_[Scenario]_[ExpectedBehavior]
public void GetById_WithValidId_ReturnsProduct()
public void Create_WithInvalidData_ThrowsException()
public void Delete_NonExistingId_ReturnsNotFound()
```

### Arrange-Act-Assert (AAA)

```csharp
[Fact]
public void Test_Example()
{
    // Arrange: Preparar datos y mocks
    var producto = new Producto { Nombre = "Test" };
    
    // Act: Ejecutar la acción a probar
    var result = _service.Create(producto);
    
    // Assert: Verificar el resultado
    Assert.NotNull(result);
    Assert.Equal("Test", result.Nombre);
}
```

### Mocking con Moq

```csharp
// Setup: Configurar comportamiento del mock
_mockRepository
    .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new Producto());

// Verify: Verificar que se llamó el método
_mockRepository.Verify(
    r => r.SaveAsync(),
    Times.Once
);
```

---

## 🐳 Docker

### Dockerfile

El Dockerfile ejecuta los tests automáticamente:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copiar archivos y restaurar
COPY . .
RUN dotnet restore Firmeza.Tests/Firmeza.Tests.csproj

# Ejecutar tests
CMD ["dotnet", "test", "Firmeza.Tests/Firmeza.Tests.csproj", "--verbosity", "normal"]
```

### Build y Run

```bash
# Build
docker build -f Firmeza.Tests/Dockerfile -t firmeza-tests .

# Run
docker run --rm firmeza-tests
```

---

## 🧪 Tipos de Tests

### Tests Unitarios
- **Qué:** Prueban unidades individuales (métodos, clases)
- **Aislamiento:** Usan mocks para dependencias
- **Velocidad:** Muy rápidos
- **Ejemplo:** Validación de lógica de negocio

### Tests de Integración
- **Qué:** Prueban interacción entre componentes
- **Aislamiento:** Usan BD en memoria o test containers
- **Velocidad:** Medianos
- **Ejemplo:** Repositorio + DbContext

### Tests E2E (próximamente)
- **Qué:** Prueban flujos completos
- **Aislamiento:** Base de datos real de test
- **Velocidad:** Lentos
- **Ejemplo:** Proceso completo de compra

---

## 📊 Resultados de Tests

### Salida en Consola

```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    45, Skipped:     0, Total:    45, Duration: 2s
```

### Con Docker

```
✅ ProductosController.GetAll_ReturnsOkResult (120ms)
✅ ProductosController.GetById_ReturnsNotFound (45ms)
✅ VentaService.Create_ValidatesStock (89ms)
...
========================================
  TESTS COMPLETADOS EXITOSAMENTE
========================================
Total: 45 tests
Passed: 45
Failed: 0
Skipped: 0
Duration: 2.5s
```

---

## 🚧 Roadmap

- [ ] Tests de integración con TestContainers
- [ ] Tests E2E con Playwright
- [ ] Benchmarks de rendimiento
- [ ] Mutation testing
- [ ] Tests de carga con k6
- [ ] Tests de seguridad

---

## 📝 Comandos Útiles

```bash
# Ejecutar solo tests rápidos
dotnet test --filter "Category=Unit"

# Ejecutar solo tests de controladores
dotnet test --filter "FullyQualifiedName~Controllers"

# Ejecutar con logs detallados
dotnet test --logger "console;verbosity=detailed"

# Ejecutar en paralelo
dotnet test --parallel

# Generar reporte TRX
dotnet test --logger "trx;LogFileName=TestResults.trx"
```

---

## 🤝 Contribuir

Al agregar nuevas funcionalidades, **siempre incluye tests**:

1. Escribe el test primero (TDD)
2. Implementa la funcionalidad
3. Verifica que el test pase
4. Refactoriza si es necesario

Ver [CONTRIBUTING.md](../CONTRIBUTING.md) en la raíz del proyecto.

---

## 📄 Licencia

Ver [LICENSE](../LICENSE) en la raíz del proyecto.

