namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa una categoría de productos
/// </summary>
public class Categoria
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;

    // Relación con productos
    public ICollection<Producto> Productos { get; private set; } = new List<Producto>();

    // Constructor privado para EF Core
    private Categoria() { }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    public static Categoria Crear(string nombre, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es requerido", nameof(nombre));

        return new Categoria
        {
            Id = Guid.NewGuid(),
            Nombre = nombre,
            Descripcion = descripcion ?? string.Empty
        };
    }

    /// <summary>
    /// Edita la información de la categoría
    /// </summary>
    public void Editar(string nombre, string descripcion)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
            Nombre = nombre;

        if (!string.IsNullOrWhiteSpace(descripcion))
            Descripcion = descripcion;
    }
}
