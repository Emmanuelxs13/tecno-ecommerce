using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;

namespace TecnoEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    /// <summary>
    /// Obtiene todos los productos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
    {
        var productos = await _productoService.GetAllAsync();
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetById(Guid id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    /// <summary>
    /// Obtiene productos por categoría
    /// </summary>
    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetByCategoria(Guid categoriaId)
    {
        var productos = await _productoService.GetByCategoriaAsync(categoriaId);
        return Ok(productos);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Create([FromBody] CrearProductoDto dto)
    {
        var producto = await _productoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarProductoDto dto)
    {
        try
        {
            await _productoService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Elimina un producto
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productoService.DeleteAsync(id);
        return NoContent();
    }
}
