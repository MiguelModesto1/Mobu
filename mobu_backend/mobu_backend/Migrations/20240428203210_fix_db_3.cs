using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosAmizade_UtilizadorRegistado_DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade");

            migrationBuilder.DropIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade");

            migrationBuilder.DropColumn(
                name: "DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_RemetenteFK",
                table: "PedidosAmizade",
                column: "RemetenteFK");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosAmizade_UtilizadorRegistado_RemetenteFK",
                table: "PedidosAmizade",
                column: "RemetenteFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosAmizade_UtilizadorRegistado_RemetenteFK",
                table: "PedidosAmizade");

            migrationBuilder.DropIndex(
                name: "IX_PedidosAmizade_RemetenteFK",
                table: "PedidosAmizade");

            migrationBuilder.AddColumn<int>(
                name: "DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade",
                column: "DonoListaPedidosIDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosAmizade_UtilizadorRegistado_DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade",
                column: "DonoListaPedidosIDUtilizador",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador");
        }
    }
}
