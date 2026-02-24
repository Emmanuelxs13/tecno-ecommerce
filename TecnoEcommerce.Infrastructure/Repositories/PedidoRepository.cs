using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;
using TecnoEcommerce.Infrastructure.Data;

namespace TecnoEcommerce.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de pedidos con métodos específicos
/// </summary>
public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(TecnoEcommerceDbContext context) : base(context)
    {
    }

    public override async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _dbSet
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();
    }
}
