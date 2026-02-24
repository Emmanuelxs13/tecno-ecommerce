using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;

namespace TecnoEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarritoController : ControllerBase
{
    private readonly ICarritoService _carritoService;

    public CarritoController(ICarritoService carritoService)
    {
        _carritoService = carritoService;
    }

    /// <summary>
    /// Obtiene el carrito de un usuario
    /// </summary>
    [HttpGet("{usuarioId}")]
    public async Task<ActionResult<CarritoDto>> GetByUsuarioId(Guid usuarioId)
    {
        var carrito = await _carritoService.GetByUsuarioIdAsync(usuarioId);
        if (carrito == null)
            return NotFound("Carrito no encontrado");

        return Ok(carrito);
    }

    /// <summary>
    /// Agrega un item al carrito
    /// </summary>
    [HttpPost("{usuarioId}/items")]
    public async Task<ActionResult<CarritoDto>> AgregarItem(Guid usuarioId, [FromBody] AgregarItemCarritoDto dto)
    {
        try
        {
            var carrito = await _carritoService.AgregarItemAsync(usuarioId, dto);
            return Ok(carrito);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica la cantidad de un item en el carrito
    /// </summary>
    [HttpPut("{usuarioId}/items/{itemId}")]
    public async Task<ActionResult<CarritoDto>> ModificarItem(Guid usuarioId, Guid itemId, [FromBody] int cantidad)
    {
        try
        {
            var carrito = await _carritoService.ModificarItemAsync(usuarioId, itemId, cantidad);
            return Ok(carrito);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina un item del carrito
    /// </summary>
    [HttpDelete("{usuarioId}/items/{itemId}")]
    public async Task<IActionResult> EliminarItem(Guid usuarioId, Guid itemId)
    {
        try
        {
            await _carritoService.EliminarItemAsync(usuarioId, itemId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Vacía el carrito de un usuario
    /// </summary>
    [HttpDelete("{usuarioId}")]
    public async Task<IActionResult> VaciarCarrito(Guid usuarioId)
    {
        try
        {
            await _carritoService.VaciarCarritoAsync(usuarioId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
