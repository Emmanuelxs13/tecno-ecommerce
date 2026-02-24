using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Enums;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Application.Services;

/// <summary>
/// Servicio para gestionar las operaciones relacionadas con pedidos
/// </summary>
public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ICarritoRepository _carritoRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IPago _pagoService;
    private readonly IEnvioService _envioService;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        ICarritoRepository carritoRepository,
        IProductoRepository productoRepository,
        IPago pagoService,
        IEnvioService envioService)
    {
        _pedidoRepository = pedidoRepository;
        _carritoRepository = carritoRepository;
        _productoRepository = productoRepository;
        _pagoService = pagoService;
        _envioService = envioService;
    }

    public async Task<PedidoDto?> GetByIdAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        return pedido == null ? null : await MapToDtoAsync(pedido);
    }

    public async Task<IEnumerable<PedidoDto>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var pedidos = await _pedidoRepository.GetByUsuarioIdAsync(usuarioId);
        var pedidosDto = new List<PedidoDto>();
        
        foreach (var pedido in pedidos)
        {
            pedidosDto.Add(await MapToDtoAsync(pedido));
        }

        return pedidosDto;
    }

    public async Task<PedidoDto> CrearPedidoDesdeCarritoAsync(CrearPedidoDto dto)
    {
        // Obtener el carrito del usuario
        var carrito = await _carritoRepository.GetByUsuarioIdAsync(dto.UsuarioId);
        if (carrito == null || !carrito.Items.Any())
            throw new InvalidOperationException("El carrito está vacío");

        // Crear el pedido
        var pedido = Pedido.Crear(dto.UsuarioId, dto.DireccionEnvio);

        // Agregar los items del carrito al pedido y actualizar stock
        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);
            if (producto == null)
                throw new InvalidOperationException($"Producto {item.ProductoId} no encontrado");

            if (!producto.VerificarDisponibilidad(item.Cantidad))
                throw new InvalidOperationException($"Stock insuficiente para {producto.Nombre}");

            pedido.AgregarDetalle(item.ProductoId, item.Cantidad, item.PrecioUnitario);
            producto.ActualizarStock(-item.Cantidad);
            await _productoRepository.UpdateAsync(producto);
        }

        // Procesar pago
        var pagoExitoso = await _pagoService.ProcesarPago(pedido.Total);
        if (pagoExitoso)
        {
            pedido.CambiarEstado(EstadoPedido.PAGADO);
            
            // Generar envío
            await _envioService.GenerarEnvio(pedido);
        }

        var pedidoCreado = await _pedidoRepository.AddAsync(pedido);

        // Vaciar el carrito
        carrito.VaciarCarrito();
        await _carritoRepository.UpdateAsync(carrito);

        return await MapToDtoAsync(pedidoCreado);
    }

    public async Task CambiarEstadoAsync(Guid pedidoId, EstadoPedido nuevoEstado)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
        if (pedido == null)
            throw new InvalidOperationException("Pedido no encontrado");

        pedido.CambiarEstado(nuevoEstado);
        await _pedidoRepository.UpdateAsync(pedido);
    }

    public async Task CancelarPedidoAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
        if (pedido == null)
            throw new InvalidOperationException("Pedido no encontrado");

        pedido.Cancelar();

        // Devolver stock
        foreach (var detalle in pedido.Detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
            if (producto != null)
            {
                producto.ActualizarStock(detalle.Cantidad);
                await _productoRepository.UpdateAsync(producto);
            }
        }

        await _pedidoRepository.UpdateAsync(pedido);
    }

    private async Task<PedidoDto> MapToDtoAsync(Pedido pedido)
    {
        var detallesDto = new List<DetallePedidoDto>();
        
        foreach (var detalle in pedido.Detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
            detallesDto.Add(new DetallePedidoDto
            {
                Id = detalle.Id,
                ProductoId = detalle.ProductoId,
                ProductoNombre = producto?.Nombre ?? "Producto no disponible",
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio,
                Subtotal = detalle.CalcularSubtotal()
            });
        }

        return new PedidoDto
        {
            Id = pedido.Id,
            UsuarioId = pedido.UsuarioId,
            Total = pedido.Total,
            Estado = pedido.Estado,
            Fecha = pedido.Fecha,
            DireccionEnvio = pedido.DireccionEnvio,
            Detalles = detallesDto
        };
    }
}
