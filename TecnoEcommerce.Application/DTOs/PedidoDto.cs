using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Application.DTOs;

public class PedidoDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public decimal Total { get; set; }
    public EstadoPedido Estado { get; set; }
    public DateTime Fecha { get; set; }
    public string DireccionEnvio { get; set; } = string.Empty;
    public List<DetallePedidoDto> Detalles { get; set; } = new();
}

public class DetallePedidoDto
{
    public Guid Id { get; set; }
    public Guid ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Subtotal { get; set; }
}

public class CrearPedidoDto
{
    public Guid UsuarioId { get; set; }
    public string DireccionEnvio { get; set; } = string.Empty;
}
