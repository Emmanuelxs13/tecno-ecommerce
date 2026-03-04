using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato del servicio de gestión de categorías.
/// Principio DIP: los controladores dependen de esta abstracción.
/// Principio SRP: centraliza todos los casos de uso relacionados con categorías.
/// </summary>
public interface ICategoriaServicio
{
    /// <summary>Obtiene todas las categorías registradas.</summary>
    /// <returns>Colección con todas las categorías, incluyendo el total de productos de cada una.</returns>
    Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync();

    /// <summary>Obtiene una categoría específica por su identificador.</summary>
    /// <param name="id">Identificador de la categoría.</param>
    /// <returns>La categoría encontrada, o <c>null</c> si no existe.</returns>
    Task<CategoriaDto?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea una nueva categoría (solo Administrador).
    /// Valida que no exista otra categoría con el mismo nombre.
    /// </summary>
    /// <param name="dto">Datos de la categoría a crear.</param>
    /// <returns>La categoría recién creada con su Id asignado.</returns>
    Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto);

    /// <summary>Actualiza los datos de una categoría existente (solo Administrador).</summary>
    /// <param name="id">Identificador de la categoría a actualizar.</param>
    /// <param name="dto">Nuevos datos de la categoría.</param>
    /// <returns>La categoría actualizada, o <c>null</c> si no existe.</returns>
    Task<CategoriaDto?> ActualizarAsync(int id, CrearCategoriaDto dto);

    /// <summary>
    /// Elimina una categoría (solo Administrador).
    /// No se puede eliminar si tiene productos asociados.
    /// </summary>
    /// <param name="id">Identificador de la categoría a eliminar.</param>
    /// <returns><c>true</c> si fue eliminada; <c>false</c> si no existía.</returns>
    /// <exception cref="InvalidOperationException">Si la categoría tiene productos asociados.</exception>
    Task<bool> EliminarAsync(int id);
}
