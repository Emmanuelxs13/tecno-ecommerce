using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Controladores;

/// <summary>
/// Controlador de pedidos.
/// Gestiona la creación de pedidos y la consulta del historial del usuario.
/// Principio SRP: responsabilidad única sobre los endpoints de pedidos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoServicio _servicio;

    /// <summary>Inicializa el controlador con el servicio de pedidos.</summary>
    public PedidosController(IPedidoServicio servicio) => _servicio = servicio;

    /// <summary>
    /// Crea un pedido a partir del carrito activo del usuario.
    /// El flujo incluye validación de stock, pago simulado y generación de envío.
    /// POST api/pedidos/{usuarioId}
    /// </summary>
    [HttpPost("{usuarioId:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(int usuarioId, [FromBody] CrearPedidoDto dto)
    {
        try
        {
            var pedido = await _servicio.CrearPedidoAsync(usuarioId, dto);
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { usuarioId, pedidoId = pedido.Id },
                pedido);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene el historial completo de pedidos del usuario.
    /// GET api/pedidos/mis-pedidos/{usuarioId}
    /// </summary>
    [HttpGet("mis-pedidos/{usuarioId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PedidoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MisPedidos(int usuarioId) =>
        Ok(await _servicio.ObtenerMisPedidosAsync(usuarioId));

    /// <summary>
    /// Obtiene el detalle completo de un pedido específico del usuario.
    /// GET api/pedidos/{usuarioId}/{pedidoId}
    /// </summary>
    [HttpGet("{usuarioId:int}/{pedidoId:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int usuarioId, int pedidoId)
    {
        var pedido = await _servicio.ObtenerPorIdAsync(pedidoId, usuarioId);
        return pedido is null ? NotFound() : Ok(pedido);
    }
}
