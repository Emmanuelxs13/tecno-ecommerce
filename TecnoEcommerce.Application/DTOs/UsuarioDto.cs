using TecnoEcommerce.Domain.Enums;

namespace TecnoEcommerce.Application.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rol Rol { get; set; }
}

public class RegistrarUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Rol Rol { get; set; }
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
