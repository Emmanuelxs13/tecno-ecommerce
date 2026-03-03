namespace TecnoEcommerce.Modelos.Enumeraciones;

/// <summary>
/// Representa el progreso logístico del envío de un pedido hacia el cliente.
/// Principio SRP: encapsula exclusivamente los estados del flujo de envío,
/// separando la responsabilidad logística del resto del dominio.
/// </summary>
public enum EstadoEnvio
{
    /// <summary>El paquete está siendo armado y preparado en el almacén.</summary>
    Preparando = 1,

    /// <summary>El paquete fue despachado y se encuentra en tránsito con el transportista.</summary>
    EnCamino = 2,

    /// <summary>El paquete fue entregado exitosamente en la dirección del cliente.</summary>
    Entregado = 3
}
