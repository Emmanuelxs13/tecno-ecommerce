// ================================================================
// TecnoEcommerce - Punto de entrada de la API
// Arquitectura: Modelo - Vista - Controlador (MVC)
// ================================================================

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------
// Agregar controladores MVC
// -----------------------------------------------
builder.Services.AddControllers();

// -----------------------------------------------
// Configurar Swagger para documentar la API
// -----------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------
// TODO Sprint 3: Configurar Entity Framework Core
// builder.Services.AddDbContext<TecnoEcommerceContexto>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionPrincipal")));
// -----------------------------------------------

// -----------------------------------------------
// TODO Sprint 2: Registrar servicios de negocio
// builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
// builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
// builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
// builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
// builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();
// -----------------------------------------------

// -----------------------------------------------
// TODO Sprint 3: Registrar repositorios de datos
// builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
// builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
// builder.Services.AddScoped<ICarritoRepositorio, CarritoRepositorio>();
// builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
// -----------------------------------------------

var app = builder.Build();

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
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -----------------------------------------------
// Ejecutar la aplicación
// -----------------------------------------------
app.Run();
