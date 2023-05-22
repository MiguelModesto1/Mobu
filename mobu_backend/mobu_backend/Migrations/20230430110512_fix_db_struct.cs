using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_struct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 3,
                column: "NomeUtilizador",
                value: "guest3");

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 4,
                column: "NomeUtilizador",
                value: "guest4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 3,
                column: "NomeUtilizador",
                value: "teste3");

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 4,
                column: "NomeUtilizador",
                value: "teste4");
        }
    }
}
