using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class change_chat_rooms_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFotografia",
                table: "Salas_Chat",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFotografia",
                table: "Salas_Chat",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFotografia",
                table: "Salas_Chat");

            migrationBuilder.DropColumn(
                name: "NomeFotografia",
                table: "Salas_Chat");
        }
    }
}
