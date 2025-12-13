using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaEspaco.Models.ViewModels;

namespace SistemaEspaco.Models;

public partial class ProjetoEspacoContext : DbContext
{
    public ProjetoEspacoContext()
    {
    }

    public ProjetoEspacoContext(DbContextOptions<ProjetoEspacoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Espaco> Espacos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // => optionsBuilder.UseSqlServer("Server=DESKTOP-9BK1MSS\\SQLEXPRESS; Database=ProjetoEspaco; User id=sa; Password=123456; trustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Espaco>(entity =>
        {
            entity.HasKey(e => e.IdEspaco).HasName("PK__Espaco__1EDE22F2CFB9AFFA");

            entity.ToTable("Espaco");

            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reserva__0E49C69D01877F98");

            entity.ToTable("Reserva");

            entity.Property(e => e.DataFim).HasColumnType("datetime");
            entity.Property(e => e.DataInicio).HasColumnType("datetime");
            entity.Property(e => e.Situacao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Pendente");

            entity.HasOne(d => d.IdEspacoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEspaco)
                .HasConstraintName("FK__Reserva__IdEspac__4F7CD00D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Reserva__IdUsuar__4E88ABD4");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF979A7A72BD");

            entity.ToTable("Usuario");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<SistemaEspaco.Models.ViewModels.DashboardReserva> DashboardReservaViewModel { get; set; } = default!;
}
