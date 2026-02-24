using TecnoEcommerce.Application.DTOs;

namespace TecnoEcommerce.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de productos
/// </summary>
public interface IProductoService
{
    Task<ProductoDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(Guid categoriaId);
    Task<ProductoDto> CreateAsync(CrearProductoDto dto);
    Task UpdateAsync(Guid id, ActualizarProductoDto dto);
    Task DeleteAsync(Guid id);
}
