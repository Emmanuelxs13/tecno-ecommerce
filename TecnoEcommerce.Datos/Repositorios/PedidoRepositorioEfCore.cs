using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Datos.Contexto;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.Datos.Repositorios;

/// <summary>
/// Implementación EF Core de <see cref="IPedidoRepositorio"/>.
/// Carga detalles, productos y envío en cada consulta de pedido.
/// </summary>
public sealed class PedidoRepositorioEfCore
    : RepositorioEfCore<Pedido>, IPedidoRepositorio
{
    public PedidoRepositorioEfCore(TiendaContexto contexto) : base(contexto) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pedido>> ObtenerPorUsuarioAsync(int usuarioId) =>
        await _contexto.Pedidos
            .AsNoTracking()
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Envio)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Pedido?> ObtenerConDetallesAsync(int pedidoId) =>
        await _contexto.Pedidos
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Envio)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

    /// <inheritdoc/>
    public override async Task AgregarAsync(Pedido pedido)
    {
        await _contexto.Pedidos.AddAsync(pedido);
        await _contexto.SaveChangesAsync();
    }
}
