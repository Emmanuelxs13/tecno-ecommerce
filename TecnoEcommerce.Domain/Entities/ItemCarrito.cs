namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa un item dentro del carrito de compras
/// </summary>
public class ItemCarrito
{
    public Guid Id { get; private set; }
    public Guid CarritoId { get; private set; }
    public Guid ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }

    // Relaciones
    public Carrito? Carrito { get; private set; }
    public Producto? Producto { get; private set; }

    // Constructor privado para EF Core
    private ItemCarrito() { }

    /// <summary>
    /// Crea un nuevo item de carrito
    /// </summary>
    public static ItemCarrito Crear(Guid productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

        if (precioUnitario <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero", nameof(precioUnitario));

        return new ItemCarrito
        {
            Id = Guid.NewGuid(),
            ProductoId = productoId,
            Cantidad = cantidad,
            PrecioUnitario = precioUnitario
        };
    }

    /// <summary>
    /// Actualiza la cantidad del item
    /// </summary>
    public void ActualizarCantidad(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

        Cantidad = cantidad;
    }

    /// <summary>
    /// Calcula el subtotal del item (cantidad * precio unitario)
    /// </summary>
    public decimal CalcularSubtotal()
    {
        return Cantidad * PrecioUnitario;
    }
}
