using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class table_name_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amigo_Utilizador_Registado_DonoListaFK",
                table: "Amigo");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_Salas_Chat_SalaFK",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_Utilizador_Registado_RemetenteFK",
                table: "Mensagem");

            migrationBuilder.DropTable(
                name: "Destinatario_Pedidos_Amizade");

            migrationBuilder.DropTable(
                name: "Registados_Salas_Chat");

            migrationBuilder.DropTable(
                name: "Registados_Salas_Jogo");

            migrationBuilder.DropTable(
                name: "Utilizador_Anonimo");

            migrationBuilder.DropTable(
                name: "Salas_Chat");

            migrationBuilder.DropTable(
                name: "Sala_Jogo_1_Contra_1");

            migrationBuilder.DropTable(
                name: "Utilizador_Registado");

            migrationBuilder.CreateTable(
                name: "SalaJogo1Contra1",
                columns: table => new
                {
                    IDSala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaJogo1Contra1", x => x.IDSala);
                });

            migrationBuilder.CreateTable(
                name: "SalasChat",
                columns: table => new
                {
                    IDSala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeSala = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SeGrupo = table.Column<bool>(type: "bit", nullable: false),
                    NomeFotografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalasChat", x => x.IDSala);
                });

            migrationBuilder.CreateTable(
                name: "UtilizadorAnonimo",
                columns: table => new
                {
                    IDUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorAnonimo", x => x.IDUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "UtilizadorRegistado",
                columns: table => new
                {
                    IDUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataJuncao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataNasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthenticationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeFotografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorRegistado", x => x.IDUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "DestinatarioPedidosAmizade",
                columns: table => new
                {
                    IDPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDestinatarioPedido = table.Column<int>(type: "int", nullable: false),
                    DataHoraPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinatarioPedidosAmizade", x => x.IDPedido);
                    table.ForeignKey(
                        name: "FK_DestinatarioPedidosAmizade_UtilizadorRegistado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistadosSalasChat",
                columns: table => new
                {
                    IDRegisto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistadosSalasChat", x => x.IDRegisto);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasChat_SalasChat_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "SalasChat",
                        principalColumn: "IDSala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasChat_UtilizadorRegistado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistadosSalasJogo",
                columns: table => new
                {
                    IDRegisto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFundador = table.Column<bool>(type: "bit", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistadosSalasJogo", x => x.IDRegisto);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasJogo_SalaJogo1Contra1_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "SalaJogo1Contra1",
                        principalColumn: "IDSala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistadosSalasJogo_UtilizadorRegistado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinatarioPedidosAmizade_RemetenteFK",
                table: "DestinatarioPedidosAmizade",
                column: "RemetenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasChat_SalaFK",
                table: "RegistadosSalasChat",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasChat_UtilizadorFK",
                table: "RegistadosSalasChat",
                column: "UtilizadorFK");

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasJogo_SalaFK",
                table: "RegistadosSalasJogo",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasJogo_UtilizadorFK",
                table: "RegistadosSalasJogo",
                column: "UtilizadorFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Amigo_UtilizadorRegistado_DonoListaFK",
                table: "Amigo",
                column: "DonoListaFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_SalasChat_SalaFK",
                table: "Mensagem",
                column: "SalaFK",
                principalTable: "SalasChat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_UtilizadorRegistado_RemetenteFK",
                table: "Mensagem",
                column: "RemetenteFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amigo_UtilizadorRegistado_DonoListaFK",
                table: "Amigo");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_SalasChat_SalaFK",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_UtilizadorRegistado_RemetenteFK",
                table: "Mensagem");

            migrationBuilder.DropTable(
                name: "DestinatarioPedidosAmizade");

            migrationBuilder.DropTable(
                name: "RegistadosSalasChat");

            migrationBuilder.DropTable(
                name: "RegistadosSalasJogo");

            migrationBuilder.DropTable(
                name: "UtilizadorAnonimo");

            migrationBuilder.DropTable(
                name: "SalasChat");

            migrationBuilder.DropTable(
                name: "SalaJogo1Contra1");

            migrationBuilder.DropTable(
                name: "UtilizadorRegistado");

            migrationBuilder.CreateTable(
                name: "Sala_Jogo_1_Contra_1",
                columns: table => new
                {
                    IDSala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala_Jogo_1_Contra_1", x => x.IDSala);
                });

            migrationBuilder.CreateTable(
                name: "Salas_Chat",
                columns: table => new
                {
                    IDSala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NomeFotografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeSala = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SeGrupo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas_Chat", x => x.IDSala);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador_Anonimo",
                columns: table => new
                {
                    IDUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador_Anonimo", x => x.IDUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador_Registado",
                columns: table => new
                {
                    IDUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthenticationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataJuncao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataNasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomeFotografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador_Registado", x => x.IDUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "Destinatario_Pedidos_Amizade",
                columns: table => new
                {
                    IDPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    DataHoraPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDDestinatarioPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinatario_Pedidos_Amizade", x => x.IDPedido);
                    table.ForeignKey(
                        name: "FK_Destinatario_Pedidos_Amizade_Utilizador_Registado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registados_Salas_Chat",
                columns: table => new
                {
                    IDRegisto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registados_Salas_Chat", x => x.IDRegisto);
                    table.ForeignKey(
                        name: "FK_Registados_Salas_Chat_Salas_Chat_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Salas_Chat",
                        principalColumn: "IDSala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registados_Salas_Jogo",
                columns: table => new
                {
                    IDRegisto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    IsFundador = table.Column<bool>(type: "bit", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registados_Salas_Jogo", x => x.IDRegisto);
                    table.ForeignKey(
                        name: "FK_Registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Sala_Jogo_1_Contra_1",
                        principalColumn: "IDSala",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinatario_Pedidos_Amizade_RemetenteFK",
                table: "Destinatario_Pedidos_Amizade",
                column: "RemetenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Chat_SalaFK",
                table: "Registados_Salas_Chat",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Chat_UtilizadorFK",
                table: "Registados_Salas_Chat",
                column: "UtilizadorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Jogo_SalaFK",
                table: "Registados_Salas_Jogo",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registados_Salas_Jogo_UtilizadorFK",
                table: "Registados_Salas_Jogo",
                column: "UtilizadorFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Amigo_Utilizador_Registado_DonoListaFK",
                table: "Amigo",
                column: "DonoListaFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_Salas_Chat_SalaFK",
                table: "Mensagem",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_Utilizador_Registado_RemetenteFK",
                table: "Mensagem",
                column: "RemetenteFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
