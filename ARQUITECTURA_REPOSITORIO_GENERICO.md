# 🏗️ Arquitectura Completa con Repositorio Genérico

## ✅ Estructura Implementada

```
Firmeza.Web/
├── Interfaces/
│   └── Repositories/
│       ├── IRepository.cs              ⭐ NUEVO - Interfaz genérica base
│       ├── IProductoRepository.cs      ✅ Hereda de IRepository<Producto>
│       └── ICategoriaRepository.cs     ⭐ NUEVO - Hereda de IRepository<Categoria>
│
├── Repositories/
│   ├── Repository.cs                   ⭐ NUEVO - Implementación genérica base
│   ├── ProductoRepository.cs           ✅ Hereda de Repository<Producto>
│   └── CategoriaRepository.cs          ⭐ NUEVO - Hereda de Repository<Categoria>
│
└── Services/
    └── ProductoService.cs              ✅ Actualizado - usa ambos repositorios
```

---

## 🎯 1. IRepository<TEntity> - Interfaz Genérica Base

```csharp
public interface IRepository<TEntity> where TEntity : class
{
    // Consultas básicas
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    
    // Verificaciones
    Task<bool> ExistsAsync(int id);
    
    // Operaciones CRUD
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
    
    // Persistencia
    Task<int> SaveChangesAsync();
}
```

### ✅ Ventajas:
- **Reutilizable**: Todos los repositorios heredan estos métodos
- **DRY**: No repetir código común
- **Testeable**: Fácil de mockear
- **Mantenible**: Cambios en un solo lugar

---

## 🎯 2. Repository<TEntity> - Implementación Genérica Base

```csharp
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    // ... más métodos implementados
}
```

### ✅ Características Importantes:

1. **Campos Protected**: `Context` y `DbSet` accesibles por clases hijas
2. **Métodos Virtual**: Pueden ser sobrescritos por clases hijas
3. **AsNoTracking()**: Optimización de performance en consultas de solo lectura
4. **Genérico**: Funciona con cualquier entidad

---

## 🎯 3. IProductoRepository - Interfaz Específica

```csharp
public interface IProductoRepository : IRepository<Producto>
{
    // ✅ Hereda automáticamente:
    // - GetAllAsync()
    // - GetByIdAsync()
    // - ExistsAsync()
    // - AddAsync()
    // - UpdateAsync()
    // - DeleteAsync()
    // - SaveChangesAsync()
    
    // ⭐ Solo define métodos específicos de Producto:
    Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Producto>> GetByStockBajoAsync(int minStock = 10);
    Task<IEnumerable<Producto>> SearchByNombreAsync(string nombre);
    Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
}
```

### ✅ Ventaja:
- **Código limpio**: Solo métodos específicos de Producto
- **Herencia**: Todo lo común viene de IRepository

---

## 🎯 4. ProductoRepository - Implementación Específica

```csharp
public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(AppDbContext context) : base(context)
    {
    }

    // ✅ Sobrescribe métodos base para incluir relaciones
    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await Context.Productos
            .Include(p => p.Categoria)  // ⭐ Incluye relación
            .AsNoTracking()
            .ToListAsync();
    }

    public override async Task<Producto?> GetByIdAsync(int id)
    {
        return await Context.Productos
            .Include(p => p.Categoria)  // ⭐ Incluye relación
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // ⭐ Implementa métodos específicos
    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId)
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
    {
        var query = Context.Productos.Where(p => p.Nombre.ToLower() == nombre.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }
}
```

### ✅ Características:
- **Hereda** de `Repository<Producto>`
- **Sobrescribe** métodos cuando necesita personalización (Include)
- **Implementa** métodos específicos de productos
- **No duplica** código (AddAsync, UpdateAsync, etc. vienen de la base)

---

## 🎯 5. ICategoriaRepository - Nueva Interfaz

```csharp
public interface ICategoriaRepository : IRepository<Categoria>
{
    // ⭐ Métodos específicos de Categoría
    Task<Categoria?> GetByNombreAsync(string nombre);
    Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync();
    Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
    Task<bool> TieneProductosAsync(int categoriaId);
}
```

---

## 🎯 6. CategoriaRepository - Nueva Implementación

```csharp
public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Categoria?> GetByNombreAsync(string nombre)
    {
        return await Context.Categorias
            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync()
    {
        return await Context.Categorias
            .Include(c => c.Productos)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> TieneProductosAsync(int categoriaId)
    {
        return await Context.Productos.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}
```

---

## 🎯 7. ProductoService - Actualizado

```csharp
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;  // ⭐ NUEVO

    public ProductoService(
        IProductoRepository productoRepository, 
        ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<bool> CreateProductoAsync(Producto producto)
    {
        // Validaciones...
        
        // ✅ Usa repositorio de categorías
        var categoriaExists = await _categoriaRepository.ExistsAsync(producto.CategoriaId);
        if (!categoriaExists)
            throw new ArgumentException("La categoría no existe.");
        
        // ✅ Usa repositorio de productos
        var nombreExists = await _productoRepository.NombreExistsAsync(producto.Nombre);
        if (nombreExists)
            throw new ArgumentException("El nombre ya existe.");
        
        await _productoRepository.AddAsync(producto);
        await _productoRepository.SaveChangesAsync();
        
        return true;
    }
}
```

### ✅ Mejora:
- **Ya NO usa DbContext directamente**
- **Usa solo repositorios**
- **Mejor separación de responsabilidades**

---

## 🎯 8. Program.cs - Registro de Dependencias

```csharp
// Repositorios
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();  // ⭐ NUEVO

// Servicios
builder.Services.AddScoped<IProductoService, ProductoService>();
```

---

## 📊 Comparación: Antes vs Después

### ❌ ANTES (Sin Repositorio Genérico)

```csharp
public class ProductoRepository : IProductoRepository
{
    public async Task<IEnumerable<Producto>> GetAllAsync() { ... }
    public async Task<Producto?> GetByIdAsync(int id) { ... }
    public async Task<bool> ExistsAsync(int id) { ... }
    public async Task AddAsync(Producto producto) { ... }
    public async Task UpdateAsync(Producto producto) { ... }
    public async Task DeleteAsync(int id) { ... }
    public async Task<int> SaveChangesAsync() { ... }
    // Métodos específicos...
}

public class CategoriaRepository : ICategoriaRepository
{
    // ⚠️ Duplicar TODO el código CRUD otra vez
    public async Task<IEnumerable<Categoria>> GetAllAsync() { ... }
    public async Task<Categoria?> GetByIdAsync(int id) { ... }
    // ... etc (código duplicado)
}
```

**Problema**: Código duplicado en cada repositorio

### ✅ DESPUÉS (Con Repositorio Genérico)

```csharp
// Base genérica (1 vez)
public class Repository<TEntity> : IRepository<TEntity>
{
    // Implementación común
}

// Repositorios específicos (solo código único)
public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    // ✅ Solo métodos específicos de Producto
    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(...) { ... }
}

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    // ✅ Solo métodos específicos de Categoría
    public async Task<Categoria?> GetByNombreAsync(...) { ... }
}
```

**Ventaja**: Código común en un solo lugar, sin duplicación

---

## 🎯 Flujo Completo de Arquitectura

```
┌─────────────────┐
│   Controller    │ ← Maneja HTTP requests
└────────┬────────┘
         │ llama a
         ↓
┌─────────────────┐
│    Service      │ ← Lógica de negocio
└────────┬────────┘
         │ usa
         ↓
┌──────────────────────────────────────┐
│    IProductoRepository                │
│    ICategoriaRepository               │ ← Interfaces específicas
└────────┬─────────────────────────────┘
         │ heredan de
         ↓
┌─────────────────┐
│ IRepository<T>  │ ← Interfaz genérica
└────────┬────────┘
         │
         ↓
┌─────────────────┐
│  Repository<T>  │ ← Implementación base
└────────┬────────┘
         │ usa
         ↓
┌─────────────────┐
│   DbContext     │ ← Entity Framework
└─────────────────┘
```

---

## ✅ Ventajas Finales

1. **DRY (Don't Repeat Yourself)** ✅
   - Código CRUD común en un solo lugar

2. **Escalable** ✅
   - Fácil agregar nuevos repositorios (Cliente, Venta, etc.)

3. **Mantenible** ✅
   - Cambios en la base afectan a todos los repositorios

4. **Testeable** ✅
   - Mock de interfaces genéricas

5. **Separación de Responsabilidades** ✅
   - Service → Repository → DbContext

6. **Performance** ✅
   - AsNoTracking() en consultas de lectura
   - Override cuando se necesita Include()

---

## 🚀 Próximos Pasos

Ahora puedes crear fácilmente repositorios para otras entidades:

### Cliente Repository
```csharp
public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByEmailAsync(string email);
    Task<IEnumerable<Cliente>> GetClientesActivosAsync();
}

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(AppDbContext context) : base(context) { }
    
    // Solo implementar métodos específicos
}
```

### Venta Repository
```csharp
public interface IVentaRepository : IRepository<Venta>
{
    Task<IEnumerable<Venta>> GetVentasPorFechaAsync(DateTime fecha);
    Task<decimal> GetTotalVentasDelMesAsync(int mes, int año);
}

public class VentaRepository : Repository<Venta>, IVentaRepository
{
    public VentaRepository(AppDbContext context) : base(context) { }
    
    // Solo implementar métodos específicos
}
```

---

## 🎉 Conclusión

Tu arquitectura ahora sigue el **patrón Repository genérico**, una de las mejores prácticas en desarrollo con .NET:

- ✅ Repositorio base genérico creado
- ✅ ProductoRepository hereda de la base
- ✅ CategoriaRepository creado heredando de la base
- ✅ Service ya no usa DbContext directamente
- ✅ Código limpio, mantenible y escalable

¡Excelente arquitectura! 🚀

