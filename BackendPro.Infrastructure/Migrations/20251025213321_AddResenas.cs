using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendPro.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddResenas : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Resenas",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PeliculaId = table.Column<int>(type: "int", nullable: false),
                Autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Calificacion = table.Column<int>(type: "int", nullable: false),
                FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Resenas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Resenas_Peliculas_PeliculaId",
                    column: x => x.PeliculaId,
                    principalTable: "Peliculas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Resenas_PeliculaId",
            table: "Resenas",
            column: "PeliculaId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Resenas");
    }
}
