using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Modelos.Entidades;
using TecnoEcommerce.Modelos.Enumeraciones;

namespace TecnoEcommerce.Datos.Contexto;

/// <summary>
/// Contexto principal de Entity Framework Core para TecnoEcommerce.
/// Mapea todas las entidades del dominio a la base de datos PostgreSQL.
/// Principio SRP: solo gestiona la configuración del acceso a datos;
/// la lógica de negocio queda en los servicios.
/// </summary>
public class TiendaContexto : DbContext
{
    // ── DbSets (tablas) ────────────────────────────────────────────────────────

    public DbSet<Usuario>       Usuarios       { get; set; } = null!;
    public DbSet<Categoria>     Categorias     { get; set; } = null!;
    public DbSet<Producto>      Productos      { get; set; } = null!;
    public DbSet<Carrito>       Carritos       { get; set; } = null!;
    public DbSet<ItemCarrito>   ItemsCarrito   { get; set; } = null!;
    public DbSet<Pedido>        Pedidos        { get; set; } = null!;
    public DbSet<DetallePedido> DetallesPedido { get; set; } = null!;
    public DbSet<Envio>         Envios         { get; set; } = null!;
    public DbSet<Resenia>       Resenias       { get; set; } = null!;
    public DbSet<Pago>          Pagos          { get; set; } = null!;

    // ── Constructor ────────────────────────────────────────────────────────────

    public TiendaContexto(DbContextOptions<TiendaContexto> options) : base(options) { }

    // ── Configuración del modelo ───────────────────────────────────────────────

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Nombres de tabla en snake_case (convención PostgreSQL) ────────────
        modelBuilder.Entity<Usuario>       ().ToTable("usuarios");
        modelBuilder.Entity<Categoria>     ().ToTable("categorias");
        modelBuilder.Entity<Producto>      ().ToTable("productos");
        modelBuilder.Entity<Carrito>       ().ToTable("carritos");
        modelBuilder.Entity<ItemCarrito>   ().ToTable("items_carrito");
        modelBuilder.Entity<Pedido>        ().ToTable("pedidos");
        modelBuilder.Entity<DetallePedido> ().ToTable("detalles_pedido");
        modelBuilder.Entity<Envio>         ().ToTable("envios");
        modelBuilder.Entity<Resenia>       ().ToTable("resenias");
        modelBuilder.Entity<Pago>          ().ToTable("pagos");

        // ── USUARIO ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(u => u.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(u => u.Email).HasColumnName("email").HasMaxLength(200).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.ContrasenaHash).HasColumnName("contrasena_hash").HasMaxLength(500).IsRequired();
            e.Property(u => u.Rol).HasColumnName("rol").HasConversion<short>();

            // Relaciones navegación
            e.HasMany(u => u.Pedidos).WithOne(p => p.Usuario).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(u => u.Carrito).WithOne(c => c.Usuario).HasForeignKey<Carrito>(c => c.UsuarioId).OnDelete(DeleteBehavior.Cascade);
        });

        // ── CATEGORIA ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            e.Property(c => c.Descripcion).HasColumnName("descripcion").IsRequired().HasDefaultValue(string.Empty);

            e.HasMany(c => c.Productos).WithOne(p => p.Categoria).HasForeignKey(p => p.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── PRODUCTO ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Producto>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(p => p.Nombre).HasColumnName("nombre").HasMaxLength(200).IsRequired();
            e.Property(p => p.Descripcion).HasColumnName("descripcion").IsRequired().HasDefaultValue(string.Empty);
            e.Property(p => p.Precio).HasColumnName("precio").HasColumnType("numeric(10,2)");
            e.Property(p => p.Stock).HasColumnName("stock");
            e.Property(p => p.ImagenUrl).HasColumnName("imagen_url").HasMaxLength(500).HasDefaultValue(string.Empty);
            e.Property(p => p.CategoriaId).HasColumnName("categoria_id");

            e.HasIndex(p => p.CategoriaId).HasDatabaseName("ix_productos_categoria");
        });

        // ── CARRITO ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Carrito>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(c => c.FechaCreacion).HasColumnName("fecha_creacion");
            e.Property(c => c.UsuarioId).HasColumnName("usuario_id");
            e.HasIndex(c => c.UsuarioId).IsUnique();

            e.HasMany(c => c.Items).WithOne(i => i.Carrito).HasForeignKey(i => i.CarritoId).OnDelete(DeleteBehavior.Cascade);
        });

        // ── ITEM CARRITO ──────────────────────────────────────────────────────
        modelBuilder.Entity<ItemCarrito>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(i => i.Cantidad).HasColumnName("cantidad");
            e.Property(i => i.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("numeric(10,2)");
            e.Property(i => i.CarritoId).HasColumnName("carrito_id");
            e.Property(i => i.ProductoId).HasColumnName("producto_id");

            // Un producto aparece una sola vez por carrito
            e.HasIndex(i => new { i.CarritoId, i.ProductoId }).IsUnique().HasDatabaseName("uq_item_carrito_producto");
            e.HasIndex(i => i.CarritoId).HasDatabaseName("ix_items_carrito_carrito");
            e.HasIndex(i => i.ProductoId).HasDatabaseName("ix_items_carrito_producto");

            e.HasOne(i => i.Producto).WithMany().HasForeignKey(i => i.ProductoId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── PEDIDO ────────────────────────────────────────────────────────────
        modelBuilder.Entity<Pedido>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(p => p.FechaCreacion).HasColumnName("fecha_creacion");
            e.Property(p => p.Total).HasColumnName("total").HasColumnType("numeric(10,2)");
            e.Property(p => p.EstadoPedido).HasColumnName("estado_pedido").HasConversion<short>();
            e.Property(p => p.EstadoPago).HasColumnName("estado_pago").HasConversion<short>();
            e.Property(p => p.UsuarioId).HasColumnName("usuario_id");

            e.HasIndex(p => p.UsuarioId).HasDatabaseName("ix_pedidos_usuario");
            e.HasMany(p => p.Detalles).WithOne(d => d.Pedido).HasForeignKey(d => d.PedidoId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(p => p.Envio).WithOne(en => en.Pedido).HasForeignKey<Envio>(en => en.PedidoId).OnDelete(DeleteBehavior.Cascade);
        });

        // ── DETALLE PEDIDO ────────────────────────────────────────────────────
        modelBuilder.Entity<DetallePedido>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(d => d.Cantidad).HasColumnName("cantidad");
            e.Property(d => d.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("numeric(10,2)");
            e.Property(d => d.PedidoId).HasColumnName("pedido_id");
            e.Property(d => d.ProductoId).HasColumnName("producto_id");

            e.HasIndex(d => d.PedidoId).HasDatabaseName("ix_detalles_pedido_pedido");
            e.HasIndex(d => d.ProductoId).HasDatabaseName("ix_detalles_pedido_producto");
            e.HasOne(d => d.Producto).WithMany().HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── ENVIO ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Envio>(e =>
        {
            e.HasKey(en => en.Id);
            e.Property(en => en.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(en => en.Direccion).HasColumnName("direccion").HasMaxLength(500).IsRequired();
            e.Property(en => en.Transportista).HasColumnName("transportista").HasMaxLength(100).HasDefaultValue(string.Empty);
            e.Property(en => en.EstadoEnvio).HasColumnName("estado_envio").HasConversion<short>();
            e.Property(en => en.FechaEstimadaEntrega).HasColumnName("fecha_estimada_entrega");
            e.Property(en => en.PedidoId).HasColumnName("pedido_id");
        });

        // ── RESENIA ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Resenia>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(r => r.Calificacion).HasColumnName("calificacion");
            e.Property(r => r.Comentario).HasColumnName("comentario").HasDefaultValue(string.Empty);
            e.Property(r => r.FechaCreacion).HasColumnName("fecha_creacion");
            e.Property(r => r.UsuarioId).HasColumnName("usuario_id");
            e.Property(r => r.ProductoId).HasColumnName("producto_id");

            e.HasIndex(r => new { r.UsuarioId, r.ProductoId }).IsUnique().HasDatabaseName("uq_resenia_usuario_producto");
            e.HasIndex(r => r.ProductoId).HasDatabaseName("ix_resenias_producto");

            e.HasOne(r => r.Usuario).WithMany().HasForeignKey(r => r.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.Producto).WithMany().HasForeignKey(r => r.ProductoId).OnDelete(DeleteBehavior.Cascade);
        });

        // ── PAGO ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<Pago>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            e.Property(p => p.Monto).HasColumnName("monto").HasColumnType("numeric(10,2)");
            e.Property(p => p.Metodo).HasColumnName("metodo").HasMaxLength(50).IsRequired();
            e.Property(p => p.Referencia).HasColumnName("referencia").HasMaxLength(200).HasDefaultValue(string.Empty);
            e.Property(p => p.FechaPago).HasColumnName("fecha_pago");
            e.Property(p => p.Estado).HasColumnName("estado").HasConversion<short>();
            e.Property(p => p.PedidoId).HasColumnName("pedido_id");

            e.HasOne(p => p.Pedido).WithMany().HasForeignKey(p => p.PedidoId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
