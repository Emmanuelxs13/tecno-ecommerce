namespace TecnoEcommerce.Modelos.DTOs;

// ── Solicitudes ───────────────────────────────────────────────────────────────

/// <summary>
/// Datos necesarios para crear o actualizar un producto en el catálogo.
/// Principio SRP: solo transporta la información editable del producto.
/// La validación de negocio (precio positivo, stock >= 0) es responsabilidad del servicio.
/// </summary>
public class CrearProductoDto
{
    /// <summary>Nombre comercial del producto.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción técnica o comercial del producto.</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Precio de venta. Debe ser mayor a cero.</summary>
    public decimal Precio { get; set; }

    /// <summary>Unidades disponibles en inventario. No puede ser negativo.</summary>
    public int Stock { get; set; }

    /// <summary>URL de la imagen principal del producto.</summary>
    public string ImagenUrl { get; set; } = string.Empty;

    /// <summary>Identificador de la categoría a la que pertenece este producto.</summary>
    public int CategoriaId { get; set; }
}

// ── Respuestas ────────────────────────────────────────────────────────────────

/// <summary>
/// Datos del producto que se devuelven al cliente.
/// Incluye el nombre de la categoría para evitar llamadas adicionales desde el cliente.
/// </summary>
public class ProductoDto
{
    /// <summary>Identificador único del producto.</summary>
    public int Id { get; set; }

    /// <summary>Nombre comercial del producto.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción del producto.</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Precio de venta al público.</summary>
    public decimal Precio { get; set; }

    /// <summary>Unidades disponibles en inventario.</summary>
    public int Stock { get; set; }

    /// <summary>URL de la imagen principal.</summary>
    public string ImagenUrl { get; set; } = string.Empty;

    /// <summary>Identificador de la categoría.</summary>
    public int CategoriaId { get; set; }

    /// <summary>Nombre de la categoría para mostrar directamente en la vista.</summary>
    public string NombreCategoria { get; set; } = string.Empty;
}
