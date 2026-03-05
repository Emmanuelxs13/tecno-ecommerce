using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Datos.Contexto;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.Datos.Repositorios;

/// <summary>
/// Implementación EF Core de <see cref="ICarritoRepositorio"/>.
/// Carga ítems y productos en cada consulta del carrito del usuario
/// para que el servicio pueda construir el DTO sin llamadas adicionales.
/// </summary>
public sealed class CarritoRepositorioEfCore
    : RepositorioEfCore<Carrito>, ICarritoRepositorio
{
    public CarritoRepositorioEfCore(TiendaContexto contexto) : base(contexto) { }

    /// <inheritdoc/>
    public async Task<Carrito?> ObtenerPorUsuarioAsync(int usuarioId) =>
        await _contexto.Carritos
            .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

    /// <inheritdoc/>
    public async Task AgregarItemAsync(ItemCarrito item)
    {
        // Si el producto ya existe en el carrito solo incrementa la cantidad
        var existente = await _contexto.ItemsCarrito
            .FirstOrDefaultAsync(i => i.CarritoId == item.CarritoId
                                   && i.ProductoId == item.ProductoId);
        if (existente is not null)
        {
            existente.Cantidad += item.Cantidad;
            _contexto.ItemsCarrito.Update(existente);
        }
        else
        {
            await _contexto.ItemsCarrito.AddAsync(item);
        }

        await _contexto.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task EliminarItemAsync(int itemId)
    {
        var item = await _contexto.ItemsCarrito.FindAsync(itemId);
        if (item is not null)
        {
            _contexto.ItemsCarrito.Remove(item);
            await _contexto.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task VaciarCarritoAsync(int carritoId)
    {
        var items = await _contexto.ItemsCarrito
            .Where(i => i.CarritoId == carritoId)
            .ToListAsync();

        _contexto.ItemsCarrito.RemoveRange(items);
        await _contexto.SaveChangesAsync();
    }
}
