using Firmeza.Web.Areas.Admin.Controllers;
using Firmeza.Web.Areas.Admin.ViewModels;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace Firmeza.Tests.Controllers;

/// <summary>
/// Tests unitarios para ProductosController
/// </summary>
public class ProductosControllerTests
{
    private readonly Mock<IProductoService> _mockProductoService;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _mockProductoService = new Mock<IProductoService>();
        // Crear AppDbContext con opciones null (no se usará en los tests)
        _controller = new ProductosController(_mockProductoService.Object, null!);

        // Configurar TempData para evitar errores en los tests
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        _controller.TempData = tempData;
    }

    #region Index Tests

    [Fact]
    public async Task Index_DebeRetornarVistaConListaDeProductos()
    {
        // Arrange
        var productos = new List<Producto>
        {
            new Producto
            {
                Id = 1,
                Nombre = "Producto 1",
                Descripcion = "Descripción 1",
                Precio = 100,
                Stock = 10,
                CategoriaId = 1,
                Activo = true,
                ImagenUrl = "imagen1.jpg",
                Categoria = new Categoria { Id = 1, Nombre = "Categoría 1" }
            },
            new Producto
            {
                Id = 2,
                Nombre = "Producto 2",
                Descripcion = "Descripción 2",
                Precio = 200,
                Stock = 20,
                CategoriaId = 2,
                Activo = false,
                ImagenUrl = null,
                Categoria = new Categoria { Id = 2, Nombre = "Categoría 2" }
            }
        };

        _mockProductoService.Setup(s => s.GetAllAsync())
            .ReturnsAsync(productos);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<List<ProductViewModel>>().Subject;
        model.Should().HaveCount(2);
        model[0].Nombre.Should().Be("Producto 1");
        model[0].Activo.Should().BeTrue();
        model[1].Nombre.Should().Be("Producto 2");
        model[1].Activo.Should().BeFalse();
    }

    [Fact]
    public async Task Index_CuandoOcurreError_DebeRetornarVistaConListaVacia()
    {
        // Arrange
        _mockProductoService.Setup(s => s.GetAllAsync())
            .ThrowsAsync(new Exception("Error de prueba"));

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<List<ProductViewModel>>().Subject;
        model.Should().BeEmpty();
        _controller.TempData["Error"].Should().NotBeNull();
    }

    #endregion

    #region Details Tests

    [Fact]
    public async Task Details_ConIdValido_DebeRetornarVistaConProducto()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Descripcion = "Descripción Test",
            Precio = 150,
            Stock = 15,
            CategoriaId = 1,
            Activo = true,
            ImagenUrl = "test.jpg",
            Categoria = new Categoria { Id = 1, Nombre = "Categoría Test" }
        };

        _mockProductoService.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(producto);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<ProductViewModel>().Subject;
        model.Id.Should().Be(1);
        model.Nombre.Should().Be("Producto Test");
        model.Activo.Should().BeTrue();
    }

    [Fact]
    public async Task Details_ConIdNull_DebeRetornarNotFound()
    {
        // Act
        var result = await _controller.Details(null);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Details_ConIdInexistente_DebeRetornarNotFound()
    {
        // Arrange
        _mockProductoService.Setup(s => s.GetByIdAsync(999))
            .ReturnsAsync((Producto)null);

        // Act
        var result = await _controller.Details(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_POST_ConModeloValido_DebeActualizarYRedireccionar()
    {
        // Arrange
        var viewModel = new EditProductViewModel
        {
            Id = 1,
            Nombre = "Producto Actualizado",
            Descripcion = "Descripción Actualizada",
            Precio = 250,
            Stock = 25,
            CategoriaId = 1
        };

        _mockProductoService.Setup(s => s.UpdateAsync(It.IsAny<Producto>()))
            .ReturnsAsync(new Producto { Id = 1, Nombre = "Producto Actualizado" });

        // Act
        var result = await _controller.Edit(1, viewModel);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("Index");
        _controller.TempData["Success"].Should().NotBeNull();
        _mockProductoService.Verify(s => s.UpdateAsync(It.IsAny<Producto>()), Times.Once);
    }

    [Fact]
    public async Task Edit_POST_ConIdDiferente_DebeRetornarNotFound()
    {
        // Arrange
        var viewModel = new EditProductViewModel { Id = 1 };

        // Act
        var result = await _controller.Edit(2, viewModel);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_GET_ConIdValido_DebeRetornarVistaConProducto()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto a Eliminar",
            Descripcion = "Descripción",
            Precio = 100,
            Stock = 10,
            CategoriaId = 1,
            Categoria = new Categoria { Id = 1, Nombre = "Categoría" }
        };

        _mockProductoService.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(producto);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<ProductViewModel>().Subject;
        model.Id.Should().Be(1);
    }

    [Fact]
    public async Task DeleteConfirmed_ConIdValido_DebeEliminarYRedireccionar()
    {
        // Arrange
        _mockProductoService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _controller.TempData["Success"].Should().NotBeNull();
        _mockProductoService.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteConfirmed_ConIdInexistente_DebeMostrarError()
    {
        // Arrange
        _mockProductoService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteConfirmed(999);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _controller.TempData["Error"].Should().NotBeNull();
    }

    #endregion
}

