using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Services;
using FluentAssertions;
using Moq;

namespace Firmeza.Tests.Services;

/// <summary>
/// Tests unitarios para ProductoService
/// </summary>
public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _mockRepository;
    private readonly ProductoService _service;

    public ProductoServiceTests()
    {
        _mockRepository = new Mock<IProductoRepository>();
        // Crear ProductoService con AppDbContext null (no se usará en los tests de repository)
        _service = new ProductoService(_mockRepository.Object, null!);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DebeRetornarTodosLosProductos()
    {
        // Arrange
        var productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Producto 1", Precio = 100, Stock = 10 },
            new Producto { Id = 2, Nombre = "Producto 2", Precio = 200, Stock = 20 },
            new Producto { Id = 3, Nombre = "Producto 3", Precio = 300, Stock = 30 }
        };

        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(productos);

        // Act
        var result = await _service.GetAllAsync();
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(3);
        resultList.Should().BeEquivalentTo(productos);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_CuandoNoHayProductos_DebeRetornarListaVacia()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Producto>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ConIdValido_DebeRetornarProducto()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Descripcion = "Descripción",
            Precio = 150,
            Stock = 15,
            CategoriaId = 1
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(producto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Nombre.Should().Be("Producto Test");
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ConIdInexistente_DebeRetornarNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Producto)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_ConNombreVacio_DebeLanzarException()
    {
        // Arrange
        var producto = new Producto
        {
            Nombre = "",
            Descripcion = "Descripción",
            Precio = 100,
            Stock = 10
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(producto));
        exception.Message.Should().Contain("nombre");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Producto>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ConPrecioNegativo_DebeLanzarException()
    {
        // Arrange
        var producto = new Producto
        {
            Nombre = "Producto",
            Descripcion = "Descripción",
            Precio = -100,
            Stock = 10
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(producto));
        exception.Message.Should().Contain("precio");
    }

    [Fact]
    public async Task CreateAsync_ConStockNegativo_DebeLanzarException()
    {
        // Arrange
        var producto = new Producto
        {
            Nombre = "Producto",
            Descripcion = "Descripción",
            Precio = 100,
            Stock = -10
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(producto));
        exception.Message.Should().Contain("stock");
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_ConNombreVacio_DebeLanzarException()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "",
            Precio = 100,
            Stock = 10
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(producto));
        exception.Message.Should().Contain("nombre");
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ConIdExistente_DebeEliminarYRetornarTrue()
    {
        // Arrange
        var producto = new Producto { Id = 1, Nombre = "Producto a Eliminar" };
        _mockRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(producto);
        _mockRepository.Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ConIdInexistente_DebeRetornarFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Producto?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    #endregion
}

