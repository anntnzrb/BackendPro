using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendPro.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Actores",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Biografia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Actores", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Directores",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Nacionalidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Directores", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Generos",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Generos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Peliculas",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Duracion = table.Column<int>(type: "int", nullable: false),
                FechaEstreno = table.Column<DateTime>(type: "datetime2", nullable: false),
                ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                GeneroId = table.Column<int>(type: "int", nullable: false),
                DirectorId = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Peliculas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Peliculas_Directores_DirectorId",
                    column: x => x.DirectorId,
                    principalTable: "Directores",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Peliculas_Generos_GeneroId",
                    column: x => x.GeneroId,
                    principalTable: "Generos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PeliculasActor",
            columns: table => new
            {
                PeliculaId = table.Column<int>(type: "int", nullable: false),
                ActorId = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PeliculasActor", x => new { x.PeliculaId, x.ActorId });
                table.ForeignKey(
                    name: "FK_PeliculasActor_Actores_ActorId",
                    column: x => x.ActorId,
                    principalTable: "Actores",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PeliculasActor_Peliculas_PeliculaId",
                    column: x => x.PeliculaId,
                    principalTable: "Peliculas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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
                { 6, "Actor irlandés conocido por su presencia magnética y su colaboración con Nolan.", new DateTime(1976, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cillian Murphy" },
            });

        migrationBuilder.InsertData(
            table: "Directores",
            columns: new[] { "Id", "FechaNacimiento", "Nacionalidad", "Nombre" },
            values: new object[,]
            {
                { 1, new DateTime(1970, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reino Unido", "Christopher Nolan" },
                { 2, new DateTime(1963, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estados Unidos", "Quentin Tarantino" },
                { 3, new DateTime(1942, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estados Unidos", "Martin Scorsese" },
                { 4, new DateTime(1967, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canadá", "Denis Villeneuve" },
            });

        migrationBuilder.InsertData(
            table: "Generos",
            columns: new[] { "Id", "Descripcion", "Nombre" },
            values: new object[,]
            {
                { 1, "Relatos que mantienen la tensión con giros inesperados y climas intensos.", "Suspenso" },
                { 2, "Historias centradas en el mundo criminal y sus consecuencias.", "Crimen" },
                { 3, "Películas llenas de adrenalina, persecuciones y secuencias espectaculares.", "Acción" },
                { 4, "Narrativas profundas que exploran conflictos humanos y emocionales.", "Drama" },
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
                { 6, 1, 180, new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "https://image.tmdb.org/t/p/w500/oppenheimer.jpg", "El físico J. Robert Oppenheimer lidera el Proyecto Manhattan y confronta el peso moral de crear la bomba atómica.", "Oppenheimer" },
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
                { 6, 6 },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Generos_Nombre",
            table: "Generos",
            column: "Nombre",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Peliculas_DirectorId",
            table: "Peliculas",
            column: "DirectorId");

        migrationBuilder.CreateIndex(
            name: "IX_Peliculas_GeneroId",
            table: "Peliculas",
            column: "GeneroId");

        migrationBuilder.CreateIndex(
            name: "IX_PeliculasActor_ActorId",
            table: "PeliculasActor",
            column: "ActorId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PeliculasActor");

        migrationBuilder.DropTable(
            name: "Actores");

        migrationBuilder.DropTable(
            name: "Peliculas");

        migrationBuilder.DropTable(
            name: "Directores");

        migrationBuilder.DropTable(
            name: "Generos");
    }
}
