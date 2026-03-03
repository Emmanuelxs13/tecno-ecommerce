using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Cabecera que representa una compra confirmada generada a partir del carrito de un usuario.
/// Consolida los estados del pedido y del pago, y actúa como raíz del agregado
/// que incluye los detalles y el envío.
/// Principio SRP: solo gestiona la cabecera del pedido y los estados de su ciclo de vida;
/// la creación del pedido a partir del carrito es responsabilidad del <c>PedidoServicio</c>.
/// </summary>
public class Pedido
{
    /// <summary>Identificador único del pedido (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Fecha y hora UTC en que el pedido fue confirmado por el usuario.</summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Monto total del pedido calculado como la suma de (PrecioUnitario × Cantidad)
    /// de todos los <see cref="DetallePedido"/>.
    /// Se usa <c>decimal</c> para evitar errores de redondeo en cálculos monetarios.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Estado actual del pedido dentro de su ciclo de vida (Pendiente → Procesando → Enviado → Entregado).
    /// Inicia en <see cref="EstadoPedido.Pendiente"/> al momento de la creación.
    /// </summary>
    public EstadoPedido EstadoPedido { get; set; } = EstadoPedido.Pendiente;

    /// <summary>
    /// Estado del pago asociado a este pedido.
    /// Inicia en <see cref="EstadoPago.Pendiente"/> hasta que el servicio de pago responda.
    /// </summary>
    public EstadoPago EstadoPago { get; set; } = EstadoPago.Pendiente;

    // ── Clave foránea ─────────────────────────────────────────────────────────

    /// <summary>Identificador del usuario que realizó el pedido.</summary>
    public int UsuarioId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Usuario que realizó el pedido; permite acceder a su nombre y email para notificaciones.</summary>
    public Usuario? Usuario { get; set; }

    /// <summary>
    /// Líneas de detalle que desglosan los productos comprados.
    /// Relación 1-a-N: un pedido contiene uno o más detalles.
    /// </summary>
    public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

    /// <summary>
    /// Información de envío vinculada a este pedido.
    /// Relación 1-a-1: cada pedido genera un único registro de envío.
    /// </summary>
    public Envio? Envio { get; set; }
}
