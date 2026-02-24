// ============================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================

var builder = WebApplication.CreateBuilder(args);

// Agregar Controllers
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO Sprint 3: Configurar Entity Framework Core
// TODO Sprint 3: Configurar inyección de dependencias de repositorios
// TODO Sprint 2-4: Configurar inyección de dependencias de servicios

var app = builder.Build();

// ============================================================
// CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARE
// ============================================================

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TecnoEcommerce API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ============================================================
// EJECUTAR APLICACIÓN
// ============================================================

app.Run();
