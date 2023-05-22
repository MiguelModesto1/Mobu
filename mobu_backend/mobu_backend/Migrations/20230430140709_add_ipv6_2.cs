using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_ipv6_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnderecoIP",
                table: "Utilizador_Anonimo",
                newName: "EnderecoIPv6");

            migrationBuilder.AddColumn<string>(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 3,
                columns: new[] { "EnderecoIPv4", "EnderecoIPv6" },
                values: new object[] { "192.168.1.1", "" });

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 4,
                columns: new[] { "EnderecoIPv4", "EnderecoIPv6" },
                values: new object[] { "192.168.1.2", "" });

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 5,
                column: "EnderecoIPv4",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo");

            migrationBuilder.RenameColumn(
                name: "EnderecoIPv6",
                table: "Utilizador_Anonimo",
                newName: "EnderecoIP");

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 3,
                column: "EnderecoIP",
                value: "192.168.1.1");

            migrationBuilder.UpdateData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 4,
                column: "EnderecoIP",
                value: "192.168.1.2");
        }
    }
}
