using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_lengths_and_db_struct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sala_Jogo_1_Contra_1",
                columns: table => new
                {
                    ID_Sala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala_Jogo_1_Contra_1", x => x.ID_Sala);
                });

            migrationBuilder.CreateTable(
                name: "Salas_Chat",
                columns: table => new
                {
                    ID_Sala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_sala = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Se_grupo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas_Chat", x => x.ID_Sala);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador_Anonimo",
                columns: table => new
                {
                    ID_Utilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EnderecoIP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador_Anonimo", x => x.ID_Utilizador);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador_Registado",
                columns: table => new
                {
                    ID_Utilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    passwHash = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador_Registado", x => x.ID_Utilizador);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mensagem",
                columns: table => new
                {
                    ID_Mensagem = table.Column<int>(type: "int", nullable: false),
                    salaFK = table.Column<int>(type: "int", nullable: false),
                    Conteudo_Msg = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Estado_Mensagem = table.Column<int>(type: "int", nullable: false),
                    remetenteFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mensagem", x => new { x.ID_Mensagem, x.salaFK });
                    table.ForeignKey(
                        name: "FK_mensagem_Salas_Chat_salaFK",
                        column: x => x.salaFK,
                        principalTable: "Salas_Chat",
                        principalColumn: "ID_Sala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mensagem_Utilizador_Registado_remetenteFK",
                        column: x => x.remetenteFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "ID_Utilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos_Amizade",
                columns: table => new
                {
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    DestinatarioFK = table.Column<int>(type: "int", nullable: false),
                    Estado_pedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos_Amizade", x => new { x.RemetenteFK, x.DestinatarioFK });
                    table.ForeignKey(
                        name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                        column: x => x.DestinatarioFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "ID_Utilizador");
                    table.ForeignKey(
                        name: "FK_Pedidos_Amizade_Utilizador_Registado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "ID_Utilizador");
                });

            migrationBuilder.CreateTable(
                name: "registados_Salas_Chat",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registados_Salas_Chat", x => new { x.UtilizadorFK, x.SalaFK });
                    table.ForeignKey(
                        name: "FK_registados_Salas_Chat_Salas_Chat_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Salas_Chat",
                        principalColumn: "ID_Sala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "ID_Utilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registados_Salas_Jogo",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    Is_fundador = table.Column<bool>(type: "bit", nullable: false),
                    Sala_Jogo_1_Contra_1ID_Sala = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registados_Salas_Jogo", x => new { x.UtilizadorFK, x.SalaFK });
                    table.ForeignKey(
                        name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1ID_Sala",
                        column: x => x.Sala_Jogo_1_Contra_1ID_Sala,
                        principalTable: "Sala_Jogo_1_Contra_1",
                        principalColumn: "ID_Sala");
                    table.ForeignKey(
                        name: "FK_registados_Salas_Jogo_Salas_Chat_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Salas_Chat",
                        principalColumn: "ID_Sala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "ID_Utilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_mensagem_remetenteFK",
                table: "mensagem",
                column: "remetenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_mensagem_salaFK",
                table: "mensagem",
                column: "salaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK");

            migrationBuilder.CreateIndex(
                name: "IX_registados_Salas_Chat_SalaFK",
                table: "registados_Salas_Chat",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1ID_Sala",
                table: "registados_Salas_Jogo",
                column: "Sala_Jogo_1_Contra_1ID_Sala");

            migrationBuilder.CreateIndex(
                name: "IX_registados_Salas_Jogo_SalaFK",
                table: "registados_Salas_Jogo",
                column: "SalaFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "mensagem");

            migrationBuilder.DropTable(
                name: "Pedidos_Amizade");

            migrationBuilder.DropTable(
                name: "registados_Salas_Chat");

            migrationBuilder.DropTable(
                name: "registados_Salas_Jogo");

            migrationBuilder.DropTable(
                name: "Utilizador_Anonimo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Sala_Jogo_1_Contra_1");

            migrationBuilder.DropTable(
                name: "Salas_Chat");

            migrationBuilder.DropTable(
                name: "Utilizador_Registado");
        }
    }
}
