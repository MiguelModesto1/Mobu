using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinatario_Pedidos_Amizade",
                table: "Destinatario_Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amigo",
                table: "Amigo");

            migrationBuilder.AddColumn<int>(
                name: "IDPedido",
                table: "Destinatario_Pedidos_Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IDAmizade",
                table: "Amigo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinatario_Pedidos_Amizade",
                table: "Destinatario_Pedidos_Amizade",
                column: "IDPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amigo",
                table: "Amigo",
                column: "IDAmizade");

            migrationBuilder.CreateIndex(
                name: "IX_Destinatario_Pedidos_Amizade_RemetenteFK",
                table: "Destinatario_Pedidos_Amizade",
                column: "RemetenteFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinatario_Pedidos_Amizade",
                table: "Destinatario_Pedidos_Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Destinatario_Pedidos_Amizade_RemetenteFK",
                table: "Destinatario_Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amigo",
                table: "Amigo");

            migrationBuilder.DropColumn(
                name: "IDPedido",
                table: "Destinatario_Pedidos_Amizade");

            migrationBuilder.DropColumn(
                name: "IDAmizade",
                table: "Amigo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinatario_Pedidos_Amizade",
                table: "Destinatario_Pedidos_Amizade",
                columns: new[] { "RemetenteFK", "IDDestinatarioPedido" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amigo",
                table: "Amigo",
                columns: new[] { "IDAmigo", "DonoListaFK" });
        }
    }
}
