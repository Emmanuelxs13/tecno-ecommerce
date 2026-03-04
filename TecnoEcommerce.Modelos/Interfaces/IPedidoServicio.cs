using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato del servicio de gestión de pedidos.
/// Principio DIP: los controladores dependen de esta abstracción.
/// Principio SRP: centraliza el flujo completo de creación de pedidos:
/// validar carrito → descontar stock → procesar pago → generar envío.
/// </summary>
public interface IPedidoServicio
{
    /// <summary>
    /// Crea un pedido a partir del carrito activo del usuario.
    /// El flujo interno es:
    /// <list type="number">
    ///   <item>Verificar que el carrito no esté vacío.</item>
    ///   <item>Calcular el total.</item>
    ///   <item>Descontar el stock de cada producto.</item>
    ///   <item>Persistir el pedido y sus detalles.</item>
    ///   <item>Invocar <see cref="IPagoServicio"/> para procesar el pago.</item>
    ///   <item>Si el pago es aprobado, invocar <see cref="IEnvioServicio"/> para generar el envío.</item>
    ///   <item>Vaciar el carrito.</item>
    /// </list>
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario que realiza el pedido.</param>
    /// <param name="dto">Datos del pedido (dirección de entrega).</param>
    /// <returns>El pedido creado con todos sus detalles.</returns>
    /// <exception cref="InvalidOperationException">Si el carrito está vacío o no hay stock suficiente.</exception>
    Task<PedidoDto> CrearPedidoAsync(int usuarioId, CrearPedidoDto dto);

    /// <summary>
    /// Obtiene el historial completo de pedidos de un usuario, ordenado del más reciente al más antiguo.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    /// <returns>Colección de pedidos del usuario con sus detalles.</returns>
    Task<IEnumerable<PedidoDto>> ObtenerMisPedidosAsync(int usuarioId);

    /// <summary>
    /// Obtiene el detalle completo de un pedido específico.
    /// Valida que el pedido pertenezca al usuario solicitante.
    /// </summary>
    /// <param name="pedidoId">Identificador del pedido.</param>
    /// <param name="usuarioId">Identificador del usuario que realiza la consulta.</param>
    /// <returns>El pedido con todos sus detalles, o <c>null</c> si no existe o no pertenece al usuario.</returns>
    Task<PedidoDto?> ObtenerPorIdAsync(int pedidoId, int usuarioId);
}
