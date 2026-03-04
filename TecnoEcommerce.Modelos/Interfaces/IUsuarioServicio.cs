using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato del servicio de gestión de usuarios.
/// Principio DIP: los controladores dependen de esta abstracción; la implementación concreta
/// (<c>UsuarioServicio</c>) puede cambiar sin afectar la capa de controladores.
/// Principio SRP: centraliza todos los casos de uso relacionados con la identidad del usuario.
/// </summary>
public interface IUsuarioServicio
{
    /// <summary>
    /// Registra un nuevo usuario en la plataforma.
    /// Valida que el correo no esté registrado y cifra la contraseña antes de persistirla.
    /// </summary>
    /// <param name="dto">Datos de registro proporcionados por el cliente.</param>
    /// <returns>Los datos públicos del usuario recién creado.</returns>
    /// <exception cref="InvalidOperationException">Si el correo ya se encuentra registrado.</exception>
    Task<UsuarioDto> RegistrarAsync(RegistroUsuarioDto dto);

    /// <summary>
    /// Valida las credenciales del usuario y genera un token de acceso.
    /// </summary>
    /// <param name="dto">Credenciales de inicio de sesión.</param>
    /// <returns>Datos del usuario y token JWT para autenticar peticiones posteriores.</returns>
    /// <exception cref="UnauthorizedAccessException">Si las credenciales son incorrectas.</exception>
    Task<LoginRespuestaDto> LoginAsync(LoginUsuarioDto dto);

    /// <summary>
    /// Obtiene el perfil público de un usuario por su identificador.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario a consultar.</param>
    /// <returns>Datos públicos del usuario, o <c>null</c> si no existe.</returns>
    Task<UsuarioDto?> ObtenerPerfilAsync(int usuarioId);
}
