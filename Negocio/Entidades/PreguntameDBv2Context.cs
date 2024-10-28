using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Negocio.Entidades;

public partial class PreguntameDBv2Context : DbContext
{
    public PreguntameDBv2Context()
    {
    }

    public PreguntameDBv2Context(DbContextOptions<PreguntameDBv2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<MeGusta> MeGustas { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Seguimiento> Seguimientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeGusta>(entity =>
        {
            entity.HasKey(e => new { e.RespuestaId, e.UsuarioId });

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Respuesta).WithMany(p => p.MeGusta)
                .HasForeignKey(d => d.RespuestaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeGustas_Respuestas");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MeGusta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeGustas_Usuarios");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("Notificaciones");

            entity.HasKey(e => new { e.NotificacionId, e.UsuarioId });

            entity.ToTable(tb => tb.HasTrigger("trg_Notificacion_FechaVista"));

            entity.HasIndex(e => e.MRespuestaId, "IX_Notificaciones_M_RespuestaId");

            entity.HasIndex(e => e.MUsuarioId, "IX_Notificaciones_M_UsuarioId");

            entity.HasIndex(e => e.SUsuarioSeguido, "IX_Notificaciones_S_Usuario_Seguido");

            entity.HasIndex(e => e.SUsuarioSeguidor, "IX_Notificaciones_S_Usuario_Seguidor");

            entity.HasIndex(e => e.UsuarioId, "IX_Notificaciones_UsuarioId");

            entity.HasIndex(e => new { e.MRespuestaId, e.MUsuarioId, e.Tipo }, "UQ_Notificaciones_MeGustas").IsUnique();

            entity.HasIndex(e => new { e.SUsuarioSeguido, e.SUsuarioSeguidor, e.Tipo }, "UQ_Notificaciones_Seguimientos").IsUnique();

            entity.Property(e => e.NotificacionId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVista).HasColumnType("datetime");
            entity.Property(e => e.MRespuestaId).HasColumnName("M_RespuestaId");
            entity.Property(e => e.MUsuarioId).HasColumnName("M_UsuarioId");
            entity.Property(e => e.SUsuarioSeguido).HasColumnName("S_Usuario_Seguido");
            entity.Property(e => e.SUsuarioSeguidor).HasColumnName("S_Usuario_Seguidor");
            entity.Property(e => e.Tipo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");

            entity.HasOne(d => d.MeGusta).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => new { d.MRespuestaId, d.MUsuarioId })
                .HasConstraintName("FK_Notificaciones_MeGustas");

            entity.HasOne(d => d.Seguimiento).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => new { d.SUsuarioSeguido, d.SUsuarioSeguidor })
                .HasConstraintName("FK_Notificaciones_Seguimientos");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.ToTable("Paises");

            entity.HasKey(e => e.PaisId);

            entity.Property(e => e.PaisId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasIndex(e => e.UsuarioEnvia, "IX_FK_Pregunta_Usuario_Envia");

            entity.HasIndex(e => e.UsuarioRecibe, "IX_FK_Pregunta_Usuario_Recibe");

            entity.Property(e => e.PreguntaId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Dsc).HasMaxLength(300);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioEnvia).HasColumnName("Usuario_Envia");
            entity.Property(e => e.UsuarioRecibe).HasColumnName("Usuario_Recibe");

            entity.HasOne(d => d.UsuarioEnviaNavigation).WithMany(p => p.PreguntaUsuarioEnviaNavigations)
                .HasForeignKey(d => d.UsuarioEnvia)
                .HasConstraintName("FK_Pregunta_UsuEnvia");

            entity.HasOne(d => d.UsuarioRecibeNavigation).WithMany(p => p.PreguntaUsuarioRecibeNavigations)
                .HasForeignKey(d => d.UsuarioRecibe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pregunta_UsuRecibe");
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasIndex(e => e.PreguntaId, "IX_FK_Respuesta_PreId");

            entity.HasIndex(e => e.PreguntaId, "UQ_PreguntaId").IsUnique();

            entity.Property(e => e.RespuestaId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Dsc).HasMaxLength(300);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nlikes).HasColumnName("NLikes");

            entity.HasOne(d => d.Pregunta).WithOne(p => p.Respuesta)
                .HasForeignKey<Respuesta>(d => d.PreguntaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Respuestas_Preguntas");
        });

        modelBuilder.Entity<Seguimiento>(entity =>
        {
            entity.HasKey(e => new { e.UsuarioSeguido, e.UsuarioSeguidor });

            entity.Property(e => e.UsuarioSeguido).HasColumnName("Usuario_Seguido");
            entity.Property(e => e.UsuarioSeguidor).HasColumnName("Usuario_Seguidor");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UsuarioSeguidoNavigation).WithMany(p => p.SeguimientoUsuarioSeguidoNavigations)
                .HasForeignKey(d => d.UsuarioSeguido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguimiento_Usuario_Seguido");

            entity.HasOne(d => d.UsuarioSeguidorNavigation).WithMany(p => p.SeguimientoUsuarioSeguidorNavigations)
                .HasForeignKey(d => d.UsuarioSeguidor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguimiento_Usuario_Seguidor");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_Usuario_Confirmado_EntoncesActivo"));

            entity.HasIndex(e => e.NombreCompleto, "IX_Usuarios_NombreCompleto");

            entity.HasIndex(e => e.NombreUsuario, "IX_Usuarios_NombreUsuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0EE526D57").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534BDEFF29F").IsUnique();

            entity.Property(e => e.UsuarioId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Activo).HasDefaultValue(false);
            entity.Property(e => e.Apellido).HasMaxLength(30);
            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.CajaPreguntas).HasMaxLength(50);
            entity.Property(e => e.Confirmado).HasDefaultValue(false);
            entity.Property(e => e.Contrasena).HasMaxLength(70);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Foto).HasMaxLength(250);
            entity.Property(e => e.Nlikes).HasColumnName("NLikes");
            entity.Property(e => e.Nombre).HasMaxLength(20);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(51)
                .HasComputedColumnSql("(([Nombre]+' ')+[Apellido])", true);
            entity.Property(e => e.NombreUsuario).HasMaxLength(20);
            entity.Property(e => e.Nseguidores).HasColumnName("NSeguidores");
            entity.Property(e => e.PaisId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Pais).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Pais");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
