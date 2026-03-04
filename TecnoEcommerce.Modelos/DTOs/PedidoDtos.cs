using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.DTOs;

// ── Solicitudes ───────────────────────────────────────────────────────────────

/// <summary>
/// Datos requeridos para confirmar un pedido a partir del carrito activo del usuario.
/// Principio SRP: solo transporta la información necesaria para crear el pedido.
/// </summary>
public class CrearPedidoDto
{
    /// <summary>
    /// Dirección completa de entrega (calle, número, ciudad, código postal, país).
    /// Se usará para generar el registro de envío asociado al pedido.
    /// </summary>
    public string DireccionEntrega { get; set; } = string.Empty;
}

// ── Respuestas ────────────────────────────────────────────────────────────────

/// <summary>
/// Detalle de una línea de producto dentro de un pedido.
/// </summary>
public class DetallePedidoDto
{
    /// <summary>Identificador del detalle.</summary>
    public int Id { get; set; }

    /// <summary>Identificador del producto comprado.</summary>
    public int ProductoId { get; set; }

    /// <summary>Nombre del producto en el momento de la compra.</summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>Precio unitario fijo al momento de la compra.</summary>
    public decimal PrecioUnitario { get; set; }

    /// <summary>Cantidad de unidades compradas.</summary>
    public int Cantidad { get; set; }

    /// <summary>Subtotal de esta línea (PrecioUnitario × Cantidad).</summary>
    public decimal Subtotal => PrecioUnitario * Cantidad;
}

/// <summary>
/// Resumen completo de un pedido devuelto al cliente.
/// Incluye detalles de productos, estado de pago y estado de envío.
/// </summary>
public class PedidoDto
{
    /// <summary>Identificador único del pedido.</summary>
    public int Id { get; set; }

    /// <summary>Fecha y hora UTC en que se confirmó el pedido.</summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>Monto total del pedido.</summary>
    public decimal Total { get; set; }

    /// <summary>Estado actual del pedido.</summary>
    public EstadoPedido EstadoPedido { get; set; }

    /// <summary>Estado del pago asociado.</summary>
    public EstadoPago EstadoPago { get; set; }

    /// <summary>Estado del envío si ya fue generado; <c>null</c> si el pedido aún está pendiente de pago.</summary>
    public EstadoEnvio? EstadoEnvio { get; set; }

    /// <summary>Dirección de entrega registrada en el envío.</summary>
    public string DireccionEntrega { get; set; } = string.Empty;

    /// <summary>Líneas de detalle que desglosan los productos comprados.</summary>
    public ICollection<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();
}
