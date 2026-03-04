using TecnoEcommerce.Modelos.Entidades;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Repositorio en memoria para la entidad Envio.
/// Reutiliza la lógica genérica de RepositorioEnMemoria.
/// Será reemplazado por implementación EF Core en Sprint 4.
/// </summary>
public class EnvioRepositorioEnMemoria : RepositorioEnMemoria<Envio>
{
    public EnvioRepositorioEnMemoria()
        : base(e => e.Id, (e, id) => e.Id = id)
    {
    }
}
