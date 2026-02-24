using TecnoEcommerce.Application.DTOs;

namespace TecnoEcommerce.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de categorías
/// </summary>
public interface ICategoriaService
{
    Task<CategoriaDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto> CreateAsync(CrearCategoriaDto dto);
    Task UpdateAsync(Guid id, CrearCategoriaDto dto);
    Task DeleteAsync(Guid id);
}
