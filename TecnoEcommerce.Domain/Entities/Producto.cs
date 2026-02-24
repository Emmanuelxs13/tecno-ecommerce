namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa un producto tecnológico
/// </summary>
public class Producto
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoriaId { get; private set; }

    // Relación con categoría
    public Categoria? Categoria { get; private set; }

    // Constructor privado para EF Core
    private Producto() { }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    public static Producto Crear(string nombre, string descripcion, decimal precio, int stock, Guid categoriaId)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es requerido", nameof(nombre));

        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero", nameof(precio));

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo", nameof(stock));

        return new Producto
        {
            Id = Guid.NewGuid(),
            Nombre = nombre,
            Descripcion = descripcion ?? string.Empty,
            Precio = precio,
            Stock = stock,
            CategoriaId = categoriaId
        };
    }

    /// <summary>
    /// Edita la información del producto
    /// </summary>
    public void Editar(string nombre, string descripcion, decimal precio, Guid categoriaId)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
            Nombre = nombre;

        if (!string.IsNullOrWhiteSpace(descripcion))
            Descripcion = descripcion;

        if (precio > 0)
            Precio = precio;

        if (categoriaId != Guid.Empty)
            CategoriaId = categoriaId;
    }

    /// <summary>
    /// Actualiza el stock del producto
    /// </summary>
    public void ActualizarStock(int cantidad)
    {
        if (Stock + cantidad < 0)
            throw new InvalidOperationException("Stock insuficiente");

        Stock += cantidad;
    }

    /// <summary>
    /// Verifica si hay disponibilidad del producto
    /// </summary>
    public bool VerificarDisponibilidad(int cantidadSolicitada)
    {
        return Stock >= cantidadSolicitada;
    }
}
