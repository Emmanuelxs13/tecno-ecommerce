using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;
using TecnoEcommerce.Domain.Entities;
using TecnoEcommerce.Domain.Interfaces;

namespace TecnoEcommerce.Application.Services;

/// <summary>
/// Servicio para gestionar las operaciones relacionadas con usuarios
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IRepository<Usuario> _usuarioRepository;

    public UsuarioService(IRepository<Usuario> usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto?> GetByIdAsync(Guid id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        return usuario == null ? null : MapToDto(usuario);
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        // En un escenario real, aquí se hashearía la contraseña con BCrypt u otro algoritmo
        var passwordHash = HashPassword(dto.Password);
        
        var usuario = Usuario.Registrar(dto.Nombre, dto.Email, passwordHash, dto.Rol);
        var usuarioCreado = await _usuarioRepository.AddAsync(usuario);
        
        return MapToDto(usuarioCreado);
    }

    public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        var passwordHash = HashPassword(dto.Password);
        
        var usuario = usuarios.FirstOrDefault(u => 
            u.Email == dto.Email && u.IniciarSesion(passwordHash));

        return usuario == null ? null : MapToDto(usuario);
    }

    private static string HashPassword(string password)
    {
        // Simulación simple de hash - En producción usar BCrypt
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static UsuarioDto MapToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }
}
