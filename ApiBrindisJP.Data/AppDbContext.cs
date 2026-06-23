using ApiBrindisJP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBrindisJP.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
            entity.Property(e => e.NombreUsuario)
                .HasColumnName("Usuario")
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(e => e.NombreUsuario).IsUnique();
            entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("NOW()");

            entity.HasData(
                new Usuario
                {
                    IdUsuario = 1,
                    NombreUsuario = "admin",
                    PasswordHash = "123456",
                    NombreCompleto = "Administrador",
                    Activo = true,
                    FechaCreacion = seedDate
                });
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.Clave);
            entity.Property(e => e.Clave).HasMaxLength(10);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("NOW()");

            entity.HasData(
                new Cliente
                {
                    Clave = "0001",
                    Nombre = "Herculano Pérez",
                    Edad = 25,
                    FechaNacimiento = new DateOnly(2001, 5, 10),
                    FechaRegistro = seedDate
                },
                new Cliente
                {
                    Clave = "0002",
                    Nombre = "Ernerdina Eréndira",
                    Edad = 30,
                    FechaNacimiento = new DateOnly(1996, 2, 15),
                    FechaRegistro = seedDate
                });
        });
    }
}
