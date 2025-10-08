using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFilmSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actores",
                columns: new[] { "Id", "Biografia", "FechaNacimiento", "Nombre" },
                values: new object[,]
                {
                    { 1, "Actor estadounidense ganador del Óscar, reconocido por sus colaboraciones con Scorsese y Nolan.", new DateTime(1974, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leonardo DiCaprio" },
                    { 2, "Actor británico conocido por sus transformaciones físicas y su papel como Batman.", new DateTime(1974, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christian Bale" },
                    { 3, "Actriz estadounidense emblemática del cine de Tarantino.", new DateTime(1970, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Uma Thurman" },
                    { 4, "Actor estadounidense con una carrera prolífica en cine y televisión, habitual en las películas de Tarantino.", new DateTime(1948, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samuel L. Jackson" },
                    { 5, "Leyenda del cine estadounidense, rostro recurrente en la filmografía de Scorsese.", new DateTime(1943, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Robert De Niro" },
                    { 6, "Actor irlandés conocido por su presencia magnética y su colaboración con Nolan.", new DateTime(1976, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cillian Murphy" }
                });

            migrationBuilder.InsertData(
                table: "Directores",
                columns: new[] { "Id", "FechaNacimiento", "Nacionalidad", "Nombre" },
                values: new object[,]
                {
                    { 1, new DateTime(1970, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reino Unido", "Christopher Nolan" },
                    { 2, new DateTime(1963, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estados Unidos", "Quentin Tarantino" },
                    { 3, new DateTime(1942, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estados Unidos", "Martin Scorsese" },
                    { 4, new DateTime(1967, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canadá", "Denis Villeneuve" }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Relatos que mantienen la tensión con giros inesperados y climas intensos.", "Suspenso" },
                    { 2, "Historias centradas en el mundo criminal y sus consecuencias.", "Crimen" },
                    { 3, "Películas llenas de adrenalina, persecuciones y secuencias espectaculares.", "Acción" },
                    { 4, "Narrativas profundas que exploran conflictos humanos y emocionales.", "Drama" }
                });

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Id", "DirectorId", "Duracion", "FechaEstreno", "GeneroId", "ImagenUrl", "Sinopsis", "Titulo" },
                values: new object[,]
                {
                    { 1, 2, 154, new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "https://image.tmdb.org/t/p/w500/pulp-fiction.jpg", "Historias entrelazadas de crimen en Los Ángeles con diálogos afilados y humor negro característicos de Tarantino.", "Pulp Fiction" },
                    { 2, 1, 152, new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "https://image.tmdb.org/t/p/w500/the-dark-knight.jpg", "Batman enfrenta al Joker en una batalla que pone a prueba la moralidad de Gotham y sus guardianes.", "The Dark Knight" },
                    { 3, 1, 148, new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://image.tmdb.org/t/p/w500/inception.jpg", "Un equipo se infiltra en los sueños para implantar ideas, desdibujando los límites entre realidad y subconsciente.", "Inception" },
                    { 4, 3, 151, new DateTime(2006, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "https://image.tmdb.org/t/p/w500/the-departed.jpg", "Un infiltrado en la mafia y un topo en la policía juegan al gato y al ratón en Boston.", "The Departed" },
                    { 5, 2, 111, new DateTime(2003, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "https://image.tmdb.org/t/p/w500/kill-bill-vol1.jpg", "La Novia inicia su venganza feroz contra el Escuadrón Asesino Víbora Mortal tras despertar de un coma.", "Kill Bill: Volumen 1" },
                    { 6, 1, 180, new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "https://image.tmdb.org/t/p/w500/oppenheimer.jpg", "El físico J. Robert Oppenheimer lidera el Proyecto Manhattan y confronta el peso moral de crear la bomba atómica.", "Oppenheimer" }
                });

            migrationBuilder.InsertData(
                table: "PeliculasActor",
                columns: new[] { "ActorId", "PeliculaId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 1 },
                    { 2, 2 },
                    { 4, 2 },
                    { 2, 3 },
                    { 6, 3 },
                    { 1, 4 },
                    { 5, 4 },
                    { 3, 5 },
                    { 1, 6 },
                    { 6, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Directores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "PeliculasActor",
                keyColumns: new[] { "ActorId", "PeliculaId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Actores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Peliculas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Directores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Directores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Directores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
