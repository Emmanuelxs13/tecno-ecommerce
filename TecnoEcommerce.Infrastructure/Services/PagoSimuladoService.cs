using TecnoEcommerce.Domain.Enums;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Infrastructure.Services;

/// <summary>
/// Implementación simulada del servicio de pago (Mock)
/// En producción se integraría con una pasarela de pago real
/// </summary>
public class PagoSimuladoService : IPago
{
    private EstadoPago _estadoActual = EstadoPago.PENDIENTE;

    /// <summary>
    /// Simula el procesamiento de un pago
    /// </summary>
    public async Task<bool> ProcesarPago(decimal monto)
    {
        // Simulación de procesamiento
        await Task.Delay(100);

        if (monto <= 0)
        {
            _estadoActual = EstadoPago.RECHAZADO;
            return false;
        }

        _estadoActual = EstadoPago.PROCESANDO;
        await Task.Delay(100);
        
        // Simulación: 95% de pagos exitosos
        var random = new Random();
        var exitoso = random.Next(100) < 95;

        _estadoActual = exitoso ? EstadoPago.APROBADO : EstadoPago.RECHAZADO;
        return exitoso;
    }

    /// <summary>
    /// Valida si un pago puede ser procesado
    /// </summary>
    public async Task<bool> ValidarPago()
    {
        await Task.Delay(50);
        // En un escenario real, validaría datos de tarjeta, límites, etc.
        return true;
    }

    /// <summary>
    /// Obtiene el estado actual del pago
    /// </summary>
    public async Task<EstadoPago> ObtenerEstado()
    {
        await Task.CompletedTask;
        return _estadoActual;
    }
}
