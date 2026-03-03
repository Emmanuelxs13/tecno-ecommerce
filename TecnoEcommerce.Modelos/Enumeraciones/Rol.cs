namespace TecnoEcommerce.Modelos.Enumeraciones;

/// <summary>
/// Define los roles disponibles en el sistema.
/// Principio SRP: esta enumeración tiene una única responsabilidad,
/// representar el tipo de acceso que tiene un usuario dentro de la plataforma.
/// </summary>
public enum Rol
{
    /// <summary>Usuario final que puede navegar el catálogo y realizar compras.</summary>
    Cliente = 1,

    /// <summary>Usuario con permisos elevados para gestionar productos, categorías y pedidos.</summary>
    Administrador = 2
}
