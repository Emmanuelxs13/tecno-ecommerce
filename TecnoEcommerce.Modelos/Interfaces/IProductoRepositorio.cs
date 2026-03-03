using TecnoEcommerce.Modelos.Entidades;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Extiende el repositorio genérico con consultas específicas del catálogo de productos.
/// Principio ISP: define únicamente los métodos que los consumidores de productos necesitan,
/// sin obligarlos a depender de operaciones que no van a usar.
/// Principio LSP: cualquier implementación concreta debe poder sustituir a este contrato
/// sin alterar el comportamiento esperado por el servicio que lo consuma.
/// </summary>
public interface IProductoRepositorio : IRepositorio<Producto>
{
    /// <summary>
    /// Obtiene todos los productos que pertenecen a una categoría específica.
    /// </summary>
    /// <param name="categoriaId">Identificador de la categoría por la que filtrar.</param>
    /// <returns>Colección de productos de esa categoría; vacía si no hay resultados.</returns>
    Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId);

    /// <summary>
    /// Busca productos cuyo nombre contenga el término indicado (búsqueda parcial, case-insensitive).
    /// </summary>
    /// <param name="nombre">Texto parcial o completo del nombre del producto.</param>
    /// <returns>Colección de productos que coinciden con el criterio; vacía si no hay resultados.</returns>
    Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);

    /// <summary>
    /// Obtiene únicamente los productos con stock disponible (Stock mayor a cero).
    /// </summary>
    /// <returns>Colección de productos con existencias en inventario.</returns>
    Task<IEnumerable<Producto>> ObtenerConStockAsync();
}
