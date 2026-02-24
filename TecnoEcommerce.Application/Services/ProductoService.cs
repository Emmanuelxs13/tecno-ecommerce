using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Application.Services;

/// <summary>
/// Servicio para gestionar las operaciones relacionadas con productos
/// </summary>
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<ProductoDto?> GetByIdAsync(Guid id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        return producto == null ? null : MapToDto(producto);
    }

    public async Task<IEnumerable<ProductoDto>> GetAllAsync()
    {
        var productos = await _productoRepository.GetAllAsync();
        return productos.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(Guid categoriaId)
    {
        var productos = await _productoRepository.GetByCategoriaAsync(categoriaId);
        return productos.Select(MapToDto);
    }

    public async Task<ProductoDto> CreateAsync(CrearProductoDto dto)
    {
        var producto = Producto.Crear(dto.Nombre, dto.Descripcion, dto.Precio, dto.Stock, dto.CategoriaId);
        var productoCreado = await _productoRepository.AddAsync(producto);
        return MapToDto(productoCreado);
    }

    public async Task UpdateAsync(Guid id, ActualizarProductoDto dto)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        if (producto == null)
            throw new InvalidOperationException("Producto no encontrado");

        producto.Editar(dto.Nombre, dto.Descripcion, dto.Precio, dto.CategoriaId);
        await _productoRepository.UpdateAsync(producto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productoRepository.DeleteAsync(id);
    }

    private static ProductoDto MapToDto(Producto producto)
    {
        return new ProductoDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Stock = producto.Stock,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria?.Nombre
        };
    }
}
