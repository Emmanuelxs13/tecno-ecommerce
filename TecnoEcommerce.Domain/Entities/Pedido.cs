using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa un pedido realizado por un usuario
/// </summary>
public class Pedido
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public decimal Total { get; private set; }
    public EstadoPedido Estado { get; private set; }
    public DateTime Fecha { get; private set; }
    public string DireccionEnvio { get; private set; } = string.Empty;

    // Relaciones
    public Usuario? Usuario { get; private set; }
    public ICollection<DetallePedido> Detalles { get; private set; } = new List<DetallePedido>();

    // Constructor privado para EF Core
    private Pedido() { }

    /// <summary>
    /// Crea un nuevo pedido
    /// </summary>
    public static Pedido Crear(Guid usuarioId, string direccionEnvio)
    {
        if (string.IsNullOrWhiteSpace(direccionEnvio))
            throw new ArgumentException("La dirección de envío es requerida", nameof(direccionEnvio));

        return new Pedido
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Estado = EstadoPedido.PENDIENTE,
            Fecha = DateTime.UtcNow,
            DireccionEnvio = direccionEnvio,
            Total = 0,
            Detalles = new List<DetallePedido>()
        };
    }

    /// <summary>
    /// Agrega un detalle al pedido
    /// </summary>
    public void AgregarDetalle(Guid productoId, int cantidad, decimal precio)
    {
        var detalle = DetallePedido.Crear(productoId, cantidad, precio);
        Detalles.Add(detalle);
        CalcularTotal();
    }

    /// <summary>
    /// Cambia el estado del pedido
    /// </summary>
    public void CambiarEstado(EstadoPedido nuevoEstado)
    {
        // Validación básica de transiciones de estado
        if (Estado == EstadoPedido.CANCELADO)
            throw new InvalidOperationException("No se puede cambiar el estado de un pedido cancelado");

        if (Estado == EstadoPedido.ENTREGADO && nuevoEstado != EstadoPedido.ENTREGADO)
            throw new InvalidOperationException("No se puede cambiar el estado de un pedido entregado");

        Estado = nuevoEstado;
    }

    /// <summary>
    /// Calcula el total del pedido sumando todos los subtotales de los detalles
    /// </summary>
    public void CalcularTotal()
    {
        Total = Detalles.Sum(d => d.CalcularSubtotal());
    }

    /// <summary>
    /// Cancela el pedido
    /// </summary>
    public void Cancelar()
    {
        if (Estado == EstadoPedido.ENVIADO || Estado == EstadoPedido.ENTREGADO)
            throw new InvalidOperationException("No se puede cancelar un pedido que ya fue enviado o entregado");

        Estado = EstadoPedido.CANCELADO;
    }
}
