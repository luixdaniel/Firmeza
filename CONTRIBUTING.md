# 🤝 CONTRIBUTING.md - Guía de Contribución

¡Gracias por tu interés en contribuir al proyecto Firmeza! Esta guía te ayudará a empezar.

---

## 📋 Tabla de Contenidos

1. [Código de Conducta](#código-de-conducta)
2. [¿Cómo Contribuir?](#cómo-contribuir)
3. [Configuración del Entorno](#configuración-del-entorno)
4. [Flujo de Trabajo](#flujo-de-trabajo)
5. [Estándares de Código](#estándares-de-código)
6. [Tests](#tests)
7. [Documentación](#documentación)
8. [Pull Requests](#pull-requests)

---

## 📜 Código de Conducta

Este proyecto se adhiere al [Contributor Covenant Code of Conduct](https://www.contributor-covenant.org/). Al participar, se espera que mantengas este código.

### Nuestros Estándares

✅ **Comportamiento esperado:**
- Ser respetuoso con todos los colaboradores
- Aceptar críticas constructivas
- Enfocarse en lo que es mejor para la comunidad
- Mostrar empatía hacia otros

❌ **Comportamiento inaceptable:**
- Lenguaje o imágenes sexualizadas
- Trolling, insultos o comentarios despectivos
- Acoso público o privado
- Publicar información privada de otros

---

## 🎯 ¿Cómo Contribuir?

### Reportar Bugs

1. Verifica que el bug no haya sido reportado
2. Abre un [nuevo issue](../../issues/new)
3. Incluye:
   - Descripción clara del bug
   - Pasos para reproducir
   - Comportamiento esperado vs actual
   - Screenshots si aplica
   - Versión del software/navegador

**Template de Bug:**

```markdown
**Descripción del Bug**
Descripción clara y concisa del problema.

**Pasos para Reproducir**
1. Ir a '...'
2. Hacer clic en '...'
3. Ver error

**Comportamiento Esperado**
Qué esperabas que sucediera.

**Screenshots**
Si aplica, agrega screenshots.

**Entorno**
- OS: [ej. Windows 11]
- Navegador: [ej. Chrome 120]
- Versión: [ej. 1.0.0]
```

### Solicitar Funcionalidades

1. Abre un [nuevo issue](../../issues/new)
2. Usa el label `enhancement`
3. Describe:
   - El problema que resuelve
   - Solución propuesta
   - Alternativas consideradas

### Contribuir Código

1. Fork el repositorio
2. Crea una rama para tu feature
3. Implementa los cambios
4. Escribe/actualiza tests
5. Actualiza documentación
6. Envía un Pull Request

---

## 🛠️ Configuración del Entorno

### 1. Fork y Clone

```bash
# Fork en GitHub (botón Fork)

# Clone tu fork
git clone https://github.com/TU-USUARIO/firmeza.git
cd firmeza

# Agregar remote upstream
git remote add upstream https://github.com/USUARIO-ORIGINAL/firmeza.git
```

### 2. Instalar Dependencias

```bash
# .NET
dotnet restore

# Node.js (cliente)
cd firmeza-client
npm install
cd ..
```

### 3. Configurar Base de Datos

```bash
# Copiar .env de ejemplo
cp .env.example .env

# Editar con tus credenciales
nano .env

# Aplicar migraciones
cd ApiFirmeza.Web
dotnet ef database update
```

### 4. Iniciar en Desarrollo

```bash
# Con Docker
docker-compose up --build

# O manualmente
# Terminal 1: API
cd ApiFirmeza.Web && dotnet run

# Terminal 2: Admin
cd Firmeza.Web && dotnet run

# Terminal 3: Cliente
cd firmeza-client && npm run dev
```

---

## 🔄 Flujo de Trabajo

### 1. Crear una Rama

```bash
# Actualizar main
git checkout main
git pull upstream main

# Crear rama para tu feature
git checkout -b feature/nombre-descriptivo

# O para un bugfix
git checkout -b fix/nombre-del-bug
```

**Convención de nombres de ramas:**
- `feature/` - Nueva funcionalidad
- `fix/` - Corrección de bug
- `docs/` - Solo documentación
- `refactor/` - Refactorización
- `test/` - Agregar/mejorar tests

### 2. Hacer Cambios

```bash
# Hacer tus cambios

# Stage changes
git add .

# Commit con mensaje descriptivo
git commit -m "feat: agregar endpoint de búsqueda de productos"
```

### 3. Mantener Actualizada tu Rama

```bash
# Fetch cambios del upstream
git fetch upstream

# Merge o rebase
git rebase upstream/main

# O si prefieres merge
git merge upstream/main
```

### 4. Push y Pull Request

```bash
# Push a tu fork
git push origin feature/nombre-descriptivo

# Crear PR en GitHub
```

---

## 📝 Estándares de Código

### C# (.NET)

#### Convenciones de Nomenclatura

```csharp
// PascalCase para clases, métodos, propiedades
public class ProductoService
{
    public async Task<Producto> GetByIdAsync(int id)
    {
        // ...
    }
}

// camelCase para variables locales y parámetros
public void ProcessOrder(int orderId)
{
    var orderDetails = await _repository.GetDetailsAsync(orderId);
}

// _camelCase para campos privados
private readonly IProductoRepository _productoRepository;

// UPPER_CASE para constantes
private const int MAX_RETRY_ATTEMPTS = 3;
```

#### Formato

```csharp
// Usar 'var' cuando el tipo es obvio
var producto = new Producto();
var productos = await _repository.GetAllAsync();

// Usar tipo explícito cuando no es obvio
IEnumerable<Producto> productos = GetProductos();

// Siempre usar llaves para if/else/for/while
if (producto != null)
{
    // Código aquí
}

// Async/await siempre que sea posible
public async Task<ActionResult> GetProductos()
{
    var productos = await _service.GetAllAsync();
    return Ok(productos);
}
```

#### Comentarios

```csharp
// Comentarios para explicar "por qué", no "qué"
// Bueno:
// Usamos cache aquí porque esta query es muy costosa
var productos = await _cache.GetOrCreateAsync("productos", ...);

// Malo:
// Obtiene todos los productos
var productos = await _repository.GetAllAsync();

/// <summary>
/// XML comments para métodos públicos
/// </summary>
/// <param name="id">ID del producto</param>
/// <returns>Producto encontrado o null</returns>
public async Task<Producto> GetByIdAsync(int id)
```

### TypeScript/React

#### Convenciones de Nomenclatura

```typescript
// PascalCase para componentes y interfaces
interface Producto {
  id: number;
  nombre: string;
}

function ProductCard({ producto }: { producto: Producto }) {
  // ...
}

// camelCase para variables, funciones
const handleAddToCart = (producto: Producto) => {
  // ...
};

// UPPER_SNAKE_CASE para constantes
const API_BASE_URL = 'http://localhost:5090';
```

#### Componentes

```tsx
// Usar functional components con TypeScript
interface ProductCardProps {
  producto: Producto;
  onAddToCart: (producto: Producto) => void;
}

export function ProductCard({ producto, onAddToCart }: ProductCardProps) {
  return (
    <div className="product-card">
      <h3>{producto.nombre}</h3>
      <p>${producto.precio}</p>
      <button onClick={() => onAddToCart(producto)}>
        Agregar
      </button>
    </div>
  );
}
```

#### Hooks

```typescript
// Custom hooks con 'use' prefix
function useAuth() {
  const [user, setUser] = useState<User | null>(null);
  
  useEffect(() => {
    // Logic
  }, []);
  
  return { user, setUser };
}
```

### SQL

```sql
-- UPPER_CASE para keywords SQL
SELECT p.Id, p.Nombre, c.Nombre AS CategoriaNombre
FROM Productos p
INNER JOIN Categorias c ON p.CategoriaId = c.Id
WHERE p.Activo = TRUE
ORDER BY p.Nombre;

-- Indent para legibilidad
CREATE TABLE Productos (
    Id SERIAL PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaId INT NOT NULL,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);
```

---

## 🧪 Tests

### Escribir Tests

**Siempre incluye tests para:**
- Nuevas funcionalidades
- Correcciones de bugs
- Cambios en lógica de negocio

### Ejecutar Tests

```bash
# Todos los tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true

# Solo un proyecto
dotnet test Firmeza.Tests/Firmeza.Tests.csproj

# Con Docker
docker-compose run tests
```

### Estructura de Tests

```csharp
public class ProductoServiceTests
{
    // Patrón AAA: Arrange, Act, Assert
    [Fact]
    public async Task CreateProducto_WithValidData_ReturnsProducto()
    {
        // Arrange
        var mockRepo = new Mock<IProductoRepository>();
        var service = new ProductoService(mockRepo.Object);
        var producto = new Producto { Nombre = "Test" };
        
        // Act
        var result = await service.CreateAsync(producto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Nombre);
    }
}
```

### Cobertura Mínima

- **Controllers**: 80%
- **Services**: 90%
- **Repositories**: 70%

---

## 📚 Documentación

### Actualizar Documentación

Al hacer cambios que afecten:
- API endpoints → Actualizar `ApiFirmeza.Web/README.md`
- Configuración → Actualizar `DEPLOYMENT.md`
- Arquitectura → Actualizar `ARCHITECTURE.md`
- UI → Actualizar `firmeza-client/README.md`

### Comentarios en Código

```csharp
// README para cada proyecto
// ARCHITECTURE.md para decisiones arquitectónicas
// DEPLOYMENT.md para instrucciones de deploy
// Este archivo (CONTRIBUTING.md) para guías de contribución
```

### Documentar APIs

```csharp
/// <summary>
/// Obtiene todos los productos activos
/// </summary>
/// <returns>Lista de productos activos</returns>
/// <response code="200">Retorna la lista de productos</response>
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK)]
public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
{
    // ...
}
```

---

## 🔀 Pull Requests

### Antes de Enviar

✅ **Checklist:**
- [ ] El código compila sin errores
- [ ] Todos los tests pasan
- [ ] Agregaste tests para tu código
- [ ] Actualizaste la documentación
- [ ] Seguiste los estándares de código
- [ ] Commit messages son descriptivos
- [ ] No hay conflictos con main
- [ ] El PR es de un solo feature/fix (no múltiples cambios)

### Crear el PR

1. Ve a tu fork en GitHub
2. Click en "Pull Request"
3. Selecciona tu rama
4. Completa el template:

```markdown
## Descripción
Breve descripción de los cambios.

## Tipo de Cambio
- [ ] Bug fix
- [ ] Nueva funcionalidad
- [ ] Breaking change
- [ ] Documentación

## ¿Cómo se ha probado?
Describe cómo probaste los cambios.

## Checklist
- [ ] Mi código sigue los estándares del proyecto
- [ ] He realizado una self-review
- [ ] He comentado código complejo
- [ ] He actualizado la documentación
- [ ] Mis cambios no generan warnings
- [ ] He agregado tests
- [ ] Todos los tests (nuevos y existentes) pasan
```

### Después del PR

- Responde a comentarios de revisión
- Haz los cambios solicitados
- Push de nuevos commits
- Una vez aprobado, será merged

---

## 🎨 Convenciones de Commits

Usamos [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Tipos

- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `docs`: Solo documentación
- `style`: Formato (no afecta código)
- `refactor`: Refactorización
- `test`: Agregar/modificar tests
- `chore`: Tareas de mantenimiento

### Ejemplos

```bash
# Feature
git commit -m "feat(api): agregar endpoint de búsqueda de productos"

# Bug fix
git commit -m "fix(client): corregir cálculo de total en carrito"

# Documentation
git commit -m "docs(readme): actualizar instrucciones de instalación"

# Refactor
git commit -m "refactor(service): simplificar lógica de validación"

# Breaking change
git commit -m "feat(api)!: cambiar estructura de response de productos

BREAKING CHANGE: La respuesta ahora incluye metadatos de paginación"
```

---

## 🏆 Reconocimiento

Los contribuidores serán agregados al archivo `CONTRIBUTORS.md` y mencionados en release notes.

---

## 🤔 ¿Preguntas?

Si tienes preguntas sobre cómo contribuir:

1. Revisa esta guía y la [documentación](README.md)
2. Busca en [issues existentes](../../issues)
3. Crea un [nuevo issue](../../issues/new) con la etiqueta `question`

---

## 📞 Contacto

- **Email**: dev@firmeza.com
- **Discord**: [Servidor de Firmeza](#)
- **Twitter**: [@firmeza](#)

---

¡Gracias por contribuir a Firmeza! 🎉

---

Última actualización: 2025-12-01

