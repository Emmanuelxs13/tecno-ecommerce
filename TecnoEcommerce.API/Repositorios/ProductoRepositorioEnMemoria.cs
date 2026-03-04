using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Implementación en memoria de <see cref="IProductoRepositorio"/>.
/// Carga la propiedad de navegación Categoria para que el servicio pueda
/// incluir el NombreCategoria en el DTO sin llamadas adicionales.
/// </summary>
public sealed class ProductoRepositorioEnMemoria
    : RepositorioEnMemoria<Producto>, IProductoRepositorio
{
    private readonly IRepositorio<Categoria> _categorias;

    public ProductoRepositorioEnMemoria(IRepositorio<Categoria> categorias)
        : base(p => p.Id, (p, id) => p.Id = id)
    {
        _categorias = categorias;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        var lista = _almacen.Values.ToList();
        await CargarCategoriasAsync(lista);
        return lista;
    }

    /// <inheritdoc/>
    public override async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        if (!_almacen.TryGetValue(id, out var p)) return null;
        p.Categoria = await _categorias.ObtenerPorIdAsync(p.CategoriaId);
        return p;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        var lista = _almacen.Values.Where(p => p.CategoriaId == categoriaId).ToList();
        await CargarCategoriasAsync(lista);
        return lista;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
    {
        var lista = _almacen.Values
            .Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            .ToList();
        await CargarCategoriasAsync(lista);
        return lista;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> ObtenerConStockAsync()
    {
        var lista = _almacen.Values.Where(p => p.Stock > 0).ToList();
        await CargarCategoriasAsync(lista);
        return lista;
    }

    // ── Helper ────────────────────────────────────────────────────────────────

    private async Task CargarCategoriasAsync(IEnumerable<Producto> productos)
    {
        foreach (var p in productos)
            p.Categoria = await _categorias.ObtenerPorIdAsync(p.CategoriaId);
    }
}
