namespace TecnoEcommerce.Modelos.DTOs;

// ── Solicitudes ───────────────────────────────────────────────────────────────

/// <summary>
/// Datos necesarios para crear o actualizar una categoría.
/// Principio SRP: solo transporta los campos editables de la categoría.
/// </summary>
public class CrearCategoriaDto
{
    /// <summary>Nombre visible de la categoría en el catálogo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción de qué tipo de productos agrupa esta categoría.</summary>
    public string Descripcion { get; set; } = string.Empty;
}

// ── Respuestas ────────────────────────────────────────────────────────────────

/// <summary>
/// Datos de la categoría que se devuelven al cliente.
/// </summary>
public class CategoriaDto
{
    /// <summary>Identificador único de la categoría.</summary>
    public int Id { get; set; }

    /// <summary>Nombre visible de la categoría.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción de la categoría.</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de productos activos pertenecientes a esta categoría.
    /// Útil para la vista de administración y menús de navegación.
    /// </summary>
    public int TotalProductos { get; set; }
}
