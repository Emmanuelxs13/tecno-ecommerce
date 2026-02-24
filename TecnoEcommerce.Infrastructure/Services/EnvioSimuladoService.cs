using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Enums;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Infrastructure.Services;

/// <summary>
/// Implementación simulada del servicio de envío (Mock)
/// En producción se integraría con servicios de logística reales
/// </summary>
public class EnvioSimuladoService : IEnvioService
{
    private readonly Dictionary<Guid, Envio> _envios = new();

    /// <summary>
    /// Genera un envío para un pedido
    /// </summary>
    public async Task<Envio> GenerarEnvio(Pedido pedido)
    {
        await Task.Delay(100);

        var envio = new Envio
        {
            Id = Guid.NewGuid(),
            PedidoId = pedido.Id,
            Estado = EstadoEnvio.PREPARANDO,
            CodigoRastreo = GenerarCodigoRastreo(),
            FechaEnvio = DateTime.UtcNow,
            FechaEntregaEstimada = DateTime.UtcNow.AddDays(5)
        };

        _envios[envio.Id] = envio;
        return envio;
    }

    /// <summary>
    /// Actualiza el estado de un envío
    /// </summary>
    public async Task ActualizarEstado(Guid envioId, EstadoEnvio estado)
    {
        await Task.Delay(50);

        if (_envios.TryGetValue(envioId, out var envio))
        {
            envio.Estado = estado;
        }
        else
        {
            throw new InvalidOperationException("Envío no encontrado");
        }
    }

    /// <summary>
    /// Rastrea un envío por su ID
    /// </summary>
    public async Task<InfoRastreo> RastrearEnvio(Guid envioId)
    {
        await Task.Delay(50);

        if (!_envios.TryGetValue(envioId, out var envio))
        {
            throw new InvalidOperationException("Envío no encontrado");
        }

        return new InfoRastreo
        {
            EnvioId = envio.Id,
            CodigoRastreo = envio.CodigoRastreo,
            EstadoActual = envio.Estado,
            UbicacionActual = ObtenerUbicacionSimulada(envio.Estado),
            UltimaActualizacion = DateTime.UtcNow
        };
    }

    private static string GenerarCodigoRastreo()
    {
        var random = new Random();
        return $"TEC{random.Next(100000, 999999)}";
    }

    private static string ObtenerUbicacionSimulada(EstadoEnvio estado)
    {
        return estado switch
        {
            EstadoEnvio.PREPARANDO => "Centro de distribución - Preparando paquete",
            EstadoEnvio.EN_TRANSITO => "En tránsito hacia destino",
            EstadoEnvio.EN_DISTRIBUCION => "Centro de distribución local",
            EstadoEnvio.ENTREGADO => "Entregado al destinatario",
            EstadoEnvio.DEVUELTO => "Devuelto al remitente",
            _ => "Ubicación desconocida"
        };
    }
}
