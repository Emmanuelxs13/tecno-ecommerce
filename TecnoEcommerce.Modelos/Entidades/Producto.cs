namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Representa un artículo disponible para la venta en la tienda.
/// Principio SRP: solo almacena los atributos comerciales del producto
/// y su vínculo con la categoría; la validación de stock y precios
/// es responsabilidad del <c>ProductoServicio</c>.
/// </summary>
public class Producto
{
    /// <summary>Identificador único del producto (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Nombre comercial del producto visible en el catálogo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción técnica o comercial que detalla las características del producto.</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Precio de venta al público.
    /// Se usa <c>decimal</c> en lugar de <c>double</c> para evitar errores de
    /// redondeo en operaciones monetarias (problema conocido con tipos de punto flotante).
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Cantidad de unidades disponibles en inventario.
    /// El <c>ProductoServicio</c> verifica que sea >= 1 antes de permitir agregar al carrito.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>URL relativa o absoluta de la imagen principal del producto.</summary>
    public string ImagenUrl { get; set; } = string.Empty;

    // ── Clave foránea ─────────────────────────────────────────────────────────

    /// <summary>
    /// Identificador de la categoría a la que pertenece este producto.
    /// EF Core usa esta propiedad para generar la columna FK en la tabla.
    /// </summary>
    public int CategoriaId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>
    /// Categoría a la que pertenece este producto.
    /// Relación N-a-1: muchos productos pertenecen a una sola categoría.
    /// </summary>
    public Categoria? Categoria { get; set; }
}
