using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    /// <summary>
    /// Obtiene un pedido por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDto>> GetById(Guid id)
    {
        var pedido = await _pedidoService.GetByIdAsync(id);
        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    /// <summary>
    /// Obtiene los pedidos de un usuario
    /// </summary>
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetByUsuarioId(Guid usuarioId)
    {
        var pedidos = await _pedidoService.GetByUsuarioIdAsync(usuarioId);
        return Ok(pedidos);
    }

    /// <summary>
    /// Crea un pedido desde el carrito del usuario
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Create([FromBody] CrearPedidoDto dto)
    {
        try
        {
            var pedido = await _pedidoService.CrearPedidoDesdeCarritoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cambia el estado de un pedido
    /// </summary>
    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] EstadoPedido nuevoEstado)
    {
        try
        {
            await _pedidoService.CambiarEstadoAsync(id, nuevoEstado);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancela un pedido
    /// </summary>
    [HttpPost("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        try
        {
            await _pedidoService.CancelarPedidoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
