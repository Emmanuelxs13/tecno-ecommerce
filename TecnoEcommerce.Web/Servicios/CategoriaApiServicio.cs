using System.Net.Http.Json;
using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio cliente HTTP para la gestión de categorías.
/// Principio SRP: solo gestiona la comunicación con los endpoints de categorías.
/// </summary>
public class CategoriaApiServicio
{
    private readonly HttpClient _http;

    /// <summary>Inicializa el servicio con el cliente HTTP preconfigurado.</summary>
    public CategoriaApiServicio(HttpClient http) => _http = http;

    /// <summary>Obtiene todas las categorías disponibles desde la API.</summary>
    /// <returns>Lista de categorías, o lista vacía si falla la petición.</returns>
    public async Task<List<CategoriaDto>> ObtenerTodasAsync()
    {
        var resultado = await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias");
        return resultado ?? new List<CategoriaDto>();
    }
}
