using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;
using TecnoEcommerce.Infrastructure.Data;

namespace TecnoEcommerce.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de productos con métodos específicos
/// </summary>
public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(TecnoEcommerceDbContext context) : base(context)
    {
    }

    public override async Task<Producto?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(Guid categoriaId)
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> GetDisponiblesAsync()
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .Where(p => p.Stock > 0)
            .ToListAsync();
    }
}
