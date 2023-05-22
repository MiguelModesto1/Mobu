using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class foto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswHash",
                table: "Utilizador_Registado",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Utilizador_Registado",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Utilizador_Anonimo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 3,
                column: "Fotografia",
                value: null);

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 4,
                column: "Fotografia",
                value: null);

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 5,
                column: "Fotografia",
                value: null);

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "IDUtilizador",
                keyValue: 1,
                column: "Fotografia",
                value: null);

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "IDUtilizador",
                keyValue: 2,
                column: "Fotografia",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Utilizador_Registado",
                newName: "PasswHash");
        }
    }
}
