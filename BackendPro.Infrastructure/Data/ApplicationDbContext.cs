using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;

namespace BackendPro.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Director> Directores => Set<Director>();
    public DbSet<Actor> Actores => Set<Actor>();
    public DbSet<PeliculaActor> PeliculasActor => Set<PeliculaActor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Pelicula entity
        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Sinopsis).IsRequired();
            entity.Property(e => e.Duracion).IsRequired();
            entity.Property(e => e.FechaEstreno).IsRequired();
            entity.Property(e => e.ImagenUrl).HasMaxLength(500);

            entity.HasOne(e => e.Genero)
                  .WithMany(g => g.Peliculas)
                  .HasForeignKey(e => e.GeneroId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Director)
                  .WithMany(d => d.Peliculas)
                  .HasForeignKey(e => e.DirectorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Genero entity
        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // Configure Director entity
        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Nacionalidad).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).IsRequired();
        });

        // Configure Actor entity
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Biografia).HasMaxLength(1000);
            entity.Property(e => e.FechaNacimiento).IsRequired();
        });

        // Configure PeliculaActor (many-to-many relationship)
        modelBuilder.Entity<PeliculaActor>(entity =>
        {
            entity.HasKey(pa => new { pa.PeliculaId, pa.ActorId });

            entity.HasOne(pa => pa.Pelicula)
                  .WithMany(p => p.PeliculasActor)
                  .HasForeignKey(pa => pa.PeliculaId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pa => pa.Actor)
                  .WithMany(a => a.PeliculasActor)
                  .HasForeignKey(pa => pa.ActorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
