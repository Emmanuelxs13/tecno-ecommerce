namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Reseña que un cliente deja sobre un producto que ha comprado.
/// Principio SRP: solo almacena la opinión del usuario sobre el producto;
/// la validación de que el usuario compró el producto es responsabilidad del servicio.
/// </summary>
public class Resenia
{
    /// <summary>Identificador único de la reseña.</summary>
    public int Id { get; set; }

    /// <summary>Puntuación de 1 a 5 estrellas.</summary>
    public int Calificacion { get; set; }

    /// <summary>Comentario libre del usuario sobre el producto.</summary>
    public string Comentario { get; set; } = string.Empty;

    /// <summary>Fecha y hora UTC en que se publicó la reseña.</summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // ── Claves foráneas ───────────────────────────────────────────────────────

    /// <summary>Identificador del usuario que escribió la reseña.</summary>
    public int UsuarioId { get; set; }

    /// <summary>Identificador del producto reseñado.</summary>
    public int ProductoId { get; set; }

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>Usuario autor de la reseña.</summary>
    public Usuario? Usuario { get; set; }

    /// <summary>Producto que está siendo reseñado.</summary>
    public Producto? Producto { get; set; }
}
