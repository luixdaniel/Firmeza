using ApiFirmeza.Web.DTOs;
using AutoMapper;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiFirmeza.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(
        IProductoService productoService,
        IMapper mapper,
        ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Obtiene todos los productos
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
    {
        try
        {
            var productos = await _productoService.GetAllAsync();
            var productosDto = _mapper.Map<IEnumerable<ProductoDto>>(productos);
            return Ok(productosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un producto por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> GetById(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });

            var productoDto = _mapper.Map<ProductoDto>(producto);
            return Ok(productoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener producto {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca productos por nombre
    /// </summary>
    [HttpGet("buscar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Search([FromQuery] string nombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Ok(new List<ProductoDto>());

            var productos = await _productoService.SearchByNombreAsync(nombre);
            var productosDto = _mapper.Map<IEnumerable<ProductoDto>>(productos);
            return Ok(productosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar productos");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene productos por categoría
    /// </summary>
    [HttpGet("categoria/{categoriaId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetByCategoria(int categoriaId)
    {
        try
        {
            var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
            var productosDto = _mapper.Map<IEnumerable<ProductoDto>>(productos);
            return Ok(productosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por categoría");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo producto (solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoCreateDto productoDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = _mapper.Map<Producto>(productoDto);
            await _productoService.CreateAsync(producto);

            var productoCreado = _mapper.Map<ProductoDto>(producto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, productoCreado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un producto existente (solo Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto productoDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productoExistente = await _productoService.GetByIdAsync(id);
            if (productoExistente == null)
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });

            _mapper.Map(productoDto, productoExistente);
            await _productoService.UpdateAsync(productoExistente);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar producto {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un producto (solo Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var resultado = await _productoService.DeleteAsync(id);
            if (!resultado)
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar producto {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
}

