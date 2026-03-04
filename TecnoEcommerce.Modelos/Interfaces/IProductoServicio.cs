using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato del servicio de catálogo de productos.
/// Principio DIP: los controladores dependen de esta abstracción.
/// Principio SRP: centraliza todos los casos de uso relacionados con el catálogo.
/// </summary>
public interface IProductoServicio
{
    /// <summary>Obtiene todos los productos del catálogo.</summary>
    /// <returns>Colección con todos los productos disponibles.</returns>
    Task<IEnumerable<ProductoDto>> ObtenerTodosAsync();

    /// <summary>Obtiene un producto específico por su identificador.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>El producto encontrado, o <c>null</c> si no existe.</returns>
    Task<ProductoDto?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Filtra los productos de una categoría específica.
    /// </summary>
    /// <param name="categoriaId">Identificador de la categoría a filtrar.</param>
    /// <returns>Colección de productos de esa categoría.</returns>
    Task<IEnumerable<ProductoDto>> ObtenerPorCategoriaAsync(int categoriaId);

    /// <summary>
    /// Busca productos cuyo nombre contenga el texto indicado (búsqueda parcial).
    /// </summary>
    /// <param name="nombre">Término de búsqueda.</param>
    /// <returns>Colección de productos que coinciden con el criterio.</returns>
    Task<IEnumerable<ProductoDto>> BuscarPorNombreAsync(string nombre);

    /// <summary>
    /// Crea un nuevo producto en el catálogo (solo Administrador).
    /// Valida que el precio sea positivo y que la categoría exista.
    /// </summary>
    /// <param name="dto">Datos del producto a crear.</param>
    /// <returns>El producto recién creado con su Id asignado.</returns>
    Task<ProductoDto> CrearAsync(CrearProductoDto dto);

    /// <summary>
    /// Actualiza los datos de un producto existente (solo Administrador).
    /// </summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="dto">Nuevos datos del producto.</param>
    /// <returns>El producto actualizado, o <c>null</c> si no existe.</returns>
    Task<ProductoDto?> ActualizarAsync(int id, CrearProductoDto dto);

    /// <summary>
    /// Elimina un producto del catálogo (solo Administrador).
    /// </summary>
    /// <param name="id">Identificador del producto a eliminar.</param>
    /// <returns><c>true</c> si fue eliminado; <c>false</c> si no existía.</returns>
    Task<bool> EliminarAsync(int id);
}
