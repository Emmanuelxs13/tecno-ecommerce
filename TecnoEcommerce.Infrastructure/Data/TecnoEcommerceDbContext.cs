using Microsoft.EntityFrameworkCore;
using TecnoEcommerce.Domain.Entities;

namespace TecnoEcommerce.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos para TecnoEcommerce usando Entity Framework Core
/// </summary>
public class TecnoEcommerceDbContext : DbContext
{
    public TecnoEcommerceDbContext(DbContextOptions<TecnoEcommerceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Carrito> Carritos => Set<Carrito>();
    public DbSet<ItemCarrito> ItemsCarrito => Set<ItemCarrito>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Rol).IsRequired();
        });

        // Configuración de Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
        });

        // Configuración de Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Stock).IsRequired();
            
            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Carrito
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UsuarioId).IsRequired();
            entity.Property(e => e.FechaCreacion).IsRequired();
            
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Items)
                .WithOne(i => i.Carrito)
                .HasForeignKey(i => i.CarritoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de ItemCarrito
        modelBuilder.Entity<ItemCarrito>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).IsRequired();
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");
            
            entity.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UsuarioId).IsRequired();
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Estado).IsRequired();
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.DireccionEnvio).IsRequired().HasMaxLength(500);
            
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de DetallePedido
        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).IsRequired();
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            
            entity.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
