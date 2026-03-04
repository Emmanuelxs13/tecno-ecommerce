using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Modelos.DTOs;

// ── Solicitudes ───────────────────────────────────────────────────────────────

/// <summary>
/// Datos necesarios para registrar un nuevo usuario en la plataforma.
/// Principio SRP: solo transporta la información de registro; la validación
/// y el cifrado de contraseña son responsabilidad del <c>UsuarioServicio</c>.
/// </summary>
public class RegistroUsuarioDto
{
    /// <summary>Nombre completo del usuario.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Correo electrónico que se usará como identificador de inicio de sesión.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña en texto plano enviada desde el cliente.
    /// El servicio la cifra antes de persistirla; nunca se almacena en texto plano.
    /// </summary>
    public string Contrasena { get; set; } = string.Empty;
}

/// <summary>
/// Credenciales enviadas por el usuario para iniciar sesión.
/// </summary>
public class LoginUsuarioDto
{
    /// <summary>Correo electrónico registrado en la plataforma.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña en texto plano; se compara contra el hash almacenado.</summary>
    public string Contrasena { get; set; } = string.Empty;
}

// ── Respuestas ────────────────────────────────────────────────────────────────

/// <summary>
/// Datos públicos del usuario que se devuelven al cliente tras el login o la consulta de perfil.
/// No expone el hash de la contraseña ni datos sensibles.
/// Principio ISP: el controlador solo recibe los campos que realmente necesita mostrar.
/// </summary>
public class UsuarioDto
{
    /// <summary>Identificador único del usuario.</summary>
    public int Id { get; set; }

    /// <summary>Nombre completo del usuario.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Correo electrónico del usuario.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Rol asignado al usuario dentro del sistema.</summary>
    public Rol Rol { get; set; }
}

/// <summary>
/// Respuesta devuelta tras un inicio de sesión exitoso.
/// Incluye los datos del usuario y el token de autenticación para futuras peticiones.
/// </summary>
public class LoginRespuestaDto
{
    /// <summary>Datos públicos del usuario autenticado.</summary>
    public UsuarioDto Usuario { get; set; } = null!;

    /// <summary>
    /// Token JWT generado para autenticar las peticiones posteriores.
    /// Se implementará en el Sprint 4 junto con la configuración de autenticación.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
