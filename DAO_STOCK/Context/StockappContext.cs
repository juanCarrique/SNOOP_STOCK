using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public partial class StockappContext : DbContext
    {
        public StockappContext()
        {
        }

        public StockappContext(DbContextOptions<StockappContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }

        public virtual DbSet<Cliente> Clientes { get; set; }

        public virtual DbSet<Composicion> Composicions { get; set; }

        public virtual DbSet<Factura> Facturas { get; set; }

        public virtual DbSet<HistorialMovimiento> HistorialMovimientos { get; set; }

        public virtual DbSet<ItemFactura> ItemFacturas { get; set; }

        public virtual DbSet<Permiso> Permisos { get; set; }

        public virtual DbSet<Producto> Productos { get; set; }

        public virtual DbSet<Proveedor> Proveedors { get; set; }

        public virtual DbSet<Reposicion> Reposicions { get; set; }

        public virtual DbSet<Rol> Rols { get; set; }

        public virtual DbSet<RolPermiso> RolPermisos { get; set; }

        public virtual DbSet<TipoMovimiento> TipoMovimientos { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellido");
                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mail");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<Composicion>(entity =>
            {
                entity.ToTable("Composicion");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.ComponenteId).HasColumnName("componenteId");
                entity.Property(e => e.ProductoId).HasColumnName("productoId");

                entity.HasOne(d => d.Componente).WithMany(p => p.ComposicionComponentes)
                    .HasForeignKey(d => d.ComponenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Composicion_Componente");

                entity.HasOne(d => d.Producto).WithMany(p => p.ComposicionProductos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Composicion_Producto");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("Factura");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ClienteId).HasColumnName("clienteId");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Numero).HasColumnName("numero");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("tipo");
                entity.Property(e => e.Total)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("total");
                entity.Property(e => e.VendedorId).HasColumnName("vendedorId");

                entity.HasOne(d => d.Cliente).WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK_Factura_Cliente");

                entity.HasOne(d => d.Vendedor).WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.VendedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Factura_Vendedor");
            });

            modelBuilder.Entity<HistorialMovimiento>(entity =>
            {
                entity.ToTable("Historial_Movimiento");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.Detalle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("detalle");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.ProductoId).HasColumnName("productoId");
                entity.Property(e => e.TipoId).HasColumnName("tipoId");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

                entity.HasOne(d => d.Producto).WithMany(p => p.HistorialMovimientos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Historial_Producto");

                entity.HasOne(d => d.Tipo).WithMany(p => p.HistorialMovimientos)
                    .HasForeignKey(d => d.TipoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Historial_TipoMovimiento");
            });

            modelBuilder.Entity<ItemFactura>(entity =>
            {
                entity.ToTable("Item_Factura");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.FacturaId).HasColumnName("facturaId");
                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("precio");
                entity.Property(e => e.ProductoId).HasColumnName("productoId");

                entity.HasOne(d => d.Factura).WithMany(p => p.ItemFacturas)
                    .HasForeignKey(d => d.FacturaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemF_Factura");

                entity.HasOne(d => d.Producto).WithMany(p => p.ItemFacturas)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemF_Producto");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.ToTable("Permiso");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");
                entity.Property(e => e.Detalle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("detalle");
                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("precio");
                entity.Property(e => e.Stock).HasColumnName("stock");
                entity.Property(e => e.StockMinimo).HasColumnName("stockMinimo");

                entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_Categoria");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("Proveedor");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mail");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");
            });

            modelBuilder.Entity<Reposicion>(entity =>
            {
                entity.ToTable("Reposicion");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.ProductoId).HasColumnName("productoId");
                entity.Property(e => e.ProveedorId).HasColumnName("proveedorId");

                entity.HasOne(d => d.Producto).WithMany(p => p.Reposicions)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reposicion_Producto");

                entity.HasOne(d => d.Proveedor).WithMany(p => p.Reposicions)
                    .HasForeignKey(d => d.ProveedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reposicion_Proveedor");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.ToTable("Rol_Permiso");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PermisoId).HasColumnName("permisoId");
                entity.Property(e => e.RolId).HasColumnName("rolId");

                entity.HasOne(d => d.Permiso).WithMany(p => p.RolPermisos)
                    .HasForeignKey(d => d.PermisoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RP_Permiso");

                entity.HasOne(d => d.Rol).WithMany(p => p.RolPermisos)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RP_Rol");
            });

            modelBuilder.Entity<TipoMovimiento>(entity =>
            {
                entity.ToTable("Tipo_Movimiento");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellido");
                entity.Property(e => e.Estado)
                    .HasDefaultValue(true)
                    .HasColumnName("estado");
                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mail");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");
                entity.Property(e => e.RolId).HasColumnName("rolId");
                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

