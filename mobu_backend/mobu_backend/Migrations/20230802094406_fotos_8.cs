using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotografia_Registado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fotografia_Registado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistadoFK = table.Column<int>(type: "int", nullable: false),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Registado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                        column: x => x.RegistadoFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                unique: true);
        }
    }
}
