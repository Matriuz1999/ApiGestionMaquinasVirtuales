using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionMaquinasVirtuales.Models;

public partial class GestionmvContext : DbContext
{
    public GestionmvContext()
    {
    }

    public GestionmvContext(DbContextOptions<GestionmvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MaquinasVirtuale> MaquinasVirtuales { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaquinasVirtuale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Maquinas__3214EC07836474C4");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Os)
                .HasMaxLength(100)
                .HasColumnName("OS");
            entity.Property(e => e.Ram).HasColumnName("RAM");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC078BA0D4E4");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105347FA11858").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
