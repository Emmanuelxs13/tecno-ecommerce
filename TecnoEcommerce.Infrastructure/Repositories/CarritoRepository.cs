using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;
using TecnoEcommerce.Infrastructure.Data;

namespace TecnoEcommerce.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de carritos con métodos específicos
/// </summary>
public class CarritoRepository : Repository<Carrito>, ICarritoRepository
{
    public CarritoRepository(TecnoEcommerceDbContext context) : base(context)
    {
    }

    public override async Task<Carrito?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Carrito?> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _dbSet
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
    }
}
