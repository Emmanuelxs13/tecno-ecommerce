using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;

namespace TecnoEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    /// <summary>
    /// Obtiene todas las categorías
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene una categoría por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetById(Guid id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);
        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] CrearCategoriaDto dto)
    {
        var categoria = await _categoriaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CrearCategoriaDto dto)
    {
        try
        {
            await _categoriaService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una categoría
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoriaService.DeleteAsync(id);
        return NoContent();
    }
}
