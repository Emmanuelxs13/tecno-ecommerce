using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de pedidos
/// </summary>
public interface IPedidoService
{
    Task<PedidoDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PedidoDto>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<PedidoDto> CrearPedidoDesdeCarritoAsync(CrearPedidoDto dto);
    Task CambiarEstadoAsync(Guid pedidoId, EstadoPedido nuevoEstado);
    Task CancelarPedidoAsync(Guid pedidoId);
}
