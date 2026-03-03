namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato genérico que define las operaciones CRUD estándar aplicables a cualquier entidad.
/// Principio OCP: nuevo comportamiento se añade extendiendo esta interfaz, sin modificarla.
/// Principio DIP: las capas superiores (Servicios, Controladores) dependen de esta abstracción
/// y nunca de implementaciones concretas como EF Core o Dapper.
/// </summary>
/// <typeparam name="T">
/// Tipo de la entidad que gestiona el repositorio. Debe ser una clase de referencia.
/// </typeparam>
public interface IRepositorio<T> where T : class
{
    /// <summary>
    /// Obtiene todas las entidades persistidas en el almacén de datos.
    /// </summary>
    /// <returns>Colección enumerable con todas las entidades encontradas.</returns>
    Task<IEnumerable<T>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene una entidad específica por su identificador único.
    /// </summary>
    /// <param name="id">Clave primaria de la entidad a buscar.</param>
    /// <returns>La entidad encontrada, o <c>null</c> si no existe ningún registro con ese Id.</returns>
    Task<T?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Persiste una nueva entidad en el almacén de datos.
    /// </summary>
    /// <param name="entidad">Instancia de la entidad a agregar.</param>
    Task AgregarAsync(T entidad);

    /// <summary>
    /// Actualiza los datos de una entidad existente en el almacén de datos.
    /// </summary>
    /// <param name="entidad">Instancia de la entidad con los nuevos valores.</param>
    Task ActualizarAsync(T entidad);

    /// <summary>
    /// Elimina permanentemente una entidad por su identificador único.
    /// </summary>
    /// <param name="id">Clave primaria de la entidad a eliminar.</param>
    Task EliminarAsync(int id);
}
