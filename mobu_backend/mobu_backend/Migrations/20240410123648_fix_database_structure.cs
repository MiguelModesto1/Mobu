using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_database_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "IDMensagemSala",
                table: "Mensagem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                column: "IDMensagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.AlterColumn<int>(
                name: "IDMensagem",
                table: "Mensagem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IDMensagemSala",
                table: "Mensagem",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                column: "IDMensagemSala");

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    IDAdmin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthenticationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataJuncao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataNasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomeAdmin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NomeFotografia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.IDAdmin);
                });
        }
    }
}
