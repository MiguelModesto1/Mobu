using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class restructure_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistadosSalasJogo");

            migrationBuilder.DropTable(
                name: "UtilizadorAnonimo");

            migrationBuilder.DropTable(
                name: "SalaJogo1Contra1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaJogo1Contra1",
                columns: table => new
                {
                    IDSala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaJogo1Contra1", x => x.IDSala);
                });

            migrationBuilder.CreateTable(
                name: "UtilizadorAnonimo",
                columns: table => new
                {
                    IDUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorAnonimo", x => x.IDUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "RegistadosSalasJogo",
                columns: table => new
                {
                    IDRegisto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    IsFundador = table.Column<bool>(type: "bit", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistadosSalasJogo", x => x.IDRegisto);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasJogo_SalaJogo1Contra1_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "SalaJogo1Contra1",
                        principalColumn: "IDSala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasJogo_UtilizadorRegistado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasJogo_SalaFK",
                table: "RegistadosSalasJogo",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasJogo_UtilizadorFK",
                table: "RegistadosSalasJogo",
                column: "UtilizadorFK");
        }
    }
}
