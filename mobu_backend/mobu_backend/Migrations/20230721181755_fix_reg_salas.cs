using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_reg_salas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat");

            migrationBuilder.AddColumn<int>(
                name: "IDRegisto",
                table: "Registados_Salas_Jogo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IDRegisto",
                table: "Registados_Salas_Chat",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo",
                column: "IDRegisto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat",
                column: "IDRegisto");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Jogo_UtilizadorFK",
                table: "Registados_Salas_Jogo",
                column: "UtilizadorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Chat_UtilizadorFK",
                table: "Registados_Salas_Chat",
                column: "UtilizadorFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropIndex(
                name: "IX_Registados_Salas_Jogo_UtilizadorFK",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat");

            migrationBuilder.DropIndex(
                name: "IX_Registados_Salas_Chat_UtilizadorFK",
                table: "Registados_Salas_Chat");

            migrationBuilder.DropColumn(
                name: "IDRegisto",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropColumn(
                name: "IDRegisto",
                table: "Registados_Salas_Chat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat",
                columns: new[] { "UtilizadorFK", "SalaFK" });
        }
    }
}
