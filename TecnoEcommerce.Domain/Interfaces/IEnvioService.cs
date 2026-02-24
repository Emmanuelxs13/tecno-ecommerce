using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de envíos
/// </summary>
public interface IEnvioService
{
    /// <summary>
    /// Genera un envío para un pedido
    /// </summary>
    Task<Envio> GenerarEnvio(Pedido pedido);

    /// <summary>
    /// Actualiza el estado de un envío
    /// </summary>
    Task ActualizarEstado(Guid envioId, EstadoEnvio estado);

    /// <summary>
    /// Rastrea un envío por su ID
    /// </summary>
    Task<InfoRastreo> RastrearEnvio(Guid envioId);
}
