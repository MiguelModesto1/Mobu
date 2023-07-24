using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.DeleteData(
                table: "Pedidos_Amizade",
                keyColumns: new[] { "DestinatarioFK", "RemetenteFK" },
                keyColumnTypes: new[] { "int", "int" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DropColumn(
                name: "DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.AddColumn<int>(
                name: "DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade",
                column: "RemetenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "DestinatarioPedidoIDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "DestinatarioPedidoIDUtilizador",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_Amizade_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.DropColumn(
                name: "DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.AddColumn<int>(
                name: "DestinatarioFK",
                table: "Pedidos_Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade",
                columns: new[] { "RemetenteFK", "DestinatarioFK" });

            migrationBuilder.InsertData(
                table: "Pedidos_Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK", "EstadoPedido" },
                values: new object[] { 2, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");
        }
    }
}
