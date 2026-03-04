using TecnoEcommerce.Modelos.DTOs;

namespace TecnoEcommerce.Web.Servicios;

/// <summary>
/// Servicio singleton que gestiona el estado de la sesión activa en el navegador.
/// Almacena el usuario autenticado y el token JWT en memoria (se pierde al recargar la página,
/// lo que es aceptable para el entorno de desarrollo; en producción se complementaría
/// con localStorage vía JSInterop).
/// Principio SRP: única responsabilidad — centralizar el estado de sesión y notificar cambios.
/// </summary>
public class SesionServicio
{
    /// <summary>
    /// Usuario actualmente autenticado.
    /// <c>null</c> cuando no hay sesión activa.
    /// </summary>
    public UsuarioDto? UsuarioActual { get; private set; }

    /// <summary>
    /// Token JWT de la sesión actual.
    /// Se incluye como cabecera Authorization en las peticiones protegidas.
    /// </summary>
    public string? Token { get; private set; }

    /// <summary>Indica si hay un usuario con sesión activa.</summary>
    public bool EstaAutenticado => UsuarioActual is not null;

    /// <summary>
    /// Evento que se dispara cuando el estado de sesión cambia (login o logout).
    /// Los componentes de la interfaz se suscriben para actualizar la vista.
    /// </summary>
    public event Action? OnCambio;

    /// <summary>
    /// Abre una sesión con los datos del usuario y el token devueltos por la API.
    /// </summary>
    /// <param name="usuario">Datos del usuario autenticado.</param>
    /// <param name="token">Token JWT válido para peticiones futuras.</param>
    public void IniciarSesion(UsuarioDto usuario, string token)
    {
        UsuarioActual = usuario;
        Token         = token;
        NotificarCambio();
    }

    /// <summary>
    /// Cierra la sesión actual limpiando el estado en memoria.
    /// </summary>
    public void CerrarSesion()
    {
        UsuarioActual = null;
        Token         = null;
        NotificarCambio();
    }

    /// <summary>
    /// Notifica a todos los suscriptores que el estado de sesión cambió.
    /// </summary>
    private void NotificarCambio() => OnCambio?.Invoke();
}
