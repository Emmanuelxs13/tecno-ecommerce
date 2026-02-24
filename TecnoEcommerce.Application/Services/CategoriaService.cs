using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Application.Services;

/// <summary>
/// Servicio para gestionar las operaciones relacionadas con categorías
/// </summary>
public class CategoriaService : ICategoriaService
{
    private readonly IRepository<Categoria> _categoriaRepository;

    public CategoriaService(IRepository<Categoria> categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaDto?> GetByIdAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        return categoria == null ? null : MapToDto(categoria);
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias.Select(MapToDto);
    }

    public async Task<CategoriaDto> CreateAsync(CrearCategoriaDto dto)
    {
        var categoria = Categoria.Crear(dto.Nombre, dto.Descripcion);
        var categoriaCreada = await _categoriaRepository.AddAsync(categoria);
        return MapToDto(categoriaCreada);
    }

    public async Task UpdateAsync(Guid id, CrearCategoriaDto dto)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException("Categoría no encontrada");

        categoria.Editar(dto.Nombre, dto.Descripcion);
        await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _categoriaRepository.DeleteAsync(id);
    }

    private static CategoriaDto MapToDto(Categoria categoria)
    {
        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };
    }
}
