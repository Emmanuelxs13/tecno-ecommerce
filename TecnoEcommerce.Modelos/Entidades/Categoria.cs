namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Agrupa los productos del catálogo bajo una clasificación temática (ej. "Laptops", "Periféricos").
/// Principio SRP: solo gestiona la organización jerárquica del catálogo de productos;
/// no contiene lógica de negocio ni acceso a datos.
/// </summary>
public class Categoria
{
    /// <summary>Identificador único de la categoría (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Nombre visible de la categoría mostrado al cliente en el catálogo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción que explica qué tipo de productos agrupa esta categoría.</summary>
    public string Descripcion { get; set; } = string.Empty;

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>
    /// Colección de productos pertenecientes a esta categoría.
    /// Relación 1-a-N: una categoría puede contener múltiples productos.
    /// </summary>
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
