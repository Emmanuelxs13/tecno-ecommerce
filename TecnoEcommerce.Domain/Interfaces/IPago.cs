using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz para el servicio de procesamiento de pagos
/// </summary>
public interface IPago
{
    /// <summary>
    /// Procesa un pago con el monto especificado
    /// </summary>
    Task<bool> ProcesarPago(decimal monto);

    /// <summary>
    /// Valida si un pago puede ser procesado
    /// </summary>
    Task<bool> ValidarPago();

    /// <summary>
    /// Obtiene el estado actual del pago
    /// </summary>
    Task<EstadoPago> ObtenerEstado();
}
