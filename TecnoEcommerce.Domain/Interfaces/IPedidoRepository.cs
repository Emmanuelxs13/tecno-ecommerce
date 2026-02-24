using TecnoEcommerce.Domain.Entities;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de pedidos
/// </summary>
public interface IPedidoRepository : IRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetByUsuarioIdAsync(Guid usuarioId);
}
