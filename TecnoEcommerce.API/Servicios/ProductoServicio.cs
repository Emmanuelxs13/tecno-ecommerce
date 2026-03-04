using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación del servicio de catálogo de productos.
/// Principio SRP: gestiona exclusivamente las operaciones sobre el catálogo.
/// Principio DIP: depende de <see cref="IProductoRepositorio"/> y <see cref="IRepositorio{Categoria}"/>.
/// Principio OCP: nuevos comportamientos (ej. paginación) se agregan sin modificar esta clase.
/// </summary>
public class ProductoServicio : IProductoServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio específico de productos con consultas de catálogo.</summary>
    private readonly IProductoRepositorio _productoRepositorio;

    /// <summary>Repositorio de categorías; usado para validar que la categoría exista al crear/editar.</summary>
    private readonly IRepositorio<Categoria> _categoriaRepositorio;

    /// <summary>Logger para registrar eventos del servicio.</summary>
    private readonly ILogger<ProductoServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public ProductoServicio(
        IProductoRepositorio productoRepositorio,
        IRepositorio<Categoria> categoriaRepositorio,
        ILogger<ProductoServicio> logger)
    {
        _productoRepositorio  = productoRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
        _logger               = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _productoRepositorio.ObtenerTodosAsync();
        return productos.Select(MapearADto);
    }

    /// <inheritdoc/>
    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        return producto is null ? null : MapearADto(producto);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductoDto>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        var productos = await _productoRepositorio.ObtenerPorCategoriaAsync(categoriaId);
        return productos.Select(MapearADto);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductoDto>> BuscarPorNombreAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return await ObtenerTodosAsync();

        var productos = await _productoRepositorio.BuscarPorNombreAsync(nombre);
        return productos.Select(MapearADto);
    }

    /// <inheritdoc/>
    public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
    {
        // Validaciones de negocio
        if (dto.Precio <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a cero.");

        if (dto.Stock < 0)
            throw new InvalidOperationException("El stock no puede ser negativo.");

        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(dto.CategoriaId)
            ?? throw new InvalidOperationException($"No existe la categoría con Id {dto.CategoriaId}.");

        var producto = new Producto
        {
            Nombre      = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio      = dto.Precio,
            Stock       = dto.Stock,
            ImagenUrl   = dto.ImagenUrl,
            CategoriaId = dto.CategoriaId,
            Categoria   = categoria
        };

        await _productoRepositorio.AgregarAsync(producto);
        _logger.LogInformation("Producto creado con Id: {Id}", producto.Id);

        return MapearADto(producto);
    }

    /// <inheritdoc/>
    public async Task<ProductoDto?> ActualizarAsync(int id, CrearProductoDto dto)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        if (producto is null) return null;

        if (dto.Precio <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a cero.");

        if (dto.Stock < 0)
            throw new InvalidOperationException("El stock no puede ser negativo.");

        // Validar que la nueva categoría exista
        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(dto.CategoriaId)
            ?? throw new InvalidOperationException($"No existe la categoría con Id {dto.CategoriaId}.");

        // Actualizar los campos de la entidad
        producto.Nombre      = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Precio      = dto.Precio;
        producto.Stock       = dto.Stock;
        producto.ImagenUrl   = dto.ImagenUrl;
        producto.CategoriaId = dto.CategoriaId;
        producto.Categoria   = categoria;

        await _productoRepositorio.ActualizarAsync(producto);
        _logger.LogInformation("Producto Id {Id} actualizado.", id);

        return MapearADto(producto);
    }

    /// <inheritdoc/>
    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        if (producto is null) return false;

        await _productoRepositorio.EliminarAsync(id);
        _logger.LogInformation("Producto Id {Id} eliminado.", id);
        return true;
    }

    // ── Métodos privados ───────────────────────────────────────────────────────

    /// <summary>
    /// Mapea una entidad <see cref="Producto"/> al DTO de respuesta.
    /// Centraliza el mapeo para evitar duplicación (DRY).
    /// </summary>
    private static ProductoDto MapearADto(Producto p) => new()
    {
        Id               = p.Id,
        Nombre           = p.Nombre,
        Descripcion      = p.Descripcion,
        Precio           = p.Precio,
        Stock            = p.Stock,
        ImagenUrl        = p.ImagenUrl,
        CategoriaId      = p.CategoriaId,
        // Si la navegación está cargada se usa el nombre; de lo contrario cadena vacía.
        NombreCategoria  = p.Categoria?.Nombre ?? string.Empty
    };
}
