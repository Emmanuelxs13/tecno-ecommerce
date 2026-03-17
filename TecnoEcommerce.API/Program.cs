// ================================================================
// TecnoEcommerce - Punto de entrada de la API
// Sprint 4: PostgreSQL + Entity Framework Core
// Arquitectura: Modelo - Vista - Controlador (MVC)
// ================================================================
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TecnoEcommerce.API.Servicios;
using TecnoEcommerce.Datos.Contexto;
using TecnoEcommerce.Datos.Repositorios;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------
// Agregar controladores MVC
// -----------------------------------------------
builder.Services.AddControllers();

// -----------------------------------------------
// Configurar CORS para permitir peticiones desde
// el cliente Blazor WebAssembly.
// En producción restringe los orígenes al dominio real.
// -----------------------------------------------
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("BlazerPolicy", politica =>
    {
        politica
            .WithOrigins(
                "http://localhost:5074",   // Blazor WASM http
                "https://localhost:7154")  // Blazor WASM https
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// -----------------------------------------------
// Configurar Swagger para documentar la API
// -----------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------
// EF Core + PostgreSQL
// La cadena de conexión se lee de appsettings.json
// (clave "ConexionPrincipal").
// -----------------------------------------------
builder.Services.AddDbContext<TiendaContexto>(opciones =>
    opciones.UseNpgsql(
        builder.Configuration.GetConnectionString("ConexionPrincipal"),
        npgsql => npgsql.MigrationsAssembly("TecnoEcommerce.Datos")));

var cadenaConexion = builder.Configuration.GetConnectionString("ConexionPrincipal");

// -----------------------------------------------
// Repositorios EF Core (Scoped: una instancia por petición HTTP).
// Reemplazan a las implementaciones en memoria del Sprint 3.
// Principio OCP: los servicios no se modifican, solo se cambia
// la implementación concreta registrada en el contenedor DI.
// -----------------------------------------------
builder.Services.AddScoped<IRepositorio<Usuario>,   RepositorioEfCore<Usuario>>();
builder.Services.AddScoped<IRepositorio<Categoria>, RepositorioEfCore<Categoria>>();
builder.Services.AddScoped<IRepositorio<Envio>,     RepositorioEfCore<Envio>>();
builder.Services.AddScoped<IProductoRepositorio,    ProductoRepositorioEfCore>();
builder.Services.AddScoped<ICarritoRepositorio,     CarritoRepositorioEfCore>();
builder.Services.AddScoped<IPedidoRepositorio,      PedidoRepositorioEfCore>();

// -----------------------------------------------
// Servicios de negocio (Scoped: una instancia por petición HTTP).
// Principio DIP: se registran las interfaces, no las implementaciones concretas.
// -----------------------------------------------
builder.Services.AddScoped<IUsuarioServicio,  UsuarioServicio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<ICategoriaServicio,CategoriaServicio>();
builder.Services.AddScoped<ICarritoServicio,  CarritoServicio>();
builder.Services.AddScoped<IPedidoServicio,   PedidoServicio>();
builder.Services.AddScoped<IPagoServicio,     PagoSimuladoServicio>();
builder.Services.AddScoped<IEnvioServicio,    EnvioSimuladoServicio>();

var app = builder.Build();

// -----------------------------------------------
// Verificar conexión a la base de datos al arrancar.
// La estructura de tablas se crea manualmente con los
// scripts SQL ubicados en la carpeta /database/.
// -----------------------------------------------
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await using var conexion = new NpgsqlConnection(cadenaConexion);
        await conexion.OpenAsync();
        logger.LogInformation("✅ Conexión a PostgreSQL establecida correctamente.");
    }
    catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.InvalidPassword)
    {
        logger.LogError(ex,
            "❌ PostgreSQL rechazó la autenticación (28P01). Si no tienes contraseña, configura autenticación local trust en pg_hba.conf o usa un usuario con contraseña.");
    }
    catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.InvalidCatalogName)
    {
        logger.LogError(ex,
            "❌ La base de datos 'tecnoecommerce' no existe (3D000). Ejecuta setup-postgres.ps1 para crear esquema y datos semilla.");
    }
    catch (NpgsqlException ex) when (ex.Message.Contains("No password has been provided", StringComparison.OrdinalIgnoreCase))
    {
        logger.LogError(ex,
            "❌ PostgreSQL exige contraseña (SASL/SCRAM). Debes definir Password en la cadena de conexión o cambiar autenticación local a trust en pg_hba.conf (solo desarrollo).");
    }
    catch (Exception ex)
    {
        logger.LogError(ex,
            "❌ Error al conectar a PostgreSQL. Verifica host, puerto, servicio activo y autenticación local en pg_hba.conf.");
    }
}

// -----------------------------------------------
// Activar Swagger solo en entorno de desarrollo
// -----------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "TecnoEcommerce API v1");
        opciones.RoutePrefix = string.Empty; // Swagger en la ruta raíz "/"
    });
}

// -----------------------------------------------
// Middleware de la aplicación
// -----------------------------------------------
app.UseCors("BlazerPolicy");          // CORS antes de cualquier middleware de auth
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -----------------------------------------------
// Ejecutar la aplicación
// -----------------------------------------------
app.Run();

