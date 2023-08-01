using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_pw_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "IDUtilizador",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Utilizador_Registado",
                keyColumn: "IDUtilizador",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Utilizador_Registado",
                keyColumn: "IDUtilizador",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Utilizador_Registado",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(96)",
                oldMaxLength: 96);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(96)",
                oldMaxLength: 96);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Utilizador_Registado",
                type: "nvarchar(96)",
                maxLength: 96,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admin",
                type: "nvarchar(96)",
                maxLength: 96,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Utilizador_Anonimo",
                columns: new[] { "IDUtilizador", "EnderecoIPv4", "EnderecoIPv6", "Fotografia", "NomeUtilizador" },
                values: new object[,]
                {
                    { 3, "192.168.1.1", "", null, "guest3" },
                    { 4, "192.168.1.2", "", null, "guest4" },
                    { 5, "", "2001:818:dfba:c100:1464:bee0:19fb:f940", null, "guest5" }
                });

            migrationBuilder.InsertData(
                table: "Utilizador_Registado",
                columns: new[] { "IDUtilizador", "Email", "Fotografia", "NomeUtilizador", "Password" },
                values: new object[,]
                {
                    { 1, "teste1@teste.com", null, "teste1", "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A" },
                    { 2, "teste2@teste.com", null, "teste2", "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A" }
                });
        }
    }
}
