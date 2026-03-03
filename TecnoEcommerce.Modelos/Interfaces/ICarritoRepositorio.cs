using TecnoEcommerce.Modelos.Entidades;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Extiende el repositorio genérico con operaciones específicas para el carrito de compras.
/// Principio ISP: define solo los métodos que el <c>CarritoServicio</c> necesita para
/// gestionar el estado del carrito, sin mezclar operaciones de otros contextos.
/// </summary>
public interface ICarritoRepositorio : IRepositorio<Carrito>
{
    /// <summary>
    /// Obtiene el carrito activo de un usuario incluyendo sus ítems y los datos de cada producto.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario propietario del carrito.</param>
    /// <returns>
    /// El carrito del usuario con sus ítems cargados (eager loading),
    /// o <c>null</c> si el usuario aún no tiene carrito.
    /// </returns>
    Task<Carrito?> ObtenerPorUsuarioAsync(int usuarioId);

    /// <summary>
    /// Agrega un nuevo ítem al carrito o incrementa la cantidad si el producto ya existe.
    /// </summary>
    /// <param name="item">Ítem a agregar o actualizar dentro del carrito.</param>
    Task AgregarItemAsync(ItemCarrito item);

    /// <summary>
    /// Elimina un ítem específico del carrito por su identificador.
    /// </summary>
    /// <param name="itemId">Identificador del ítem a eliminar.</param>
    Task EliminarItemAsync(int itemId);

    /// <summary>
    /// Elimina todos los ítems del carrito, dejándolo vacío sin borrar el carrito en sí.
    /// Se llama después de que el pedido es confirmado exitosamente.
    /// </summary>
    /// <param name="carritoId">Identificador del carrito a vaciar.</param>
    Task VaciarCarritoAsync(int carritoId);
}
