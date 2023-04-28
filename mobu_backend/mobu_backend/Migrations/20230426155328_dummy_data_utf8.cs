using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class dummy_data_utf8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1,
                column: "passwHash",
                value: "�~T�����%��v.K�p�>���GDo\n7b�A���%����\"��`��*");

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2,
                column: "passwHash",
                value: "�~T�����%��v.K�p�>���GDo\n7b�A���%����\"��`��*");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1,
                column: "passwHash",
                value: "?~T?????%??v.K?p?>???GDo\n7b?A???%?????\"??`????*");

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2,
                column: "passwHash",
                value: "?~T?????%??v.K?p?>???GDo\n7b?A???%?????\"??`????*");
        }
    }
}
