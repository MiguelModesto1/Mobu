using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos_Amizade");

            migrationBuilder.CreateTable(
                name: "Amigo",
                columns: table => new
                {
                    IDAmigo = table.Column<int>(type: "int", nullable: false),
                    DonoListaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amigo", x => new { x.IDAmigo, x.DonoListaFK });
                    table.ForeignKey(
                        name: "FK_Amigo_Utilizador_Registado_DonoListaFK",
                        column: x => x.DonoListaFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destinatario_Pedidos_Amizade",
                columns: table => new
                {
                    IDDestinatarioPedido = table.Column<int>(type: "int", nullable: false),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    EstadoPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinatario_Pedidos_Amizade", x => new { x.RemetenteFK, x.IDDestinatarioPedido });
                    table.ForeignKey(
                        name: "FK_Destinatario_Pedidos_Amizade_Utilizador_Registado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amigo_DonoListaFK",
                table: "Amigo",
                column: "DonoListaFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amigo");

            migrationBuilder.DropTable(
                name: "Destinatario_Pedidos_Amizade");

            migrationBuilder.CreateTable(
                name: "Pedidos_Amizade",
                columns: table => new
                {
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    DestinatarioFK = table.Column<int>(type: "int", nullable: false),
                    EstadoPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos_Amizade", x => new { x.RemetenteFK, x.DestinatarioFK });
                    table.ForeignKey(
                        name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                        column: x => x.DestinatarioFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Amizade_Utilizador_Registado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Pedidos_Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK", "EstadoPedido" },
                values: new object[] { 2, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK");
        }
    }
}
