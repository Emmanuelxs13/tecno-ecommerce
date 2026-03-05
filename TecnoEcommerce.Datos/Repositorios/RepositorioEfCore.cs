using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Datos.Contexto;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.Datos.Repositorios;

/// <summary>
/// Implementación genérica del repositorio usando Entity Framework Core.
/// Reemplaza a <c>RepositorioEnMemoria&lt;T&gt;</c> del Sprint 3.
/// Principio OCP: los servicios no cambian; solo se sustituye la implementación registrada en DI.
/// Principio DIP: los consumidores siguen dependiendo de <see cref="IRepositorio{T}"/>.
/// </summary>
public class RepositorioEfCore<T> : IRepositorio<T> where T : class
{
    protected readonly TiendaContexto _contexto;
    protected readonly DbSet<T> _conjunto;

    public RepositorioEfCore(TiendaContexto contexto)
    {
        _contexto = contexto;
        _conjunto = contexto.Set<T>();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> ObtenerTodosAsync() =>
        await _conjunto.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public virtual async Task<T?> ObtenerPorIdAsync(int id) =>
        await _conjunto.FindAsync(id);

    /// <inheritdoc/>
    public virtual async Task AgregarAsync(T entidad)
    {
        await _conjunto.AddAsync(entidad);
        await _contexto.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public virtual async Task ActualizarAsync(T entidad)
    {
        _conjunto.Update(entidad);
        await _contexto.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public virtual async Task EliminarAsync(int id)
    {
        var entidad = await _conjunto.FindAsync(id);
        if (entidad is not null)
        {
            _conjunto.Remove(entidad);
            await _contexto.SaveChangesAsync();
        }
    }
}
