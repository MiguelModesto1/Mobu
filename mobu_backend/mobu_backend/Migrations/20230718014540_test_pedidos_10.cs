using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.InsertData(
                table: "Pedidos_Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK", "EstadoPedido" },
                values: new object[] { 2, 1, 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.DeleteData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
