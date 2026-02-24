using TecnoEcommerce.Application.DTOs;

namespace TecnoEcommerce.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de usuarios
/// </summary>
public interface IUsuarioService
{
    Task<UsuarioDto?> GetByIdAsync(Guid id);
    Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto);
    Task<UsuarioDto?> LoginAsync(LoginDto dto);
}
