namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Línea individual de un pedido que registra qué producto se compró,
/// en qué cantidad y al precio vigente en el momento de la compra.
/// Principio SRP: solo almacena la relación inmutable entre un pedido y un producto;
/// el cálculo del subtotal (PrecioUnitario × Cantidad) se realiza en el <c>PedidoServicio</c>.
/// </summary>
public class DetallePedido
{
    /// <summary>Identificador único del detalle (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Número de unidades del producto compradas en esta línea.</summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Precio unitario en el momento de la compra, fijado de forma inmutable.
    /// Garantiza que cambios posteriores en el catálogo no alteren el historial de pedidos.
    /// </summary>
    public decimal PrecioUnitario { get; set; }

    // ── Claves foráneas ───────────────────────────────────────────────────────

    /// <summary>Identificador del pedido al que pertenece esta línea de detalle.</summary>
    public int PedidoId { get; set; }

    /// <summary>Identificador del producto adquirido en esta línea.</summary>
    public int ProductoId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Pedido al que pertenece este detalle.</summary>
    public Pedido? Pedido { get; set; }

    /// <summary>Producto adquirido; permite acceder al nombre para mostrar en el resumen del pedido.</summary>
    public Producto? Producto { get; set; }
}
