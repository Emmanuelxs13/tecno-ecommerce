using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa un envío
/// </summary>
public class Envio
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public EstadoEnvio Estado { get; set; }
    public string CodigoRastreo { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public DateTime? FechaEntregaEstimada { get; set; }
}

/// <summary>
/// Información de rastreo de un envío
/// </summary>
public class InfoRastreo
{
    public Guid EnvioId { get; set; }
    public string CodigoRastreo { get; set; } = string.Empty;
    public EstadoEnvio EstadoActual { get; set; }
    public string UbicacionActual { get; set; } = string.Empty;
    public DateTime UltimaActualizacion { get; set; }
}
