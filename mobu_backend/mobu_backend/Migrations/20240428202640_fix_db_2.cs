using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropIndex(
                name: "IX_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropColumn(
                name: "DonoListaAmigosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropColumn(
                name: "DonoListaPedidosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.CreateTable(
                name: "Amizade",
                columns: table => new
                {
                    DonoListaAmigosFK = table.Column<int>(type: "int", nullable: false),
                    AmigoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amizade", x => new { x.DonoListaAmigosFK, x.AmigoFK });
                    table.ForeignKey(
                        name: "FK_Amizade_UtilizadorRegistado_AmigoFK",
                        column: x => x.AmigoFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amizade_UtilizadorRegistado_DonoListaAmigosFK",
                        column: x => x.DonoListaAmigosFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosAmizade",
                columns: table => new
                {
                    DonoListaPedidosFK = table.Column<int>(type: "int", nullable: false),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    DonoListaPedidosIDUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosAmizade", x => new { x.DonoListaPedidosFK, x.RemetenteFK });
                    table.ForeignKey(
                        name: "FK_PedidosAmizade_UtilizadorRegistado_DonoListaPedidosFK",
                        column: x => x.DonoListaPedidosFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosAmizade_UtilizadorRegistado_DonoListaPedidosIDUtilizador",
                        column: x => x.DonoListaPedidosIDUtilizador,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amizade_AmigoFK",
                table: "Amizade",
                column: "AmigoFK");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosIDUtilizador",
                table: "PedidosAmizade",
                column: "DonoListaPedidosIDUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amizade");

            migrationBuilder.DropTable(
                name: "PedidosAmizade");

            migrationBuilder.AddColumn<int>(
                name: "DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaAmigosFK");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaPedidosFK");

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaAmigosFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaPedidosFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
