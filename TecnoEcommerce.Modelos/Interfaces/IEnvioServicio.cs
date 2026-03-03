using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato que define las operaciones del servicio de gestión de envíos.
/// Principio DIP: el <c>PedidoServicio</c> y los controladores dependen de esta abstracción;
/// la implementación concreta (<c>EnvioSimuladoServicio</c>) puede sustituirse por
/// una API de logística real (DHL, FedEx, etc.) sin modificar ninguna capa superior.
/// Principio OCP: abierto a extensión mediante nuevas implementaciones, cerrado a modificación.
/// </summary>
public interface IEnvioServicio
{
    /// <summary>
    /// Crea y registra un nuevo envío asociado al pedido indicado con la dirección de entrega.
    /// Se llama inmediatamente después de que el pago es aprobado.
    /// </summary>
    /// <param name="pedidoId">Identificador del pedido para el cual se genera el envío.</param>
    /// <param name="direccion">Dirección completa de entrega proporcionada por el cliente.</param>
    /// <returns>El registro de <see cref="Envio"/> recién creado con su estado inicial.</returns>
    Task<Envio> CrearEnvioAsync(int pedidoId, string direccion);

    /// <summary>
    /// Actualiza el estado de un envío existente dentro de su ciclo logístico.
    /// </summary>
    /// <param name="envioId">Identificador del envío cuyo estado se desea actualizar.</param>
    /// <param name="nuevoEstado">Nuevo estado del envío según <see cref="EstadoEnvio"/>.</param>
    Task ActualizarEstadoAsync(int envioId, EstadoEnvio nuevoEstado);

    /// <summary>
    /// Consulta el estado actual de un envío por su identificador.
    /// </summary>
    /// <param name="envioId">Identificador del envío a consultar.</param>
    /// <returns>El registro de <see cref="Envio"/> con su estado actualizado, o <c>null</c> si no existe.</returns>
    Task<Envio?> ConsultarEstadoAsync(int envioId);
}
