using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Controladores;

/// <summary>
/// Controlador del catálogo de productos.
/// Principio SRP: expone únicamente los endpoints relacionados con productos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoServicio _servicio;

    /// <summary>Inicializa el controlador con el servicio de productos.</summary>
    public ProductosController(IProductoServicio servicio) => _servicio = servicio;

    /// <summary>
    /// Obtiene todos los productos del catálogo.
    /// GET api/productos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos() =>
        Ok(await _servicio.ObtenerTodosAsync());

    /// <summary>
    /// Obtiene un producto por su Id.
    /// GET api/productos/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _servicio.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    /// <summary>
    /// Filtra productos por categoría.
    /// GET api/productos/categoria/{categoriaId}
    /// </summary>
    [HttpGet("categoria/{categoriaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCategoria(int categoriaId) =>
        Ok(await _servicio.ObtenerPorCategoriaAsync(categoriaId));

    /// <summary>
    /// Busca productos por nombre (búsqueda parcial).
    /// GET api/productos/buscar?nombre=...
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string nombre) =>
        Ok(await _servicio.BuscarPorNombreAsync(nombre));

    /// <summary>
    /// Crea un nuevo producto en el catálogo.
    /// POST api/productos
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto)
    {
        try
        {
            var producto = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
