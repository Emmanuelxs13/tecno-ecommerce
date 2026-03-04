using System.Collections.Concurrent;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Implementación genérica de <see cref="IRepositorio{T}"/> basada en memoria (ConcurrentDictionary).
/// Usada durante desarrollo y pruebas del Sprint 3, antes de implementar EF Core en Sprint 4.
/// Principio OCP: será reemplazada por la implementación EF Core sin modificar los servicios.
/// </summary>
public class RepositorioEnMemoria<T> : IRepositorio<T> where T : class
{
    /// <summary>Almacén en memoria hilo-seguro. Clave = Id de la entidad.</summary>
    protected readonly ConcurrentDictionary<int, T> _almacen = new();

    /// <summary>Función que extrae el Id entero de una entidad.</summary>
    private readonly Func<T, int>     _obtenerId;

    /// <summary>Acción que asigna un Id a la entidad al persistirla.</summary>
    private readonly Action<T, int>   _asignarId;

    private int _secuencia;

    /// <summary>Inicializa el repositorio con accesores de Id por reflexión simple.</summary>
    public RepositorioEnMemoria(Func<T, int> obtenerId, Action<T, int> asignarId)
    {
        _obtenerId = obtenerId;
        _asignarId = asignarId;
    }

    /// <inheritdoc/>
    public virtual Task<IEnumerable<T>> ObtenerTodosAsync() =>
        Task.FromResult<IEnumerable<T>>(_almacen.Values.ToList());

    /// <inheritdoc/>
    public virtual Task<T?> ObtenerPorIdAsync(int id) =>
        Task.FromResult(_almacen.TryGetValue(id, out var e) ? e : null);

    /// <inheritdoc/>
    public virtual Task AgregarAsync(T entidad)
    {
        var id = Interlocked.Increment(ref _secuencia);
        _asignarId(entidad, id);
        _almacen[id] = entidad;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ActualizarAsync(T entidad)
    {
        _almacen[_obtenerId(entidad)] = entidad;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task EliminarAsync(int id)
    {
        _almacen.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
