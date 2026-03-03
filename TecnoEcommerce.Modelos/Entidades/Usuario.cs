using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.Entidades;

/// <summary>
/// Representa a una persona registrada en la plataforma.
/// Es la entidad central de autenticación, autorización y seguimiento de compras.
/// Principio SRP: solo almacena la identidad del usuario y sus relaciones directas;
/// la lógica de autenticación y cifrado queda delegada al servicio correspondiente.
/// </summary>
public class Usuario
{
    /// <summary>Identificador único del usuario (clave primaria en la base de datos).</summary>
    public int Id { get; set; }

    /// <summary>Nombre completo del usuario, usado en comunicaciones y pantallas de perfil.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario; se emplea como identificador único en el inicio de sesión.
    /// Debe ser único en toda la tabla.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña almacenada como hash (BCrypt o similar).
    /// Nunca se guarda ni se expone el texto plano.
    /// El <c>UsuarioServicio</c> es responsable de cifrar antes de persistir.
    /// </summary>
    public string ContrasenaHash { get; set; } = string.Empty;

    /// <summary>
    /// Rol que determina los permisos del usuario dentro del sistema.
    /// Por defecto es <see cref="Rol.Cliente"/> al momento del registro.
    /// </summary>
    public Rol Rol { get; set; } = Rol.Cliente;

    // ── Propiedades de navegación ─────────────────────────────────────────────

    /// <summary>
    /// Carrito de compras activo del usuario.
    /// Relación 1-a-1: cada usuario tiene como máximo un carrito.
    /// </summary>
    public Carrito? Carrito { get; set; }

    /// <summary>
    /// Historial completo de pedidos realizados por este usuario.
    /// Relación 1-a-N: un usuario puede tener múltiples pedidos.
    /// </summary>
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
