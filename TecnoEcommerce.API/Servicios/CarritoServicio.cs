using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación del servicio de gestión del carrito de compras.
/// Principio SRP: coordina exclusivamente las operaciones del carrito,
/// incluyendo validación de stock antes de permitir agregar ítems.
/// Principio DIP: depende de <see cref="ICarritoRepositorio"/> y <see cref="IProductoRepositorio"/>.
/// </summary>
public class CarritoServicio : ICarritoServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio del carrito con operaciones específicas de ítems.</summary>
    private readonly ICarritoRepositorio _carritoRepositorio;

    /// <summary>Repositorio de productos; usado para validar existencia y stock.</summary>
    private readonly IProductoRepositorio _productoRepositorio;

    /// <summary>Logger para registrar eventos del servicio.</summary>
    private readonly ILogger<CarritoServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public CarritoServicio(
        ICarritoRepositorio carritoRepositorio,
        IProductoRepositorio productoRepositorio,
        ILogger<CarritoServicio> logger)
    {
        _carritoRepositorio  = carritoRepositorio;
        _productoRepositorio = productoRepositorio;
        _logger              = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<CarritoDto> ObtenerCarritoAsync(int usuarioId)
    {
        var carrito = await _carritoRepositorio.ObtenerPorUsuarioAsync(usuarioId);

        // Si el usuario no tiene carrito aún, crear uno vacío
        if (carrito is null)
        {
            carrito = new Carrito { UsuarioId = usuarioId };
            await _carritoRepositorio.AgregarAsync(carrito);
            _logger.LogInformation("Carrito creado automáticamente para usuario Id: {UsuarioId}", usuarioId);
        }

        return MapearADto(carrito);
    }

    /// <inheritdoc/>
    public async Task<CarritoDto> AgregarItemAsync(int usuarioId, AgregarItemCarritoDto dto)
    {
        if (dto.Cantidad <= 0)
            throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

        // Validar que el producto exista y tenga stock
        var producto = await _productoRepositorio.ObtenerPorIdAsync(dto.ProductoId)
            ?? throw new InvalidOperationException($"El producto con Id {dto.ProductoId} no existe.");

        if (producto.Stock < dto.Cantidad)
            throw new InvalidOperationException(
                $"Stock insuficiente. Disponible: {producto.Stock}, solicitado: {dto.Cantidad}.");

        // Obtener o crear el carrito
        var carrito = await _carritoRepositorio.ObtenerPorUsuarioAsync(usuarioId);
        if (carrito is null)
        {
            carrito = new Carrito { UsuarioId = usuarioId };
            await _carritoRepositorio.AgregarAsync(carrito);
        }

        // Verificar si el producto ya existe en el carrito para sumar la cantidad
        var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);
        if (itemExistente is not null)
        {
            itemExistente.Cantidad += dto.Cantidad;
            await _carritoRepositorio.ActualizarAsync(carrito);
        }
        else
        {
            var nuevoItem = new ItemCarrito
            {
                CarritoId      = carrito.Id,
                ProductoId     = dto.ProductoId,
                Cantidad       = dto.Cantidad,
                PrecioUnitario = producto.Precio  // Captura del precio en el momento de agregar
            };
            await _carritoRepositorio.AgregarItemAsync(nuevoItem);
        }

        _logger.LogInformation("Producto Id {ProductoId} agregado al carrito del usuario Id {UsuarioId}.",
            dto.ProductoId, usuarioId);

        // Retornar el carrito actualizado
        return await ObtenerCarritoAsync(usuarioId);
    }

    /// <inheritdoc/>
    public async Task<CarritoDto> EliminarItemAsync(int usuarioId, int itemId)
    {
        // Verificar que el ítem pertenece al carrito del usuario
        var carrito = await _carritoRepositorio.ObtenerPorUsuarioAsync(usuarioId)
            ?? throw new InvalidOperationException("El usuario no tiene carrito activo.");

        var itemPertenece = carrito.Items.Any(i => i.Id == itemId);
        if (!itemPertenece)
            throw new InvalidOperationException("El ítem no pertenece al carrito del usuario.");

        await _carritoRepositorio.EliminarItemAsync(itemId);
        _logger.LogInformation("Ítem Id {ItemId} eliminado del carrito del usuario Id {UsuarioId}.", itemId, usuarioId);

        return await ObtenerCarritoAsync(usuarioId);
    }

    /// <inheritdoc/>
    public async Task VaciarCarritoAsync(int usuarioId)
    {
        var carrito = await _carritoRepositorio.ObtenerPorUsuarioAsync(usuarioId);
        if (carrito is null) return;

        await _carritoRepositorio.VaciarCarritoAsync(carrito.Id);
        _logger.LogInformation("Carrito del usuario Id {UsuarioId} vaciado.", usuarioId);
    }

    // ── Métodos privados ───────────────────────────────────────────────────────

    /// <summary>
    /// Mapea una entidad <see cref="Carrito"/> al DTO de respuesta incluyendo sus ítems.
    /// </summary>
    private static CarritoDto MapearADto(Carrito carrito) => new()
    {
        Id        = carrito.Id,
        UsuarioId = carrito.UsuarioId,
        Items     = carrito.Items.Select(MapearItemADto).ToList()
    };

    /// <summary>
    /// Mapea un <see cref="ItemCarrito"/> al DTO de respuesta.
    /// Si la navegación al producto está cargada incluye nombre e imagen.
    /// </summary>
    private static ItemCarritoDto MapearItemADto(ItemCarrito item) => new()
    {
        Id              = item.Id,
        ProductoId      = item.ProductoId,
        NombreProducto  = item.Producto?.Nombre    ?? string.Empty,
        ImagenUrl       = item.Producto?.ImagenUrl ?? string.Empty,
        PrecioUnitario  = item.PrecioUnitario,
        Cantidad        = item.Cantidad
    };
}
