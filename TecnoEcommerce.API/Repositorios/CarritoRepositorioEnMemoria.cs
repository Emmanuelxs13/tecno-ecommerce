using System.Collections.Concurrent;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Implementación en memoria de <see cref="ICarritoRepositorio"/>.
/// Mantiene los ítems del carrito en un diccionario separado para simular una tabla de relación.
/// </summary>
public sealed class CarritoRepositorioEnMemoria
    : RepositorioEnMemoria<Carrito>, ICarritoRepositorio
{
    /// <summary>Almacén de ítems del carrito.</summary>
    private readonly ConcurrentDictionary<int, ItemCarrito> _items = new();
    private int _itemSecuencia;

    /// <summary>
    /// Repositorio de productos necesario para cargar las propiedades de navegación.
    /// Se inyecta como singleton para que compartan el mismo almacén en memoria.
    /// </summary>
    private readonly IProductoRepositorio _productos;

    public CarritoRepositorioEnMemoria(IProductoRepositorio productos)
        : base(c => c.Id, (c, id) => c.Id = id)
    {
        _productos = productos;
    }

    /// <inheritdoc/>
    public async Task<Carrito?> ObtenerPorUsuarioAsync(int usuarioId)
    {
        var carrito = _almacen.Values.FirstOrDefault(c => c.UsuarioId == usuarioId);
        if (carrito is null) return null;
        await CargarItemsAsync(carrito);
        return carrito;
    }

    /// <inheritdoc/>
    public Task AgregarItemAsync(ItemCarrito item)
    {
        // Si ya existe el mismo producto en el carrito, incrementar cantidad
        var existente = _items.Values
            .FirstOrDefault(i => i.CarritoId == item.CarritoId && i.ProductoId == item.ProductoId);

        if (existente is not null)
        {
            existente.Cantidad += item.Cantidad;
            return Task.CompletedTask;
        }

        var id = Interlocked.Increment(ref _itemSecuencia);
        item.Id = id;
        _items[id] = item;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task EliminarItemAsync(int itemId)
    {
        _items.TryRemove(itemId, out _);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task VaciarCarritoAsync(int carritoId)
    {
        var ids = _items.Values
            .Where(i => i.CarritoId == carritoId)
            .Select(i => i.Id)
            .ToList();
        foreach (var id in ids) _items.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Carga los ítems y la propiedad Producto de cada ítem (eager loading manual).</summary>
    private async Task CargarItemsAsync(Carrito carrito)
    {
        var itemsDelCarrito = _items.Values
            .Where(i => i.CarritoId == carrito.Id)
            .ToList();

        foreach (var item in itemsDelCarrito)
            item.Producto = await _productos.ObtenerPorIdAsync(item.ProductoId);

        carrito.Items = itemsDelCarrito;
    }
}
