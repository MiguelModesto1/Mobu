using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.RenameColumn(
                name: "DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                newName: "Utilizador_RegistadoIDUtilizador");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_Amizade_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                newName: "IX_Pedidos_Amizade_Utilizador_RegistadoIDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "Utilizador_RegistadoIDUtilizador",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.RenameColumn(
                name: "Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                newName: "DestinatarioPedidoIDUtilizador");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_Amizade_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                newName: "IX_Pedidos_Amizade_DestinatarioPedidoIDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioPedidoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "DestinatarioPedidoIDUtilizador",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");
        }
    }
}
