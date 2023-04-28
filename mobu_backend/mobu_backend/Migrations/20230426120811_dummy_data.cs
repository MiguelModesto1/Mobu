using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class dummy_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Utilizador_Anonimo",
                columns: new[] { "ID_Utilizador", "EnderecoIP", "NomeUtilizador" },
                values: new object[,]
                {
                    { 3, "192.168.1.1", "teste3" },
                    { 4, "192.168.1.2", "teste4" }
                });

            migrationBuilder.InsertData(
                table: "Utilizador_Registado",
                columns: new[] { "ID_Utilizador", "Email", "NomeUtilizador", "passwHash" },
                values: new object[,]
                {
                    { 1, "teste1@teste.com", "teste1", "?~T?????%??v.K?p?>???GDo\n7b?A???%?????\"??`????*" },
                    { 2, "teste2@teste.com", "teste2", "?~T?????%??v.K?p?>???GDo\n7b?A???%?????\"??`????*" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Utilizador_Anonimo",
                keyColumn: "ID_Utilizador",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2);
        }
    }
}
