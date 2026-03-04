using System.Net.Http.Json;
using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio cliente HTTP para el catálogo de productos.
/// Encapsula todas las llamadas a los endpoints de la API relacionados con productos.
/// Principio SRP: solo gestiona la comunicación con los endpoints de productos.
/// </summary>
public class ProductoApiServicio
{
    private readonly HttpClient _http;

    /// <summary>Inicializa el servicio con el cliente HTTP preconfigurado.</summary>
    public ProductoApiServicio(HttpClient http) => _http = http;

    /// <summary>Obtiene todos los productos del catálogo desde la API.</summary>
    /// <returns>Lista de productos, o lista vacía si falla la petición.</returns>
    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var resultado = await _http.GetFromJsonAsync<List<ProductoDto>>("api/productos");
        return resultado ?? new List<ProductoDto>();
    }

    /// <summary>Obtiene el detalle completo de un producto por su Id.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>El producto, o <c>null</c> si no existe.</returns>
    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        try { return await _http.GetFromJsonAsync<ProductoDto>($"api/productos/{id}"); }
        catch { return null; }
    }

    /// <summary>Obtiene los productos de una categoría específica.</summary>
    /// <param name="categoriaId">Identificador de la categoría.</param>
    public async Task<List<ProductoDto>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        var resultado = await _http.GetFromJsonAsync<List<ProductoDto>>(
            $"api/productos?categoriaId={categoriaId}");
        return resultado ?? new List<ProductoDto>();
    }

    /// <summary>
    /// Busca productos cuyo nombre contenga el texto indicado (búsqueda parcial).
    /// </summary>
    /// <param name="nombre">Texto de búsqueda.</param>
    public async Task<List<ProductoDto>> BuscarPorNombreAsync(string nombre)
    {
        var resultado = await _http.GetFromJsonAsync<List<ProductoDto>>(
            $"api/productos?nombre={Uri.EscapeDataString(nombre)}");
        return resultado ?? new List<ProductoDto>();
    }
}
