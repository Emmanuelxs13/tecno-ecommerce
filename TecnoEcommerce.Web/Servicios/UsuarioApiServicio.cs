using System.Net.Http.Json;
using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio cliente HTTP para la gestión de usuarios.
/// Encapsula las llamadas a los endpoints de la API relacionados con autenticación y perfil.
/// Principio SRP: solo gestiona la comunicación con los endpoints de usuarios.
/// Principio DIP: los componentes Blazor dependen de este servicio, no de HttpClient directamente.
/// </summary>
public class UsuarioApiServicio
{
    private readonly HttpClient     _http;
    private readonly SesionServicio _sesion;

    /// <summary>
    /// Inicializa el servicio con el cliente HTTP preconfigurado y el servicio de sesión.
    /// </summary>
    public UsuarioApiServicio(HttpClient http, SesionServicio sesion)
    {
        _http   = http;
        _sesion = sesion;
    }

    /// <summary>
    /// Envía los datos de registro a la API y devuelve el usuario creado.
    /// </summary>
    /// <param name="dto">Datos del formulario de registro.</param>
    /// <returns>El usuario recién registrado, o <c>null</c> si la operación falla.</returns>
    public async Task<UsuarioDto?> RegistrarAsync(RegistroUsuarioDto dto)
    {
        var respuesta = await _http.PostAsJsonAsync("api/usuarios/registro", dto);
        if (!respuesta.IsSuccessStatusCode) return null;
        return await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
    }

    /// <summary>
    /// Envía las credenciales a la API, abre la sesión local si el login es exitoso
    /// y retorna los datos del usuario autenticado.
    /// </summary>
    /// <param name="dto">Credenciales del formulario de login.</param>
    /// <returns>Respuesta con usuario y token, o <c>null</c> si las credenciales son incorrectas.</returns>
    public async Task<LoginRespuestaDto?> LoginAsync(LoginUsuarioDto dto)
    {
        var respuesta = await _http.PostAsJsonAsync("api/usuarios/login", dto);
        if (!respuesta.IsSuccessStatusCode) return null;

        var resultado = await respuesta.Content.ReadFromJsonAsync<LoginRespuestaDto>();
        if (resultado is not null)
            _sesion.IniciarSesion(resultado.Usuario, resultado.Token);

        return resultado;
    }
}
