using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación del servicio de gestión de categorías.
/// Principio SRP: gestiona exclusivamente las operaciones CRUD sobre categorías.
/// Principio DIP: depende de las abstracciones <see cref="IRepositorio{T}"/>.
/// </summary>
public class CategoriaServicio : ICategoriaServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio genérico de categorías.</summary>
    private readonly IRepositorio<Categoria> _categoriaRepositorio;

    /// <summary>Repositorio de productos; usado para contar productos por categoría y para
    /// impedir la eliminación de categorías que aún tienen productos asociados.</summary>
    private readonly IProductoRepositorio _productoRepositorio;

    /// <summary>Logger para registrar eventos del servicio.</summary>
    private readonly ILogger<CategoriaServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public CategoriaServicio(
        IRepositorio<Categoria> categoriaRepositorio,
        IProductoRepositorio productoRepositorio,
        ILogger<CategoriaServicio> logger)
    {
        _categoriaRepositorio = categoriaRepositorio;
        _productoRepositorio  = productoRepositorio;
        _logger               = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
    {
        var categorias = await _categoriaRepositorio.ObtenerTodosAsync();

        // Calcular el total de productos de cada categoría
        var dtos = new List<CategoriaDto>();
        foreach (var cat in categorias)
        {
            var productos = await _productoRepositorio.ObtenerPorCategoriaAsync(cat.Id);
            dtos.Add(MapearADto(cat, productos.Count()));
        }

        return dtos;
    }

    /// <inheritdoc/>
    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(id);
        if (categoria is null) return null;

        var productos = await _productoRepositorio.ObtenerPorCategoriaAsync(id);
        return MapearADto(categoria, productos.Count());
    }

    /// <inheritdoc/>
    public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto)
    {
        // Validar nombre único
        var existentes = await _categoriaRepositorio.ObtenerTodosAsync();
        if (existentes.Any(c => c.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Ya existe una categoría con el nombre '{dto.Nombre}'.");

        var categoria = new Categoria
        {
            Nombre      = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        await _categoriaRepositorio.AgregarAsync(categoria);
        _logger.LogInformation("Categoría creada con Id: {Id}", categoria.Id);

        return MapearADto(categoria, 0);
    }

    /// <inheritdoc/>
    public async Task<CategoriaDto?> ActualizarAsync(int id, CrearCategoriaDto dto)
    {
        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(id);
        if (categoria is null) return null;

        categoria.Nombre      = dto.Nombre;
        categoria.Descripcion = dto.Descripcion;

        await _categoriaRepositorio.ActualizarAsync(categoria);
        _logger.LogInformation("Categoría Id {Id} actualizada.", id);

        var productos = await _productoRepositorio.ObtenerPorCategoriaAsync(id);
        return MapearADto(categoria, productos.Count());
    }

    /// <inheritdoc/>
    public async Task<bool> EliminarAsync(int id)
    {
        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(id);
        if (categoria is null) return false;

        // Impedir eliminar si tiene productos asociados
        var productos = await _productoRepositorio.ObtenerPorCategoriaAsync(id);
        if (productos.Any())
            throw new InvalidOperationException(
                $"No se puede eliminar la categoría '{categoria.Nombre}' porque tiene productos asociados.");

        await _categoriaRepositorio.EliminarAsync(id);
        _logger.LogInformation("Categoría Id {Id} eliminada.", id);
        return true;
    }

    // ── Métodos privados ───────────────────────────────────────────────────────

    /// <summary>
    /// Mapea una entidad <see cref="Categoria"/> al DTO de respuesta.
    /// </summary>
    /// <param name="categoria">Entidad a mapear.</param>
    /// <param name="totalProductos">Cantidad de productos activos de la categoría.</param>
    private static CategoriaDto MapearADto(Categoria categoria, int totalProductos) => new()
    {
        Id             = categoria.Id,
        Nombre         = categoria.Nombre,
        Descripcion    = categoria.Descripcion,
        TotalProductos = totalProductos
    };
}
