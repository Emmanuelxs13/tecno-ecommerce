// ================================================================
// TecnoEcommerce.Web — Punto de entrada de la aplicación Blazor WASM
// Registra servicios HTTP, servicios de sesión y configura el
// cliente HTTP que consume la API REST.
// ================================================================

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TecnoEcommerce.Web;
using TecnoEcommerce.Web.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ── Leer la URL de la API desde appsettings.json en wwwroot/ ─────
var apiUrl = builder.Configuration["ApiUrl"]
             ?? "http://localhost:5247";

// ── Registrar HttpClient apuntando a la API REST ──────────────────
// Todos los servicios de API inyectan este mismo cliente configurado.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

// ── Servicio de sesión (Singleton: vive mientras el tab esté abierto) ──
// Centraliza el usuario autenticado y el token JWT en memoria.
builder.Services.AddSingleton<SesionServicio>();

// ── Servicios de acceso a la API (Scoped: uno por ciclo de renderizado) ─
builder.Services.AddScoped<ProductoApiServicio>();
builder.Services.AddScoped<CategoriaApiServicio>();
builder.Services.AddScoped<UsuarioApiServicio>();
builder.Services.AddScoped<CarritoApiServicio>();
builder.Services.AddScoped<PedidoApiServicio>();

await builder.Build().RunAsync();
