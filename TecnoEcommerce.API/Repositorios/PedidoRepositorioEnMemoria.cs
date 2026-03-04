using System.Collections.Concurrent;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Implementacion en memoria de <see cref="IPedidoRepositorio"/>.
/// Los detalles del pedido se extraen de la propia coleccion del Pedido al persistirlo.
/// Los envios se consultan desde un IRepositorio Envio compartido (singleton).
/// </summary>
public sealed class PedidoRepositorioEnMemoria
    : RepositorioEnMemoria<Pedido>, IPedidoRepositorio
{
    private readonly ConcurrentDictionary<int, DetallePedido> _detalles = new();
    private int _detalleSecuencia;
    private readonly IRepositorio<Envio>  _envios;
    private readonly IProductoRepositorio _productos;

    public PedidoRepositorioEnMemoria(
        IProductoRepositorio productos,
        IRepositorio<Envio>  envios)
        : base(p => p.Id, (p, id) => p.Id = id)
    {
        _productos = productos;
        _envios    = envios;
    }

    /// <summary>Persiste el pedido y extrae sus detalles a un diccionario separado.</summary>
    public override Task AgregarAsync(Pedido pedido)
    {
        base.AgregarAsync(pedido);
        foreach (var detalle in pedido.Detalles)
        {
            detalle.PedidoId = pedido.Id;
            var id = Interlocked.Increment(ref _detalleSecuencia);
            detalle.Id = id;
            _detalles[id] = detalle;
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pedido>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        var pedidos = _almacen.Values
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToList();
        foreach (var p in pedidos) await CargarRelacionesAsync(p);
        return pedidos;
    }

    /// <inheritdoc/>
    public async Task<Pedido?> ObtenerConDetallesAsync(int pedidoId)
    {
        if (!_almacen.TryGetValue(pedidoId, out var pedido)) return null;
        await CargarRelacionesAsync(pedido);
        return pedido;
    }

    private async Task CargarRelacionesAsync(Pedido pedido)
    {
        var detalles = _detalles.Values.Where(d => d.PedidoId == pedido.Id).ToList();
        foreach (var d in detalles)
            d.Producto = await _productos.ObtenerPorIdAsync(d.ProductoId);
        pedido.Detalles = detalles;

        var todosEnvios = await _envios.ObtenerTodosAsync();
        pedido.Envio = todosEnvios.FirstOrDefault(e => e.PedidoId == pedido.Id);
    }
}
