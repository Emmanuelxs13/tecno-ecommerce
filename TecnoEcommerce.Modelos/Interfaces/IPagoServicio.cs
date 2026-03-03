using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato que define las operaciones del servicio de procesamiento de pagos.
/// Principio DIP: el <c>PedidoServicio</c> y los controladores dependen de esta abstracción;
/// la implementación concreta (<c>PagoSimuladoServicio</c>) puede sustituirse por una
/// pasarela real (Stripe, PayPal, etc.) sin modificar ninguna capa superior.
/// Principio OCP: abierto a extensión mediante nuevas implementaciones, cerrado a modificación.
/// </summary>
public interface IPagoServicio
{
    /// <summary>
    /// Procesa el pago de un pedido por el monto especificado.
    /// </summary>
    /// <param name="pedidoId">Identificador del pedido al que corresponde el pago.</param>
    /// <param name="monto">
    /// Monto total a cobrar en la moneda configurada en el sistema.
    /// Se usa <c>decimal</c> para evitar errores de redondeo.
    /// </param>
    /// <returns>
    /// El estado resultante del pago (<see cref="EstadoPago.Aprobado"/> o <see cref="EstadoPago.Rechazado"/>).
    /// </returns>
    Task<EstadoPago> ProcesarPagoAsync(int pedidoId, decimal monto);

    /// <summary>
    /// Emite un reembolso sobre un pago previamente aprobado.
    /// </summary>
    /// <param name="pedidoId">Identificador del pedido cuyo pago se desea reembolsar.</param>
    /// <returns><c>true</c> si el reembolso fue procesado exitosamente; <c>false</c> en caso contrario.</returns>
    Task<bool> ReembolsarPagoAsync(int pedidoId);
}
