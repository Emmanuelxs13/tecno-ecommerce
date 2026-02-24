using TecnoEcommerce.Domain.Entities;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de carritos
/// </summary>
public interface ICarritoRepository : IRepository<Carrito>
{
    Task<Carrito?> GetByUsuarioIdAsync(Guid usuarioId);
}
