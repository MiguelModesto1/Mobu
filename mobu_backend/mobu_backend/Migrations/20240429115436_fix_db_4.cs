using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosAmizade",
                table: "PedidosAmizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade");

            migrationBuilder.AddColumn<int>(
                name: "IdPedido",
                table: "PedidosAmizade",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdAmizade",
                table: "Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosAmizade",
                table: "PedidosAmizade",
                column: "IdPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade",
                column: "IdAmizade");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosFK",
                table: "PedidosAmizade",
                column: "DonoListaPedidosFK");

            migrationBuilder.CreateIndex(
                name: "IX_Amizade_DonoListaAmigosFK",
                table: "Amizade",
                column: "DonoListaAmigosFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosAmizade",
                table: "PedidosAmizade");

            migrationBuilder.DropIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosFK",
                table: "PedidosAmizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Amizade_DonoListaAmigosFK",
                table: "Amizade");

            migrationBuilder.DropColumn(
                name: "IdPedido",
                table: "PedidosAmizade");

            migrationBuilder.DropColumn(
                name: "IdAmizade",
                table: "Amizade");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosAmizade",
                table: "PedidosAmizade",
                columns: new[] { "DonoListaPedidosFK", "RemetenteFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade",
                columns: new[] { "DonoListaAmigosFK", "AmigoFK" });
        }
    }
}
