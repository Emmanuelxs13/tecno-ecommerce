namespace TecnoEcommerce.Modelos.Enumeraciones;

/// <summary>
/// Representa los posibles estados dentro del ciclo de vida de un pedido.
/// Principio SRP: encapsula exclusivamente la progresión de estados de un pedido,
/// evitando el uso de cadenas de texto ("magic strings") propensas a errores.
/// </summary>
public enum EstadoPedido
{
    /// <summary>El pedido fue registrado pero aún no inició su procesamiento.</summary>
    Pendiente = 1,

    /// <summary>El pedido está siendo preparado en el almacén.</summary>
    Procesando = 2,

    /// <summary>El pedido fue entregado al transportista y está en camino.</summary>
    Enviado = 3,

    /// <summary>El pedido llegó correctamente a la dirección del cliente.</summary>
    Entregado = 4,

    /// <summary>El pedido fue anulado antes o durante su procesamiento.</summary>
    Cancelado = 5
}
