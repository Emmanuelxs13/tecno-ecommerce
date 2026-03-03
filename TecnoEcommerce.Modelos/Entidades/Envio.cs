using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Almacena la información logística del despacho de un pedido hacia el cliente.
/// Principio SRP: solo gestiona el seguimiento físico del paquete;
/// la simulación del progreso de envío es responsabilidad del <c>EnvioSimuladoServicio</c>.
/// </summary>
public class Envio
{
    /// <summary>Identificador único del envío (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>
    /// Dirección completa de entrega proporcionada por el cliente al confirmar el pedido
    /// (calle, número, ciudad, código postal, país).
    /// </summary>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>Nombre de la empresa o persona encargada del transporte (ej. "DHL", "FedEx").</summary>
    public string Transportista { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual del envío dentro de su ciclo logístico.
    /// Inicia en <see cref="EstadoEnvio.Preparando"/> al crear el registro.
    /// </summary>
    public EstadoEnvio EstadoEnvio { get; set; } = EstadoEnvio.Preparando;

    /// <summary>
    /// Fecha estimada en que el paquete llegará al cliente.
    /// Es <c>null</c> hasta que el servicio de envío calcule la fecha.
    /// </summary>
    public DateTime? FechaEstimadaEntrega { get; set; }

    // ── Clave foránea ─────────────────────────────────────────────────────────

    /// <summary>
    /// Identificador del pedido al que pertenece este envío.
    /// EF Core usa esta propiedad para mantener la restricción de unicidad 1-a-1 con Pedido.
    /// </summary>
    public int PedidoId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Pedido al que está vinculado este envío.</summary>
    public Pedido? Pedido { get; set; }
}
