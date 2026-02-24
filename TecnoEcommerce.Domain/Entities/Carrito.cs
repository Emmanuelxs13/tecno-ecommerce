namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa el carrito de compras de un usuario
/// </summary>
public class Carrito
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Relaciones
    public Usuario? Usuario { get; private set; }
    public ICollection<ItemCarrito> Items { get; private set; } = new List<ItemCarrito>();

    // Constructor privado para EF Core
    private Carrito() { }

    /// <summary>
    /// Crea un nuevo carrito para un usuario
    /// </summary>
    public static Carrito Crear(Guid usuarioId)
    {
        return new Carrito
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            FechaCreacion = DateTime.UtcNow,
            Items = new List<ItemCarrito>()
        };
    }

    /// <summary>
    /// Agrega un item al carrito
    /// </summary>
    public void AgregarItem(Guid productoId, int cantidad, decimal precioUnitario)
    {
        var itemExistente = Items.FirstOrDefault(i => i.ProductoId == productoId);

        if (itemExistente != null)
        {
            itemExistente.ActualizarCantidad(itemExistente.Cantidad + cantidad);
        }
        else
        {
            var nuevoItem = ItemCarrito.Crear(productoId, cantidad, precioUnitario);
            Items.Add(nuevoItem);
        }
    }

    /// <summary>
    /// Modifica la cantidad de un item en el carrito
    /// </summary>
    public void ModificarItem(Guid itemId, int cantidad)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item no encontrado en el carrito");

        item.ActualizarCantidad(cantidad);
    }

    /// <summary>
    /// Elimina un item del carrito
    /// </summary>
    public void EliminarItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    /// <summary>
    /// Vacía todos los items del carrito
    /// </summary>
    public void VaciarCarrito()
    {
        Items.Clear();
    }

    /// <summary>
    /// Calcula el total del carrito sumando todos los subtotales
    /// </summary>
    public decimal CalcularTotal()
    {
        return Items.Sum(i => i.CalcularSubtotal());
    }
}
