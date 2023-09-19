using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class remove_request_states : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoMensagem",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "EstadoPedido",
                table: "Destinatario_Pedidos_Amizade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoMensagem",
                table: "Mensagem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoPedido",
                table: "Destinatario_Pedidos_Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
