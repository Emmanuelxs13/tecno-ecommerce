using TecnoEcommerce.Modelos.Enumeraciones;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación simulada del servicio de procesamiento de pagos.
/// Simula el comportamiento de una pasarela real (Stripe, PayPal, etc.) para
/// permitir el desarrollo y pruebas sin integración externa.
/// Principio OCP: cuando se integre una pasarela real, solo se cambia esta clase
/// (o se agrega una nueva implementación de <see cref="IPagoServicio"/>) sin
/// modificar ninguna otra capa del sistema.
/// Principio SRP: solo se encarga de simular el flujo de cobro y reembolso.
/// </summary>
public class PagoSimuladoServicio : IPagoServicio
{
    /// <summary>Logger para registrar los eventos de pago simulados.</summary>
    private readonly ILogger<PagoSimuladoServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public PagoSimuladoServicio(ILogger<PagoSimuladoServicio> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Simulación: aprueba el 90 % de los pagos de forma aleatoria para permitir
    /// probar ambos escenarios (aprobado/rechazado) durante el desarrollo.
    /// En producción esta lógica se reemplaza por la llamada a la API de la pasarela.
    /// </remarks>
    public Task<EstadoPago> ProcesarPagoAsync(int pedidoId, decimal monto)
    {
        _logger.LogInformation(
            "Simulando pago para pedido Id: {PedidoId}, monto: {Monto}", pedidoId, monto);

        // 90 % de probabilidad de aprobación para propósitos de desarrollo
        var aprobado = Random.Shared.NextDouble() >= 0.10;
        var estado   = aprobado ? EstadoPago.Aprobado : EstadoPago.Rechazado;

        _logger.LogInformation(
            "Resultado simulado del pago para pedido Id {PedidoId}: {Estado}", pedidoId, estado);

        return Task.FromResult(estado);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Simulación: siempre retorna <c>true</c> indicando que el reembolso fue exitoso.
    /// </remarks>
    public Task<bool> ReembolsarPagoAsync(int pedidoId)
    {
        _logger.LogInformation("Simulando reembolso para pedido Id: {PedidoId}", pedidoId);
        return Task.FromResult(true);
    }
}
