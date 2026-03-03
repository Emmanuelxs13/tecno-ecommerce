namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Línea individual dentro del carrito que relaciona un producto con la cantidad deseada.
/// Principio SRP: solo almacena la asociación entre un carrito y un producto con su precio
/// capturado; la operación de agregar o quitar ítems pertenece al <c>CarritoServicio</c>.
/// </summary>
public class ItemCarrito
{
    /// <summary>Identificador único del ítem (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Número de unidades del producto que el usuario desea comprar.</summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Precio unitario del producto en el momento en que fue agregado al carrito.
    /// Se captura para que cambios posteriores en el catálogo no afecten el precio mostrado al usuario.
    /// </summary>
    public decimal PrecioUnitario { get; set; }

    // ── Claves foráneas ───────────────────────────────────────────────────────

    /// <summary>Identificador del carrito al que pertenece este ítem.</summary>
    public int CarritoId { get; set; }

    /// <summary>Identificador del producto referenciado por este ítem.</summary>
    public int ProductoId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Carrito al que pertenece este ítem.</summary>
    public Carrito? Carrito { get; set; }

    /// <summary>Producto referenciado; se usa para mostrar nombre, imagen y precio actualizado.</summary>
    public Producto? Producto { get; set; }
}
