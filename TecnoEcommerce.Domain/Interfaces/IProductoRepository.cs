using TecnoEcommerce.Domain.Entities;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de productos con métodos adicionales
/// </summary>
public interface IProductoRepository : IRepository<Producto>
{
    Task<IEnumerable<Producto>> GetByCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<Producto>> GetDisponiblesAsync();
}
