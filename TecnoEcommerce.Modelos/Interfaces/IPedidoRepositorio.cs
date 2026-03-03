using TecnoEcommerce.Modelos.Entidades;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Extiende el repositorio genérico con consultas específicas para la gestión de pedidos.
/// Principio ISP: expone solo las operaciones que el <c>PedidoServicio</c> requiere
/// para consultar el historial y administrar el estado de los pedidos.
/// </summary>
public interface IPedidoRepositorio : IRepositorio<Pedido>
{
    /// <summary>
    /// Obtiene todos los pedidos de un usuario específico, ordenados del más reciente al más antiguo.
    /// Incluye los detalles y los datos del envío para construir la vista de historial.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario cuyo historial se desea consultar.</param>
    /// <returns>Colección de pedidos con sus detalles y envío cargados; vacía si no hay pedidos.</returns>
    Task<IEnumerable<Pedido>> ObtenerPorUsuarioAsync(int usuarioId);

    /// <summary>
    /// Obtiene un pedido específico incluyendo sus detalles, productos y envío (carga completa).
    /// Se usa para mostrar la vista detallada de un pedido individual.
    /// </summary>
    /// <param name="pedidoId">Identificador del pedido a obtener.</param>
    /// <returns>El pedido con toda su información relacionada, o <c>null</c> si no existe.</returns>
    Task<Pedido?> ObtenerConDetallesAsync(int pedidoId);
}
