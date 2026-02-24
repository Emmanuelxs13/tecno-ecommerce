using Microsoft.AspNetCore.Mvc;
using TecnoEcommerce.Application.DTOs;
using TecnoEcommerce.Application.Interfaces;

namespace TecnoEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtiene un usuario por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(Guid id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioDto>> Registrar([FromBody] RegistrarUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.RegistrarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Inicia sesión de un usuario
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginDto dto)
    {
        var usuario = await _usuarioService.LoginAsync(dto);
        if (usuario == null)
            return Unauthorized("Credenciales inválidas");

        return Ok(usuario);
    }
}
