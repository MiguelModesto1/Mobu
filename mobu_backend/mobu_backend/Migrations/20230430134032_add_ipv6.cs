using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_ipv6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIP",
                table: "Utilizador_Anonimo",
                type: "nvarchar(39)",
                maxLength: 39,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "Utilizador_Anonimo",
                columns: new[] { "ID_Utilizador", "EnderecoIP", "NomeUtilizador" },
                values: new object[] { 5, "2001:818:dfba:c100:1464:bee0:19fb:f940", "guest5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIP",
                table: "Utilizador_Anonimo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(39)",
                oldMaxLength: 39);
        }
    }
}
