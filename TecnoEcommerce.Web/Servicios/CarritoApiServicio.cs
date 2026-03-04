using System.Net.Http.Json;
using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio cliente HTTP para la gestión del carrito de compras.
/// Agrega el token JWT de la sesión activa en cada petición que lo requiere.
/// Principio SRP: solo gestiona la comunicación con los endpoints del carrito.
/// </summary>
public class CarritoApiServicio
{
    private readonly HttpClient     _http;
    private readonly SesionServicio _sesion;

    /// <summary>Inicializa el servicio con las dependencias requeridas.</summary>
    public CarritoApiServicio(HttpClient http, SesionServicio sesion)
    {
        _http   = http;
        _sesion = sesion;
    }

    /// <summary>Obtiene el carrito activo del usuario autenticado.</summary>
    /// <returns>El carrito del usuario, o <c>null</c> si no está autenticado.</returns>
    public async Task<CarritoDto?> ObtenerCarritoAsync()
    {
        if (!_sesion.EstaAutenticado) return null;
        try
        {
            AgregarTokenAlCliente();
            return await _http.GetFromJsonAsync<CarritoDto>(
                $"api/carrito/{_sesion.UsuarioActual!.Id}");
        }
        catch { return null; }
    }

    /// <summary>
    /// Agrega un producto al carrito del usuario autenticado.
    /// </summary>
    /// <param name="dto">Producto y cantidad a agregar.</param>
    /// <returns>El carrito actualizado, o <c>null</c> si la operación falla.</returns>
    public async Task<CarritoDto?> AgregarItemAsync(AgregarItemCarritoDto dto)
    {
        if (!_sesion.EstaAutenticado) return null;
        AgregarTokenAlCliente();
        var respuesta = await _http.PostAsJsonAsync(
            $"api/carrito/{_sesion.UsuarioActual!.Id}/items", dto);
        if (!respuesta.IsSuccessStatusCode) return null;
        return await respuesta.Content.ReadFromJsonAsync<CarritoDto>();
    }

    /// <summary>
    /// Elimina un ítem específico del carrito del usuario autenticado.
    /// </summary>
    /// <param name="itemId">Identificador del ítem a eliminar.</param>
    /// <returns>El carrito actualizado, o <c>null</c> si la operación falla.</returns>
    public async Task<CarritoDto?> EliminarItemAsync(int itemId)
    {
        if (!_sesion.EstaAutenticado) return null;
        AgregarTokenAlCliente();
        var respuesta = await _http.DeleteAsync(
            $"api/carrito/{_sesion.UsuarioActual!.Id}/items/{itemId}");
        if (!respuesta.IsSuccessStatusCode) return null;
        return await respuesta.Content.ReadFromJsonAsync<CarritoDto>();
    }

    /// <summary>
    /// Agrega el token JWT de la sesión actual como cabecera de autorización.
    /// Se llama antes de cada petición protegida.
    /// </summary>
    private void AgregarTokenAlCliente()
    {
        _http.DefaultRequestHeaders.Remove("Authorization");
        if (!string.IsNullOrEmpty(_sesion.Token))
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_sesion.Token}");
    }
}
