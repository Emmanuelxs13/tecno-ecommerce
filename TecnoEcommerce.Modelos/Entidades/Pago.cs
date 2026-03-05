using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Registro del pago realizado para cubrir un pedido.
/// Principio SRP: solo almacena los datos de la transacción de pago;
/// la lógica de autorización es responsabilidad del <c>PagoSimuladoServicio</c>.
/// </summary>
public class Pago
{
    /// <summary>Identificador único del pago.</summary>
    public int Id { get; set; }

    /// <summary>Monto total cobrado en la transacción.</summary>
    public decimal Monto { get; set; }

    /// <summary>Método de pago utilizado (ej. "Tarjeta", "PSE", "Transferencia").</summary>
    public string Metodo { get; set; } = string.Empty;

    /// <summary>Código de referencia generado por el procesador de pago.</summary>
    public string Referencia { get; set; } = string.Empty;

    /// <summary>Fecha y hora UTC en que se procesó el pago.</summary>
    public DateTime FechaPago { get; set; } = DateTime.UtcNow;

    /// <summary>Estado actual del pago.</summary>
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;

    // ── Clave foránea ─────────────────────────────────────────────────────────

    /// <summary>Identificador del pedido al que corresponde este pago.</summary>
    public int PedidoId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Pedido al que pertenece esta transacción de pago.</summary>
    public Pedido? Pedido { get; set; }
}
