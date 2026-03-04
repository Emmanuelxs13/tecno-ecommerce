namespace TecnoEcommerce.Modelos.DTOs;

// ── Solicitudes ───────────────────────────────────────────────────────────────

/// <summary>
/// Datos requeridos para agregar un producto al carrito.
/// Principio SRP: solo transporta los datos de la operación de agregar ítem.
/// </summary>
public class AgregarItemCarritoDto
{
    /// <summary>Identificador del producto a agregar.</summary>
    public int ProductoId { get; set; }

    /// <summary>Número de unidades a agregar. Debe ser mayor a cero.</summary>
    public int Cantidad { get; set; }
}

/// <summary>
/// Datos requeridos para actualizar la cantidad de un ítem existente en el carrito.
/// Si la cantidad es 0 o inferior el servicio elimina el ítem.
/// </summary>
public class ActualizarCantidadItemDto
{
    /// <summary>Nueva cantidad deseada. Valores ≤ 0 provocan la eliminación del ítem.</summary>
    public int Cantidad { get; set; }
}

// ── Respuestas ────────────────────────────────────────────────────────────────

/// <summary>
/// Representación de una línea dentro del carrito, enviada al cliente.
/// Incluye nombre e imagen del producto para no requerir llamadas adicionales.
/// </summary>
public class ItemCarritoDto
{
    /// <summary>Identificador del ítem dentro del carrito.</summary>
    public int Id { get; set; }

    /// <summary>Identificador del producto.</summary>
    public int ProductoId { get; set; }

    /// <summary>Nombre del producto para mostrar en la línea del carrito.</summary>
    public string NombreProducto { get; set; } = string.Empty;

    /// <summary>URL de la imagen del producto.</summary>
    public string ImagenUrl { get; set; } = string.Empty;

    /// <summary>Precio unitario capturado al momento de agregar al carrito.</summary>
    public decimal PrecioUnitario { get; set; }

    /// <summary>Cantidad de unidades seleccionadas por el usuario.</summary>
    public int Cantidad { get; set; }

    /// <summary>Subtotal calculado como PrecioUnitario × Cantidad.</summary>
    public decimal Subtotal => PrecioUnitario * Cantidad;
}

/// <summary>
/// Vista completa del carrito de un usuario, con todos sus ítems y el total acumulado.
/// </summary>
public class CarritoDto
{
    /// <summary>Identificador único del carrito.</summary>
    public int Id { get; set; }

    /// <summary>Identificador del usuario propietario.</summary>
    public int UsuarioId { get; set; }

    /// <summary>Ítems (líneas de producto) contenidos actualmente en el carrito.</summary>
    public ICollection<ItemCarritoDto> Items { get; set; } = new List<ItemCarritoDto>();

    /// <summary>
    /// Total general del carrito calculado como la suma de todos los subtotales.
    /// Propiedad calculada; no se persiste en la base de datos.
    /// </summary>
    public decimal Total => Items.Sum(i => i.Subtotal);
}
