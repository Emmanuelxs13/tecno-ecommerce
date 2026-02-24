using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Domain.Entities;

/// <summary>
/// Entidad que representa un usuario del sistema
/// </summary>
public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Rol Rol { get; private set; }

    // Constructor privado para EF Core
    private Usuario() { }

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    public static Usuario Registrar(string nombre, string email, string passwordHash, Rol rol)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es requerido", nameof(nombre));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es requerido", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("La contraseña es requerida", nameof(passwordHash));

        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = nombre,
            Email = email,
            PasswordHash = passwordHash,
            Rol = rol
        };
    }

    /// <summary>
    /// Valida las credenciales del usuario para iniciar sesión
    /// </summary>
    public bool IniciarSesion(string passwordHash)
    {
        return PasswordHash == passwordHash;
    }

    /// <summary>
    /// Actualiza la información del perfil del usuario
    /// </summary>
    public void ActualizarPerfil(string nombre, string email)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
            Nombre = nombre;

        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
    }
}
