using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIPv6",
                table: "Utilizador_Anonimo",
                type: "nvarchar(39)",
                maxLength: 39,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(39)",
                oldMaxLength: 39);

            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIPv6",
                table: "Utilizador_Anonimo",
                type: "nvarchar(39)",
                maxLength: 39,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(39)",
                oldMaxLength: 39,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);
        }
    }
}
