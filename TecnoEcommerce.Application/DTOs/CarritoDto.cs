namespace TecnoEcommerce.Application.DTOs;

public class CarritoDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public List<ItemCarritoDto> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public class ItemCarritoDto
{
    public Guid Id { get; set; }
    public Guid ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class AgregarItemCarritoDto
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
}
