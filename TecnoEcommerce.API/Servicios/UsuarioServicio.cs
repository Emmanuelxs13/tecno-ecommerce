using System.Security.Cryptography;
using System.Text;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación del servicio de gestión de usuarios.
/// Principio SRP: gestiona exclusivamente el registro, autenticación y consulta de perfil.
/// Principio DIP: depende de <see cref="IRepositorio{Usuario}"/>, no de EF Core directamente.
/// </summary>
public class UsuarioServicio : IUsuarioServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio genérico para persistir y consultar usuarios.</summary>
    private readonly IRepositorio<Usuario> _usuarioRepositorio;

    /// <summary>Logger para registrar eventos relevantes del servicio.</summary>
    private readonly ILogger<UsuarioServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas.
    /// Principio DIP: las dependencias llegan por constructor (inyección de dependencias).
    /// </summary>
    public UsuarioServicio(IRepositorio<Usuario> usuarioRepositorio, ILogger<UsuarioServicio> logger)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _logger = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<UsuarioDto> RegistrarAsync(RegistroUsuarioDto dto)
    {
        _logger.LogInformation("Registrando nuevo usuario con email: {Email}", dto.Email);

        // Verificar que el correo no esté ya registrado
        var existentes = await _usuarioRepositorio.ObtenerTodosAsync();
        if (existentes.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"El correo '{dto.Email}' ya está registrado.");

        // Crear la entidad con la contraseña cifrada
        var usuario = new Usuario
        {
            Nombre          = dto.Nombre,
            Email           = dto.Email,
            ContrasenaHash  = CifrarContrasena(dto.Contrasena)
        };

        await _usuarioRepositorio.AgregarAsync(usuario);
        _logger.LogInformation("Usuario registrado correctamente con Id: {Id}", usuario.Id);

        return MapearADto(usuario);
    }

    /// <inheritdoc/>
    public async Task<LoginRespuestaDto> LoginAsync(LoginUsuarioDto dto)
    {
        _logger.LogInformation("Intento de login para: {Email}", dto.Email);

        // Buscar el usuario por email
        var usuarios = await _usuarioRepositorio.ObtenerTodosAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

        // Verificar existencia y contraseña
        if (usuario is null || usuario.ContrasenaHash != CifrarContrasena(dto.Contrasena))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        _logger.LogInformation("Login exitoso para el usuario Id: {Id}", usuario.Id);

        return new LoginRespuestaDto
        {
            Usuario = MapearADto(usuario),
            // TODO Sprint 4: generar token JWT real con JwtSecurityTokenHandler
            Token = $"token-simulado-{usuario.Id}"
        };
    }

    /// <inheritdoc/>
    public async Task<UsuarioDto?> ObtenerPerfilAsync(int usuarioId)
    {
        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(usuarioId);
        return usuario is null ? null : MapearADto(usuario);
    }

    // ── Métodos privados ───────────────────────────────────────────────────────

    /// <summary>
    /// Cifra una contraseña en texto plano usando SHA-256.
    /// NOTA: En producción se debe usar BCrypt o Argon2 para mayor seguridad.
    /// Se dejó SHA-256 para no agregar dependencias externas antes del Sprint 3.
    /// </summary>
    /// <param name="contrasena">Contraseña en texto plano.</param>
    /// <returns>Representación hexadecimal del hash SHA-256.</returns>
    private static string CifrarContrasena(string contrasena)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    /// <summary>
    /// Mapea una entidad <see cref="Usuario"/> al DTO de respuesta.
    /// Centraliza el mapeo para evitar duplicación (DRY).
    /// </summary>
    private static UsuarioDto MapearADto(Usuario usuario) => new()
    {
        Id     = usuario.Id,
        Nombre = usuario.Nombre,
        Email  = usuario.Email,
        Rol    = usuario.Rol
    };
}
