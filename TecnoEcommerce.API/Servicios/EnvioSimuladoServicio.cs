using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Enumeraciones;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Servicios;

/// <summary>
/// Implementación simulada del servicio de gestión de envíos.
/// Simula el comportamiento de un proveedor logístico real (DHL, FedEx, etc.)
/// para permitir el desarrollo y las pruebas sin integración externa.
/// Principio OCP: cuando se integre un proveedor real, solo se cambia esta clase
/// (o se agrega una nueva implementación de <see cref="IEnvioServicio"/>) sin
/// modificar ninguna otra capa del sistema.
/// Principio SRP: solo gestiona el ciclo de vida del envío.
/// </summary>
public class EnvioSimuladoServicio : IEnvioServicio
{
    // ── Dependencias inyectadas ────────────────────────────────────────────────

    /// <summary>Repositorio genérico de envíos para persistir y consultar registros logísticos.</summary>
    private readonly IRepositorio<Envio> _envioRepositorio;

    /// <summary>Logger para registrar los eventos de envío simulados.</summary>
    private readonly ILogger<EnvioSimuladoServicio> _logger;

    /// <summary>
    /// Inicializa el servicio con las dependencias requeridas por inyección de dependencias.
    /// </summary>
    public EnvioSimuladoServicio(
        IRepositorio<Envio> envioRepositorio,
        ILogger<EnvioSimuladoServicio> logger)
    {
        _envioRepositorio = envioRepositorio;
        _logger           = logger;
    }

    // ── Métodos públicos ───────────────────────────────────────────────────────

    /// <inheritdoc/>
    /// <remarks>
    /// Simulación: asigna un transportista ficticio y calcula una fecha de entrega
    /// estimada de 3 a 7 días hábiles a partir de hoy.
    /// </remarks>
    public async Task<Envio> CrearEnvioAsync(int pedidoId, string direccion)
    {
        _logger.LogInformation(
            "Creando envío simulado para pedido Id: {PedidoId}", pedidoId);

        // Transportistas disponibles en la simulación
        string[] transportistas = ["TecnoExpress", "RapidoEnvíos", "LogisTech", "PaquetePro"];
        var transportista = transportistas[Random.Shared.Next(transportistas.Length)];

        var envio = new Envio
        {
            PedidoId              = pedidoId,
            Direccion             = direccion,
            Transportista         = transportista,
            EstadoEnvio           = EstadoEnvio.Preparando,
            // Fecha estimada: entre 3 y 7 días a partir de hoy
            FechaEstimadaEntrega  = DateTime.UtcNow.AddDays(Random.Shared.Next(3, 8))
        };

        await _envioRepositorio.AgregarAsync(envio);

        _logger.LogInformation(
            "Envío Id {EnvioId} creado. Transportista: {Transportista}. Entrega estimada: {Fecha}.",
            envio.Id, envio.Transportista, envio.FechaEstimadaEntrega);

        return envio;
    }

    /// <inheritdoc/>
    public async Task ActualizarEstadoAsync(int envioId, EstadoEnvio nuevoEstado)
    {
        var envio = await _envioRepositorio.ObtenerPorIdAsync(envioId)
            ?? throw new InvalidOperationException($"No existe un envío con Id {envioId}.");

        var estadoAnterior = envio.EstadoEnvio;
        envio.EstadoEnvio  = nuevoEstado;

        await _envioRepositorio.ActualizarAsync(envio);

        _logger.LogInformation(
            "Estado del envío Id {EnvioId} cambiado de {EstadoAnterior} a {NuevoEstado}.",
            envioId, estadoAnterior, nuevoEstado);
    }

    /// <inheritdoc/>
    public async Task<Envio?> ConsultarEstadoAsync(int envioId)
    {
        return await _envioRepositorio.ObtenerPorIdAsync(envioId);
    }
}
