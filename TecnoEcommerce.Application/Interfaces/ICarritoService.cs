using TecnoEcommerce.Application.DTOs;

namespace TecnoEcommerce.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión del carrito de compras
/// </summary>
public interface ICarritoService
{
    Task<CarritoDto?> GetByUsuarioIdAsync(Guid usuarioId);
    Task<CarritoDto> AgregarItemAsync(Guid usuarioId, AgregarItemCarritoDto dto);
    Task<CarritoDto> ModificarItemAsync(Guid usuarioId, Guid itemId, int cantidad);
    Task EliminarItemAsync(Guid usuarioId, Guid itemId);
    Task VaciarCarritoAsync(Guid usuarioId);
}
