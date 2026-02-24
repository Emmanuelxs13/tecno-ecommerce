using TecnoEcommerce.Domain.Entities;

namespace TecnoEcommerce.Domain.Interfaces;

/// <summary>
/// Interfaz genérica para repositorios siguiendo el patrón Repository
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
