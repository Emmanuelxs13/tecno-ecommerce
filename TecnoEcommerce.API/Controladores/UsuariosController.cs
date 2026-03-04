using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Modelos.DTOs;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Controladores;

/// <summary>
/// Controlador para el registro y autenticación de usuarios.
/// Principio SRP: solo gestiona los endpoints relacionados con la identidad del usuario.
/// Principio DIP: depende de IUsuarioServicio, no de la implementación concreta.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioServicio _servicio;

    /// <summary>Inicializa el controlador con el servicio de usuarios.</summary>
    public UsuariosController(IUsuarioServicio servicio) => _servicio = servicio;

    /// <summary>
    /// Registra un nuevo usuario en la plataforma.
    /// POST api/usuarios/registro
    /// </summary>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioDto dto)
    {
        try
        {
            var usuario = await _servicio.RegistrarAsync(dto);
            return CreatedAtAction(nameof(ObtenerPerfil), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Autentica al usuario y devuelve un token de acceso.
    /// POST api/usuarios/login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
    {
        try
        {
            var respuesta = await _servicio.LoginAsync(dto);
            return Ok(respuesta);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene el perfil público de un usuario.
    /// GET api/usuarios/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPerfil(int id)
    {
        var usuario = await _servicio.ObtenerPerfilAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }
}
