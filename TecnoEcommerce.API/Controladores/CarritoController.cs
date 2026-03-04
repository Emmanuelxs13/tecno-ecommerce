using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Controladores;

/// <summary>
/// Controlador del carrito de compras.
/// Los endpoints incluyen el usuarioId en la ruta para identificar el carrito propietario.
/// Principio SRP: gestiona únicamente las operaciones del carrito.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CarritoController : ControllerBase
{
    private readonly ICarritoServicio _servicio;

    /// <summary>Inicializa el controlador con el servicio del carrito.</summary>
    public CarritoController(ICarritoServicio servicio) => _servicio = servicio;

    /// <summary>
    /// Obtiene el carrito activo del usuario (lo crea si no existe).
    /// GET api/carrito/{usuarioId}
    /// </summary>
    [HttpGet("{usuarioId:int}")]
    [ProducesResponseType(typeof(CarritoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCarrito(int usuarioId) =>
        Ok(await _servicio.ObtenerCarritoAsync(usuarioId));

    /// <summary>
    /// Agrega un producto al carrito del usuario.
    /// POST api/carrito/{usuarioId}/items
    /// </summary>
    [HttpPost("{usuarioId:int}/items")]
    [ProducesResponseType(typeof(CarritoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AgregarItem(int usuarioId, [FromBody] AgregarItemCarritoDto dto)
    {
        try
        {
            var carrito = await _servicio.AgregarItemAsync(usuarioId, dto);
            return Ok(carrito);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un ítem específico del carrito del usuario.
    /// DELETE api/carrito/{usuarioId}/items/{itemId}
    /// </summary>
    [HttpDelete("{usuarioId:int}/items/{itemId:int}")]
    [ProducesResponseType(typeof(CarritoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EliminarItem(int usuarioId, int itemId)
    {
        try
        {
            var carrito = await _servicio.EliminarItemAsync(usuarioId, itemId);
            return Ok(carrito);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza la cantidad de un ítem del carrito.
    /// PUT api/carrito/{usuarioId}/items/{itemId}
    /// </summary>
    [HttpPut("{usuarioId:int}/items/{itemId:int}")]
    [ProducesResponseType(typeof(CarritoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarCantidad(
        int usuarioId, int itemId, [FromBody] ActualizarCantidadItemDto dto)
    {
        try
        {
            var carrito = await _servicio.ActualizarCantidadItemAsync(usuarioId, itemId, dto.Cantidad);
            return Ok(carrito);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Vacía por completo el carrito del usuario.
    /// DELETE api/carrito/{usuarioId}
    /// </summary>
    [HttpDelete("{usuarioId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Vaciar(int usuarioId)
    {
        await _servicio.VaciarCarritoAsync(usuarioId);
        return NoContent();
    }
}
