using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Application.Services;

/// <summary>
/// Servicio para gestionar las operaciones del carrito de compras
/// </summary>
public class CarritoService : ICarritoService
{
    private readonly ICarritoRepository _carritoRepository;
    private readonly IProductoRepository _productoRepository;

    public CarritoService(ICarritoRepository carritoRepository, IProductoRepository productoRepository)
    {
        _carritoRepository = carritoRepository;
        _productoRepository = productoRepository;
    }

    public async Task<CarritoDto?> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var carrito = await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
        return carrito == null ? null : await MapToDtoAsync(carrito);
    }

    public async Task<CarritoDto> AgregarItemAsync(Guid usuarioId, AgregarItemCarritoDto dto)
    {
        var producto = await _productoRepository.GetByIdAsync(dto.ProductoId);
        if (producto == null)
            throw new InvalidOperationException("Producto no encontrado");

        if (!producto.VerificarDisponibilidad(dto.Cantidad))
            throw new InvalidOperationException("Stock insuficiente");

        var carrito = await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
        
        if (carrito == null)
        {
            carrito = Carrito.Crear(usuarioId);
            carrito = await _carritoRepository.AddAsync(carrito);
        }

        carrito.AgregarItem(dto.ProductoId, dto.Cantidad, producto.Precio);
        await _carritoRepository.UpdateAsync(carrito);

        return await MapToDtoAsync(carrito);
    }

    public async Task<CarritoDto> ModificarItemAsync(Guid usuarioId, Guid itemId, int cantidad)
    {
        var carrito = await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
        if (carrito == null)
            throw new InvalidOperationException("Carrito no encontrado");

        carrito.ModificarItem(itemId, cantidad);
        await _carritoRepository.UpdateAsync(carrito);

        return await MapToDtoAsync(carrito);
    }

    public async Task EliminarItemAsync(Guid usuarioId, Guid itemId)
    {
        var carrito = await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
        if (carrito == null)
            throw new InvalidOperationException("Carrito no encontrado");

        carrito.EliminarItem(itemId);
        await _carritoRepository.UpdateAsync(carrito);
    }

    public async Task VaciarCarritoAsync(Guid usuarioId)
    {
        var carrito = await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
        if (carrito == null)
            throw new InvalidOperationException("Carrito no encontrado");

        carrito.VaciarCarrito();
        await _carritoRepository.UpdateAsync(carrito);
    }

    private async Task<CarritoDto> MapToDtoAsync(Carrito carrito)
    {
        var itemsDto = new List<ItemCarritoDto>();
        
        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);
            itemsDto.Add(new ItemCarritoDto
            {
                Id = item.Id,
                ProductoId = item.ProductoId,
                ProductoNombre = producto?.Nombre ?? "Producto no disponible",
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario,
                Subtotal = item.CalcularSubtotal()
            });
        }

        return new CarritoDto
        {
            Id = carrito.Id,
            UsuarioId = carrito.UsuarioId,
            Items = itemsDto,
            Total = carrito.CalcularTotal()
        };
    }
}
