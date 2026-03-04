using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Modelos.Interfaces;

/// <summary>
/// Contrato del servicio de gestión del carrito de compras.
/// Principio DIP: los controladores dependen de esta abstracción.
/// Principio SRP: centraliza todos los casos de uso del carrito,
/// incluyendo la validación de stock antes de agregar ítems.
/// </summary>
public interface ICarritoServicio
{
    /// <summary>
    /// Obtiene el carrito activo del usuario con todos sus ítems.
    /// Si el usuario no tiene carrito, crea uno nuevo vacío.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    /// <returns>El carrito del usuario con sus ítems y totales calculados.</returns>
    Task<CarritoDto> ObtenerCarritoAsync(int usuarioId);

    /// <summary>
    /// Agrega un producto al carrito del usuario.
    /// Valida que el producto exista y tenga stock suficiente.
    /// Si el producto ya existe en el carrito, incrementa la cantidad.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario propietario del carrito.</param>
    /// <param name="dto">Datos del producto y la cantidad a agregar.</param>
    /// <returns>El carrito actualizado.</returns>
    /// <exception cref="InvalidOperationException">Si no hay stock suficiente.</exception>
    Task<CarritoDto> AgregarItemAsync(int usuarioId, AgregarItemCarritoDto dto);

    /// <summary>
    /// Elimina un ítem específico del carrito.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario propietario del carrito.</param>
    /// <param name="itemId">Identificador del ítem a eliminar.</param>
    /// <returns>El carrito actualizado sin el ítem eliminado.</returns>
    /// <exception cref="InvalidOperationException">Si el ítem no pertenece al carrito del usuario.</exception>
    Task<CarritoDto> EliminarItemAsync(int usuarioId, int itemId);

    /// <summary>
    /// Vacía por completo el carrito del usuario eliminando todos sus ítems.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    Task VaciarCarritoAsync(int usuarioId);
}
