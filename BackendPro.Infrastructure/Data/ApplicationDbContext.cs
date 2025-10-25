using Microsoft.EntityFrameworkCore;
using BackendPro.Core.Entities;

namespace BackendPro.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Director> Directores => Set<Director>();
    public DbSet<Actor> Actores => Set<Actor>();
    public DbSet<PeliculaActor> PeliculasActor => Set<PeliculaActor>();
    public DbSet<Resena> Resenas => Set<Resena>();

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

        // Configure Resena entity
        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Autor).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Comentario).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Calificacion).IsRequired();
            entity.Property(e => e.FechaPublicacion).IsRequired();

            entity.HasOne(e => e.Pelicula)
                  .WithMany(p => p.Resenas)
                  .HasForeignKey(e => e.PeliculaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Genero>().HasData(
            new Genero { Id = 1, Nombre = "Suspenso", Descripcion = "Relatos que mantienen la tensión con giros inesperados y climas intensos." },
            new Genero { Id = 2, Nombre = "Crimen", Descripcion = "Historias centradas en el mundo criminal y sus consecuencias." },
            new Genero { Id = 3, Nombre = "Acción", Descripcion = "Películas llenas de adrenalina, persecuciones y secuencias espectaculares." },
            new Genero { Id = 4, Nombre = "Drama", Descripcion = "Narrativas profundas que exploran conflictos humanos y emocionales." }
        );

        modelBuilder.Entity<Director>().HasData(
            new Director { Id = 1, Nombre = "Christopher Nolan", Nacionalidad = "Reino Unido", FechaNacimiento = new DateTime(1970, 7, 30) },
            new Director { Id = 2, Nombre = "Quentin Tarantino", Nacionalidad = "Estados Unidos", FechaNacimiento = new DateTime(1963, 3, 27) },
            new Director { Id = 3, Nombre = "Martin Scorsese", Nacionalidad = "Estados Unidos", FechaNacimiento = new DateTime(1942, 11, 17) },
            new Director { Id = 4, Nombre = "Denis Villeneuve", Nacionalidad = "Canadá", FechaNacimiento = new DateTime(1967, 10, 3) }
        );

        modelBuilder.Entity<Actor>().HasData(
            new Actor { Id = 1, Nombre = "Leonardo DiCaprio", FechaNacimiento = new DateTime(1974, 11, 11), Biografia = "Actor estadounidense ganador del Óscar, reconocido por sus colaboraciones con Scorsese y Nolan." },
            new Actor { Id = 2, Nombre = "Christian Bale", FechaNacimiento = new DateTime(1974, 1, 30), Biografia = "Actor británico conocido por sus transformaciones físicas y su papel como Batman." },
            new Actor { Id = 3, Nombre = "Uma Thurman", FechaNacimiento = new DateTime(1970, 4, 29), Biografia = "Actriz estadounidense emblemática del cine de Tarantino." },
            new Actor { Id = 4, Nombre = "Samuel L. Jackson", FechaNacimiento = new DateTime(1948, 12, 21), Biografia = "Actor estadounidense con una carrera prolífica en cine y televisión, habitual en las películas de Tarantino." },
            new Actor { Id = 5, Nombre = "Robert De Niro", FechaNacimiento = new DateTime(1943, 8, 17), Biografia = "Leyenda del cine estadounidense, rostro recurrente en la filmografía de Scorsese." },
            new Actor { Id = 6, Nombre = "Cillian Murphy", FechaNacimiento = new DateTime(1976, 5, 25), Biografia = "Actor irlandés conocido por su presencia magnética y su colaboración con Nolan." }
        );

        modelBuilder.Entity<Pelicula>().HasData(
            new Pelicula
            {
                Id = 1,
                Titulo = "Pulp Fiction",
                Sinopsis = "Historias entrelazadas de crimen en Los Ángeles con diálogos afilados y humor negro característicos de Tarantino.",
                Duracion = 154,
                FechaEstreno = new DateTime(1994, 10, 14),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/pulp-fiction.jpg",
                GeneroId = 2,
                DirectorId = 2,
            },
            new Pelicula
            {
                Id = 2,
                Titulo = "The Dark Knight",
                Sinopsis = "Batman enfrenta al Joker en una batalla que pone a prueba la moralidad de Gotham y sus guardianes.",
                Duracion = 152,
                FechaEstreno = new DateTime(2008, 7, 18),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/the-dark-knight.jpg",
                GeneroId = 3,
                DirectorId = 1,
            },
            new Pelicula
            {
                Id = 3,
                Titulo = "Inception",
                Sinopsis = "Un equipo se infiltra en los sueños para implantar ideas, desdibujando los límites entre realidad y subconsciente.",
                Duracion = 148,
                FechaEstreno = new DateTime(2010, 7, 16),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/inception.jpg",
                GeneroId = 1,
                DirectorId = 1,
            },
            new Pelicula
            {
                Id = 4,
                Titulo = "The Departed",
                Sinopsis = "Un infiltrado en la mafia y un topo en la policía juegan al gato y al ratón en Boston.",
                Duracion = 151,
                FechaEstreno = new DateTime(2006, 10, 6),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/the-departed.jpg",
                GeneroId = 2,
                DirectorId = 3,
            },
            new Pelicula
            {
                Id = 5,
                Titulo = "Kill Bill: Volumen 1",
                Sinopsis = "La Novia inicia su venganza feroz contra el Escuadrón Asesino Víbora Mortal tras despertar de un coma.",
                Duracion = 111,
                FechaEstreno = new DateTime(2003, 10, 10),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/kill-bill-vol1.jpg",
                GeneroId = 3,
                DirectorId = 2,
            },
            new Pelicula
            {
                Id = 6,
                Titulo = "Oppenheimer",
                Sinopsis = "El físico J. Robert Oppenheimer lidera el Proyecto Manhattan y confronta el peso moral de crear la bomba atómica.",
                Duracion = 180,
                FechaEstreno = new DateTime(2023, 7, 21),
                ImagenUrl = "https://image.tmdb.org/t/p/w500/oppenheimer.jpg",
                GeneroId = 4,
                DirectorId = 1,
            }
        );

        modelBuilder.Entity<PeliculaActor>().HasData(
            new PeliculaActor { PeliculaId = 1, ActorId = 3 },
            new PeliculaActor { PeliculaId = 1, ActorId = 4 },
            new PeliculaActor { PeliculaId = 2, ActorId = 2 },
            new PeliculaActor { PeliculaId = 2, ActorId = 4 },
            new PeliculaActor { PeliculaId = 3, ActorId = 6 },
            new PeliculaActor { PeliculaId = 3, ActorId = 2 },
            new PeliculaActor { PeliculaId = 4, ActorId = 1 },
            new PeliculaActor { PeliculaId = 4, ActorId = 5 },
            new PeliculaActor { PeliculaId = 5, ActorId = 3 },
            new PeliculaActor { PeliculaId = 6, ActorId = 6 },
            new PeliculaActor { PeliculaId = 6, ActorId = 1 }
        );
    }
}
