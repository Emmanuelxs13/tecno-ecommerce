namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa el detalle de un pedido (línea de pedido)
/// </summary>
public class DetallePedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }

    // Relaciones
    public Pedido? Pedido { get; private set; }
    public Producto? Producto { get; private set; }

    // Constructor privado para EF Core
    private DetallePedido() { }

    /// <summary>
    /// Crea un nuevo detalle de pedido
    /// </summary>
    public static DetallePedido Crear(Guid productoId, int cantidad, decimal precio)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero", nameof(precio));

        return new DetallePedido
        {
            Id = Guid.NewGuid(),
            ProductoId = productoId,
            Cantidad = cantidad,
            Precio = precio
        };
    }

    /// <summary>
    /// Calcula el subtotal del detalle (cantidad * precio)
    /// </summary>
    public decimal CalcularSubtotal()
    {
        return Cantidad * Precio;
    }
}
