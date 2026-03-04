using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Enumeraciones;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación del servicio de gestión de pedidos.
/// Es el servicio de mayor responsabilidad en el sistema: coordina el carrito,
/// el inventario, el pago y el envío para crear un pedido completo.
/// Principio SRP: toda la orquestación del flujo de compra está en un único lugar.
/// Principio DIP: depende de interfaces; ninguna implementación concreta está acoplada aquí.
/// </summary>
public class PedidoServicio : IPedidoServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio de pedidos para persistir y consultar el historial.</summary>
    private readonly IPedidoRepositorio _pedidoRepositorio;

    /// <summary>Repositorio del carrito para leer ítems y vaciarlo tras la compra.</summary>
    private readonly ICarritoRepositorio _carritoRepositorio;

    /// <summary>Repositorio de productos para descontar stock al confirmar el pedido.</summary>
    private readonly IProductoRepositorio _productoRepositorio;

    /// <summary>Servicio de pago; procesa el cobro antes de generar el envío.</summary>
    private readonly IPagoServicio _pagoServicio;

    /// <summary>Servicio de envío; genera el registro logístico cuando el pago es aprobado.</summary>
    private readonly IEnvioServicio _envioServicio;

    /// <summary>Logger para registrar cada etapa del flujo de compra.</summary>
    private readonly ILogger<PedidoServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con todas las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public PedidoServicio(
        IPedidoRepositorio pedidoRepositorio,
        ICarritoRepositorio carritoRepositorio,
        IProductoRepositorio productoRepositorio,
        IPagoServicio pagoServicio,
        IEnvioServicio envioServicio,
        ILogger<PedidoServicio> logger)
    {
        _pedidoRepositorio   = pedidoRepositorio;
        _carritoRepositorio  = carritoRepositorio;
        _productoRepositorio = productoRepositorio;
        _pagoServicio        = pagoServicio;
        _envioServicio       = envioServicio;
        _logger              = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<PedidoDto> CrearPedidoAsync(int usuarioId, CrearPedidoDto dto)
    {
        _logger.LogInformation("Iniciando creación de pedido para usuario Id: {UsuarioId}", usuarioId);

        // ── Paso 1: Verificar que el carrito no esté vacío ────────────────────
        var carrito = await _carritoRepositorio.ObtenerPorUsuarioAsync(usuarioId)
            ?? throw new InvalidOperationException("El usuario no tiene carrito activo.");

        if (!carrito.Items.Any())
            throw new InvalidOperationException("El carrito está vacío. Agrega productos antes de confirmar.");

        // ── Paso 2: Validar stock y construir los detalles ────────────────────
        var detalles = new List<DetallePedido>();
        decimal total = 0;

        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepositorio.ObtenerPorIdAsync(item.ProductoId)
                ?? throw new InvalidOperationException($"Producto Id {item.ProductoId} no encontrado.");

            if (producto.Stock < item.Cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}.");

            detalles.Add(new DetallePedido
            {
                ProductoId     = item.ProductoId,
                Cantidad       = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario  // Precio fijado en el momento del carrito
            });

            total += item.PrecioUnitario * item.Cantidad;
        }

        // ── Paso 3: Crear el pedido en estado Pendiente ───────────────────────
        var pedido = new Pedido
        {
            UsuarioId    = usuarioId,
            Total        = total,
            EstadoPedido = EstadoPedido.Pendiente,
            EstadoPago   = EstadoPago.Pendiente,
            Detalles     = detalles
        };

        await _pedidoRepositorio.AgregarAsync(pedido);
        _logger.LogInformation("Pedido Id {PedidoId} creado con total {Total}.", pedido.Id, total);

        // ── Paso 4: Descontar stock de cada producto ──────────────────────────
        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepositorio.ObtenerPorIdAsync(item.ProductoId)!;
            producto!.Stock -= item.Cantidad;
            await _productoRepositorio.ActualizarAsync(producto);
        }

        // ── Paso 5: Procesar el pago ──────────────────────────────────────────
        var estadoPago = await _pagoServicio.ProcesarPagoAsync(pedido.Id, total);
        pedido.EstadoPago = estadoPago;

        if (estadoPago == EstadoPago.Aprobado)
        {
            _logger.LogInformation("Pago aprobado para pedido Id: {PedidoId}", pedido.Id);
            pedido.EstadoPedido = EstadoPedido.Procesando;

            // ── Paso 6: Generar el envío ──────────────────────────────────────
            await _envioServicio.CrearEnvioAsync(pedido.Id, dto.DireccionEntrega);
            _logger.LogInformation("Envío generado para pedido Id: {PedidoId}", pedido.Id);

            // ── Paso 7: Vaciar el carrito ─────────────────────────────────────
            await _carritoRepositorio.VaciarCarritoAsync(carrito.Id);
            _logger.LogInformation("Carrito vaciado tras confirmación del pedido Id: {PedidoId}", pedido.Id);
        }
        else
        {
            // Pago rechazado: revertir el stock descontado
            _logger.LogWarning("Pago rechazado para pedido Id: {PedidoId}. Revirtiendo stock.", pedido.Id);
            foreach (var item in carrito.Items)
            {
                var producto = await _productoRepositorio.ObtenerPorIdAsync(item.ProductoId)!;
                producto!.Stock += item.Cantidad;
                await _productoRepositorio.ActualizarAsync(producto);
            }
        }

        // Persistir el estado final del pedido
        await _pedidoRepositorio.ActualizarAsync(pedido);

        // Recargar el pedido completo con todos sus detalles y envío para la respuesta
        var pedidoCompleto = await _pedidoRepositorio.ObtenerConDetallesAsync(pedido.Id);
        return MapearADto(pedidoCompleto!);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PedidoDto>> ObtenerMisPedidosAsync(int usuarioId)
    {
        var pedidos = await _pedidoRepositorio.ObtenerPorUsuarioAsync(usuarioId);
        return pedidos.Select(MapearADto);
    }

    /// <inheritdoc/>
    public async Task<PedidoDto?> ObtenerPorIdAsync(int pedidoId, int usuarioId)
    {
        var pedido = await _pedidoRepositorio.ObtenerConDetallesAsync(pedidoId);

        // Verificar que el pedido existe y pertenece al usuario solicitante
        if (pedido is null || pedido.UsuarioId != usuarioId) return null;

        return MapearADto(pedido);
    }

    // ── Métodos privados ───────────────────────────────────────────────────────

    /// <summary>
    /// Mapea una entidad <see cref="Pedido"/> al DTO de respuesta incluyendo detalles y envío.
    /// </summary>
    private static PedidoDto MapearADto(Pedido pedido) => new()
    {
        Id               = pedido.Id,
        FechaCreacion    = pedido.FechaCreacion,
        Total            = pedido.Total,
        EstadoPedido     = pedido.EstadoPedido,
        EstadoPago       = pedido.EstadoPago,
        EstadoEnvio      = pedido.Envio?.EstadoEnvio,
        DireccionEntrega = pedido.Envio?.Direccion ?? string.Empty,
        Detalles         = pedido.Detalles.Select(d => new DetallePedidoDto
        {
            Id              = d.Id,
            ProductoId      = d.ProductoId,
            NombreProducto  = d.Producto?.Nombre ?? string.Empty,
            PrecioUnitario  = d.PrecioUnitario,
            Cantidad        = d.Cantidad
        }).ToList()
    };
}
