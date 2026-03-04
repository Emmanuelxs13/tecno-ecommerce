// ================================================================
// TecnoEcommerce - Punto de entrada de la API
// Arquitectura: Modelo - Vista - Controlador (MVC)
// ================================================================
using TecnoEcommerce.API.Repositorios;
using TecnoEcommerce.API.Servicios;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------
// Agregar controladores MVC
// -----------------------------------------------
builder.Services.AddControllers();

// -----------------------------------------------
// Configurar CORS para permitir peticiones desde
// el cliente Blazor WebAssembly (Sprint 3).
// En producción restrict los orígenes al dominio real.
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
// Repositorios en memoria (Singleton para que los datos
// persistan durante toda la sesión de prueba).
// Sprint 4 reemplazará estas implementaciones con EF Core + PostgreSQL.
// -----------------------------------------------
builder.Services.AddSingleton<IRepositorio<Usuario>>(
    new RepositorioEnMemoria<Usuario>(u => u.Id, (u, id) => u.Id = id));
builder.Services.AddSingleton<IRepositorio<Categoria>>(
    new RepositorioEnMemoria<Categoria>(c => c.Id, (c, id) => c.Id = id));
builder.Services.AddSingleton<IRepositorio<Envio>, EnvioRepositorioEnMemoria>();
builder.Services.AddSingleton<IProductoRepositorio, ProductoRepositorioEnMemoria>();
builder.Services.AddSingleton<ICarritoRepositorio, CarritoRepositorioEnMemoria>();
builder.Services.AddSingleton<IPedidoRepositorio, PedidoRepositorioEnMemoria>();

// -----------------------------------------------
// Servicios de negocio (Scoped: una instancia por petición HTTP).
// Principio DIP: se registran las interfaces, no las implementaciones concretas.
// -----------------------------------------------
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();
builder.Services.AddScoped<IPagoServicio, PagoSimuladoServicio>();
builder.Services.AddScoped<IEnvioServicio, EnvioSimuladoServicio>();

var app = builder.Build();

// -----------------------------------------------
// Cargar datos iniciales de prueba (categorías + productos)
// -----------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var categorias = scope.ServiceProvider.GetRequiredService<IRepositorio<Categoria>>();
    var productos  = scope.ServiceProvider.GetRequiredService<IProductoRepositorio>();
    await DatosSemilla.CargarAsync(categorias, productos);
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
