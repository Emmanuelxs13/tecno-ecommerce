using System.Net.Http.Json;
using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio cliente HTTP para la gestión de pedidos.
/// Agrega el token JWT de sesión en cada petición protegida.
/// Principio SRP: solo gestiona la comunicación con los endpoints de pedidos.
/// </summary>
public class PedidoApiServicio
{
    private readonly HttpClient     _http;
    private readonly SesionServicio _sesion;

    /// <summary>Inicializa el servicio con las dependencias requeridas.</summary>
    public PedidoApiServicio(HttpClient http, SesionServicio sesion)
    {
        _http   = http;
        _sesion = sesion;
    }

    /// <summary>
    /// Crea un pedido a partir del carrito activo del usuario autenticado.
    /// </summary>
    /// <param name="dto">Dirección de entrega para el pedido.</param>
    /// <returns>El pedido creado con su estado de pago y envío, o <c>null</c> si falla.</returns>
    public async Task<PedidoDto?> CrearPedidoAsync(CrearPedidoDto dto)
    {
        if (!_sesion.EstaAutenticado) return null;
        AgregarTokenAlCliente();
        var respuesta = await _http.PostAsJsonAsync(
            $"api/pedidos/{_sesion.UsuarioActual!.Id}", dto);
        if (!respuesta.IsSuccessStatusCode) return null;
        return await respuesta.Content.ReadFromJsonAsync<PedidoDto>();
    }

    /// <summary>
    /// Obtiene el historial completo de pedidos del usuario autenticado.
    /// </summary>
    /// <returns>Lista de pedidos ordenados del más reciente al más antiguo.</returns>
    public async Task<List<PedidoDto>> ObtenerMisPedidosAsync()
    {
        if (!_sesion.EstaAutenticado) return new List<PedidoDto>();
        try
        {
            AgregarTokenAlCliente();
            var resultado = await _http.GetFromJsonAsync<List<PedidoDto>>(
                $"api/pedidos/mis-pedidos/{_sesion.UsuarioActual!.Id}");
            return resultado ?? new List<PedidoDto>();
        }
        catch { return new List<PedidoDto>(); }
    }

    /// <summary>
    /// Agrega el token JWT como cabecera de autorización antes de cada petición protegida.
    /// </summary>
    private void AgregarTokenAlCliente()
    {
        _http.DefaultRequestHeaders.Remove("Authorization");
        if (!string.IsNullOrEmpty(_sesion.Token))
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_sesion.Token}");
    }
}
