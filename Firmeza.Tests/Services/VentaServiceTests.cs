using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Firmeza.Web.Services;
using FluentAssertions;
using Moq;

namespace Firmeza.Tests.Services;

/// <summary>
/// Tests unitarios para VentaService
/// </summary>
public class VentaServiceTests
{
    private readonly Mock<IVentaRepository> _mockVentaRepository;
    private readonly Mock<IProductoRepository> _mockProductoRepository;
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<IPdfService> _mockPdfService;
    private readonly VentaService _service;

    public VentaServiceTests()
    {
        _mockVentaRepository = new Mock<IVentaRepository>();
        _mockProductoRepository = new Mock<IProductoRepository>();
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockPdfService = new Mock<IPdfService>();
        _service = new VentaService(
            _mockVentaRepository.Object, 
            _mockProductoRepository.Object,
            _mockClienteRepository.Object,
            _mockPdfService.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DebeRetornarTodasLasVentas()
    {
        // Arrange
        var ventas = new List<Venta>
        {
            new Venta { Id = 1, Cliente = "Cliente 1", Total = 100 },
            new Venta { Id = 2, Cliente = "Cliente 2", Total = 200 }
        };

        _mockVentaRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(ventas);

        // Act
        var result = await _service.GetAllAsync();
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(2);
        resultList.Should().BeEquivalentTo(ventas);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ConIdValido_DebeRetornarVenta()
    {
        // Arrange
        var venta = new Venta
        {
            Id = 1,
            Cliente = "Cliente Test",
            Total = 150,
            ClienteId = 1
        };

        _mockVentaRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(venta);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Cliente.Should().Be("Cliente Test");
    }

    [Fact]
    public async Task GetByIdAsync_ConIdInexistente_DebeRetornarNull()
    {
        // Arrange
        _mockVentaRepository.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Venta)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ConIdExistente_DebeEliminarYRetornarTrue()
    {
        // Arrange
        _mockVentaRepository.Setup(r => r.ExistsAsync(1))
            .ReturnsAsync(true);
        _mockVentaRepository.Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync((Venta?)null);
        _mockVentaRepository.Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockVentaRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ConIdInexistente_DebeRetornarFalse()
    {
        // Arrange
        _mockVentaRepository.Setup(r => r.ExistsAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockVentaRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    #endregion

    #region CrearVentaConDetallesAsync Tests

    [Fact]
    public async Task CrearVentaConDetallesAsync_ConVentaValida_DebeCrearVenta()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Precio = 100,
            Stock = 50,
            Activo = true
        };

        var venta = new Venta
        {
            Cliente = "Cliente Test",
            ClienteId = 1,
            MetodoPago = "Efectivo",
            Detalles = new List<DetalleDeVenta>
            {
                new DetalleDeVenta
                {
                    ProductoId = 1,
                    Cantidad = 2,
                    PrecioUnitario = 100
                }
            }
        };

        _mockProductoRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(producto);

        _mockVentaRepository.Setup(r => r.AddAsync(It.IsAny<Venta>()))
            .Returns(Task.CompletedTask);

        _mockProductoRepository.Setup(r => r.UpdateAsync(It.IsAny<Producto>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CrearVentaConDetallesAsync(venta);

        // Assert
        venta.NumeroFactura.Should().NotBeNullOrEmpty();
        venta.Subtotal.Should().Be(200);
        venta.Total.Should().BeGreaterThan(200); // Incluye IVA
        _mockVentaRepository.Verify(r => r.AddAsync(It.IsAny<Venta>()), Times.Once);
    }

    [Fact]
    public async Task CrearVentaConDetallesAsync_ConClienteVacio_DebeLanzarException()
    {
        // Arrange
        var venta = new Venta
        {
            Cliente = "",
            Detalles = new List<DetalleDeVenta>()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CrearVentaConDetallesAsync(venta));
        exception.Message.Should().Contain("El cliente es requerido");
    }

    [Fact]
    public async Task CrearVentaConDetallesAsync_SinDetalles_DebeLanzarException()
    {
        // Arrange
        var venta = new Venta
        {
            Cliente = "Cliente Test",
            Detalles = new List<DetalleDeVenta>()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CrearVentaConDetallesAsync(venta));
        exception.Message.Should().Contain("al menos un detalle");
    }

    [Fact]
    public async Task CrearVentaConDetallesAsync_ConStockInsuficiente_DebeLanzarException()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Precio = 100,
            Stock = 1, // Stock insuficiente
            Activo = true
        };

        var venta = new Venta
        {
            Cliente = "Cliente Test",
            ClienteId = 1,
            Detalles = new List<DetalleDeVenta>
            {
                new DetalleDeVenta
                {
                    ProductoId = 1,
                    Cantidad = 5, // Más de lo disponible
                    PrecioUnitario = 100
                }
            }
        };

        _mockProductoRepository.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(producto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CrearVentaConDetallesAsync(venta));
        exception.Message.Should().Contain("Stock insuficiente");
    }

    #endregion
}

