namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Contenedor temporal de productos seleccionados por el usuario antes de confirmar la compra.
/// Cada usuario tiene como máximo un carrito activo (relación 1-a-1 con <see cref="Usuario"/>).
/// Principio SRP: solo gestiona la sesión de compra del usuario; la lógica de
/// agregar/quitar ítems y calcular totales es responsabilidad del <c>CarritoServicio</c>.
/// </summary>
public class Carrito
{
    /// <summary>Identificador único del carrito (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Fecha y hora UTC en que se creó el carrito.</summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // ── Clave foránea ─────────────────────────────────────────────────────────

    /// <summary>
    /// Identificador del usuario propietario del carrito.
    /// EF Core usa esta propiedad para generar la FK y la restricción de unicidad.
    /// </summary>
    public int UsuarioId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Usuario al que pertenece este carrito.</summary>
    public Usuario? Usuario { get; set; }

    /// <summary>
    /// Ítems (líneas de producto) que componen el carrito.
    /// Relación 1-a-N: un carrito puede contener múltiples ítems distintos.
    /// </summary>
    public ICollection<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
}
