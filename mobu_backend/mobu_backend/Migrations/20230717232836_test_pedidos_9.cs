using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyValues: new object[] { 2, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pedidos_Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK", "EstadoPedido" },
                values: new object[] { 2, 1, 1 });
        }
    }
}
