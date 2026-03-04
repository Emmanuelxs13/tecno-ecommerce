using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Interfaces;

namespace TecnoEcommerce.API.Repositorios;

/// <summary>
/// Datos semilla de prueba para el entorno de desarrollo.
/// Se cargan en memoria al iniciar la aplicación (Sprint 3).
/// En Sprint 4 serán reemplazados por migraciones de EF Core + PostgreSQL.
/// </summary>
public static class DatosSemilla
{
    /// <summary>
    /// Carga categorías y productos de ejemplo en los repositorios en memoria.
    /// </summary>
    public static async Task CargarAsync(
        IRepositorio<Categoria>  categorias,
        IProductoRepositorio     productos)
    {
        // ── Categorías ────────────────────────────────────────────────────────
        var laptops      = new Categoria { Nombre = "Laptops",      Descripcion = "Portátiles y ultrabooks" };
        var smartphones  = new Categoria { Nombre = "Smartphones",  Descripcion = "Teléfonos inteligentes" };
        var accesorios   = new Categoria { Nombre = "Accesorios",   Descripcion = "Periféricos y complementos" };
        var gaming       = new Categoria { Nombre = "Gaming",       Descripcion = "Hardware y periféricos gamer" };

        await categorias.AgregarAsync(laptops);
        await categorias.AgregarAsync(smartphones);
        await categorias.AgregarAsync(accesorios);
        await categorias.AgregarAsync(gaming);

        // ── Productos ─────────────────────────────────────────────────────────
        await productos.AgregarAsync(new Producto
        {
            Nombre      = "ASUS VivoBook 15",
            Descripcion = "Laptop con Intel Core i5-12500H, 16 GB RAM, SSD 512 GB, pantalla FHD 15.6\"",
            Precio      = 12_499.00m,
            Stock       = 8,
            ImagenUrl   = "https://placehold.co/400x300?text=ASUS+VivoBook",
            CategoriaId = laptops.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "MacBook Air M2",
            Descripcion = "Apple MacBook Air con chip M2, 8 GB RAM unificada, SSD 256 GB, pantalla Liquid Retina 13.6\"",
            Precio      = 24_999.00m,
            Stock       = 5,
            ImagenUrl   = "https://placehold.co/400x300?text=MacBook+Air+M2",
            CategoriaId = laptops.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "iPhone 15 Pro",
            Descripcion = "Apple iPhone 15 Pro, chip A17 Pro, 256 GB, cámara 48 MP, titanio natural",
            Precio      = 22_999.00m,
            Stock       = 12,
            ImagenUrl   = "https://placehold.co/400x300?text=iPhone+15+Pro",
            CategoriaId = smartphones.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "Samsung Galaxy S24",
            Descripcion = "Samsung Galaxy S24, Snapdragon 8 Gen 3, 8 GB RAM, 128 GB, pantalla AMOLED 6.2\"",
            Precio      = 16_499.00m,
            Stock       = 15,
            ImagenUrl   = "https://placehold.co/400x300?text=Galaxy+S24",
            CategoriaId = smartphones.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "Monitor LG UltraWide 29\"",
            Descripcion = "Monitor curvo 29\" UltraWide, resolución 2560×1080, 100 Hz, IPS, HDR10",
            Precio      = 5_299.00m,
            Stock       = 6,
            ImagenUrl   = "https://placehold.co/400x300?text=Monitor+LG",
            CategoriaId = accesorios.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "Teclado Mecánico Keychron K2",
            Descripcion = "Teclado inalámbrico compacto 75%, switches Gateron Red, retroiluminación RGB",
            Precio      = 1_899.00m,
            Stock       = 20,
            ImagenUrl   = "https://placehold.co/400x300?text=Keychron+K2",
            CategoriaId = accesorios.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "GPU NVIDIA RTX 4060",
            Descripcion = "NVIDIA GeForce RTX 4060, 8 GB GDDR6, DLSS 3, ideal para gaming 1080p/1440p",
            Precio      = 8_799.00m,
            Stock       = 4,
            ImagenUrl   = "https://placehold.co/400x300?text=RTX+4060",
            CategoriaId = gaming.Id
        });

        await productos.AgregarAsync(new Producto
        {
            Nombre      = "Audífonos HyperX Cloud III",
            Descripcion = "Audífonos gaming con drivers 53 mm, DTS Headphone:X, micrófono desmontable",
            Precio      = 2_199.00m,
            Stock       = 18,
            ImagenUrl   = "https://placehold.co/400x300?text=HyperX+Cloud+III",
            CategoriaId = gaming.Id
        });
    }
}
