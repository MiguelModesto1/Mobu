using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_friend_request_dummy_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pedidos_Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK", "Estado_pedido" },
                values: new object[] { 2, 1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyValues: new object[] { 2, 1 });
        }
    }
}
