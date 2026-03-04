using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Controladores;

/// <summary>
/// Controlador para la gestión de categorías de productos.
/// Principio SRP: expone únicamente los endpoints de categorías.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaServicio _servicio;

    /// <summary>Inicializa el controlador con el servicio de categorías.</summary>
    public CategoriasController(ICategoriaServicio servicio) => _servicio = servicio;

    /// <summary>
    /// Obtiene todas las categorías con el conteo de productos.
    /// GET api/categorias
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodas() =>
        Ok(await _servicio.ObtenerTodasAsync());

    /// <summary>
    /// Obtiene una categoría por su Id.
    /// GET api/categorias/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var categoria = await _servicio.ObtenerPorIdAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    /// <summary>
    /// Crea una nueva categoría.
    /// POST api/categorias
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
    {
        try
        {
            var categoria = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.Id }, categoria);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
