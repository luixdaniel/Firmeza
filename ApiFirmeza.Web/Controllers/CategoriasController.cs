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
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(
        ICategoriaService categoriaService,
        IMapper mapper,
        ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las categorías
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        try
        {
            var categorias = await _categoriaService.GetAllAsync();
            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
            return Ok(categoriasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categorías");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una categoría por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaDto>> GetById(int id)
    {
        try
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);
            return Ok(categoriaDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] CategoriaCreateDto categoriaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = new Firmeza.Web.Data.Entities.Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion
            };

            await _categoriaService.CreateAsync(categoria);

            var categoriaCreada = new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                CantidadProductos = 0
            };

            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoriaCreada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear categoría");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza una categoría existente (solo Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateDto categoriaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriaExistente = await _categoriaService.GetByIdAsync(id);
            if (categoriaExistente == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            _mapper.Map(categoriaDto, categoriaExistente);
            await _categoriaService.UpdateAsync(categoriaExistente);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una categoría (solo Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
                return NotFound($"Categoría con ID {id} no encontrada");

            // Verificar si tiene productos asociados
            if (categoria.Productos?.Any() == true)
                return BadRequest("No se puede eliminar una categoría que tiene productos asociados");

            var resultado = await _categoriaService.DeleteAsync(id);
            if (!resultado)
                return NotFound($"Categoría con ID {id} no encontrada");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar categoría {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

