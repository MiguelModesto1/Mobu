using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class base_64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyValues: new object[] { 2, 1 },
                column: "Estado_pedido",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1,
                column: "passwHash",
                value: "5H5Uip6ikpYl/K12LkvjcOA+yPDnR0RvHgo3YshBqYj0Jd/jhd70IreUYPjCk+Aq");

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2,
                column: "passwHash",
                value: "5H5Uip6ikpYl/K12LkvjcOA+yPDnR0RvHgo3YshBqYj0Jd/jhd70IreUYPjCk+Aq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyValues: new object[] { 2, 1 },
                column: "Estado_pedido",
                value: 0);

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
    }
}
