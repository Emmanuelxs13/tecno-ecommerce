using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Datos.Contexto;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.Datos.Repositorios;

/// <summary>
/// Implementación EF Core de <see cref="IProductoRepositorio"/>.
/// Usa eager loading para incluir la categoría en todas las consultas,
/// de modo que los servicios no requieran llamadas adicionales.
/// </summary>
public sealed class ProductoRepositorioEfCore
    : RepositorioEfCore<Producto>, IProductoRepositorio
{
    public ProductoRepositorioEfCore(TiendaContexto contexto) : base(contexto) { }

    /// <inheritdoc/>
    public override async Task<IEnumerable<Producto>> ObtenerTodosAsync() =>
        await _contexto.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .ToListAsync();

    /// <inheritdoc/>
    public override async Task<Producto?> ObtenerPorIdAsync(int id) =>
        await _contexto.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId) =>
        await _contexto.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre) =>
        await _contexto.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(p => EF.Functions.ILike(p.Nombre, $"%{nombre}%"))
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> ObtenerConStockAsync() =>
        await _contexto.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(p => p.Stock > 0)
            .ToListAsync();
}
