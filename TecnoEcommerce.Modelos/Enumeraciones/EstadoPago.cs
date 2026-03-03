namespace TecnoEcommerce.Modelos.Enumeraciones;

/// <summary>
/// Representa el resultado del procesamiento del pago asociado a un pedido.
/// Principio SRP: encapsula exclusivamente los estados del flujo de pago,
/// desacoplando la lógica de pago de la lógica del pedido.
/// </summary>
public enum EstadoPago
{
    /// <summary>El pago fue solicitado pero aún no tiene respuesta del procesador.</summary>
    Pendiente = 1,

    /// <summary>El pago fue autorizado y cobrado correctamente.</summary>
    Aprobado = 2,

    /// <summary>El pago fue denegado (fondos insuficientes, datos incorrectos, etc.).</summary>
    Rechazado = 3
}
