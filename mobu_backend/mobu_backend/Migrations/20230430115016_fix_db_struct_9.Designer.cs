﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mobu_backend.Data;

#nullable disable

namespace mobu_backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230430115016_fix_db_struct_9")]
    partial class fix_db_struct_9
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("mobu_backend.Models.Mensagem", b =>
                {
                    b.Property<int>("ID_Mensagem")
                        .HasColumnType("int");

                    b.Property<int>("salaFK")
                        .HasColumnType("int");

                    b.Property<string>("Conteudo_Msg")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Estado_Mensagem")
                        .HasColumnType("int");

                    b.Property<int>("remetenteFK")
                        .HasColumnType("int");

                    b.HasKey("ID_Mensagem", "salaFK");

                    b.HasIndex("remetenteFK");

                    b.HasIndex("salaFK");

                    b.ToTable("mensagem");
                });

            modelBuilder.Entity("mobu_backend.Models.Pedidos_Amizade", b =>
                {
                    b.Property<int>("RemetenteFK")
                        .HasColumnType("int");

                    b.Property<int>("DestinatarioFK")
                        .HasColumnType("int");

                    b.Property<int>("Estado_pedido")
                        .HasColumnType("int");

                    b.HasKey("RemetenteFK", "DestinatarioFK");

                    b.HasIndex("DestinatarioFK");

                    b.ToTable("Pedidos_Amizade");

                    b.HasData(
                        new
                        {
                            RemetenteFK = 1,
                            DestinatarioFK = 2,
                            Estado_pedido = 1
                        });
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Chat", b =>
                {
                    b.Property<int>("UtilizadorFK")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.HasKey("UtilizadorFK", "SalaFK");

                    b.HasIndex("SalaFK");

                    b.ToTable("registados_Salas_Chat");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Jogo", b =>
                {
                    b.Property<int>("UtilizadorFK")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<bool>("Is_fundador")
                        .HasColumnType("bit");

                    b.Property<int?>("Sala_Jogo_1_Contra_1ID_Sala")
                        .HasColumnType("int");

                    b.HasKey("UtilizadorFK", "SalaFK");

                    b.HasIndex("SalaFK");

                    b.HasIndex("Sala_Jogo_1_Contra_1ID_Sala");

                    b.ToTable("registados_Salas_Jogo");
                });

            modelBuilder.Entity("mobu_backend.Models.Sala_Jogo_1_Contra_1", b =>
                {
                    b.Property<int>("ID_Sala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Sala"));

                    b.HasKey("ID_Sala");

                    b.ToTable("Sala_Jogo_1_Contra_1");
                });

            modelBuilder.Entity("mobu_backend.Models.Salas_Chat", b =>
                {
                    b.Property<int>("ID_Sala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Sala"));

                    b.Property<string>("Nome_sala")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("Se_grupo")
                        .HasColumnType("bit");

                    b.HasKey("ID_Sala");

                    b.ToTable("Salas_Chat");
                });

            modelBuilder.Entity("mobu_backend.Models.Utilizador_Anonimo", b =>
                {
                    b.Property<int>("ID_Utilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Utilizador"));

                    b.Property<string>("EnderecoIP")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("NomeUtilizador")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID_Utilizador");

                    b.ToTable("Utilizador_Anonimo");

                    b.HasData(
                        new
                        {
                            ID_Utilizador = 3,
                            EnderecoIP = "192.168.1.1",
                            NomeUtilizador = "guest3"
                        },
                        new
                        {
                            ID_Utilizador = 4,
                            EnderecoIP = "192.168.1.2",
                            NomeUtilizador = "guest4"
                        });
                });

            modelBuilder.Entity("mobu_backend.Models.Utilizador_Registado", b =>
                {
                    b.Property<int>("ID_Utilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Utilizador"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomeUtilizador")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("passwHash")
                        .IsRequired()
                        .HasMaxLength(96)
                        .HasColumnType("nvarchar(96)");

                    b.HasKey("ID_Utilizador");

                    b.ToTable("Utilizador_Registado");

                    b.HasData(
                        new
                        {
                            ID_Utilizador = 1,
                            Email = "teste1@teste.com",
                            NomeUtilizador = "teste1",
                            passwHash = "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A"
                        },
                        new
                        {
                            ID_Utilizador = 2,
                            Email = "teste2@teste.com",
                            NomeUtilizador = "teste2",
                            passwHash = "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("mobu_backend.Models.Mensagem", b =>
                {
                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "REMETENTE")
                        .WithMany("ListaMensagensEnviadas")
                        .HasForeignKey("remetenteFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Salas_Chat", "SALA")
                        .WithMany("ListaMensagensRecebidas")
                        .HasForeignKey("salaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("REMETENTE");

                    b.Navigation("SALA");
                });

            modelBuilder.Entity("mobu_backend.Models.Pedidos_Amizade", b =>
                {
                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "DESTINATARIO_PEDIDO")
                        .WithMany("ListaPedidosEnviados")
                        .HasForeignKey("DestinatarioFK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "REMETENTE_PEDIDO")
                        .WithMany("ListaPedidosRecebidos")
                        .HasForeignKey("RemetenteFK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DESTINATARIO_PEDIDO");

                    b.Navigation("REMETENTE_PEDIDO");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Chat", b =>
                {
                    b.HasOne("mobu_backend.Models.Salas_Chat", "SALA")
                        .WithMany("ListaRegistadosSalasChat")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "UTILZADOR")
                        .WithMany("ListaSalasDeChat")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SALA");

                    b.Navigation("UTILZADOR");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Jogo", b =>
                {
                    b.HasOne("mobu_backend.Models.Salas_Chat", "SALA")
                        .WithMany()
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Sala_Jogo_1_Contra_1", null)
                        .WithMany("ListaRegistados")
                        .HasForeignKey("Sala_Jogo_1_Contra_1ID_Sala");

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "UTILZADOR")
                        .WithMany("ListaSalasJogo")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SALA");

                    b.Navigation("UTILZADOR");
                });

            modelBuilder.Entity("mobu_backend.Models.Sala_Jogo_1_Contra_1", b =>
                {
                    b.Navigation("ListaRegistados");
                });

            modelBuilder.Entity("mobu_backend.Models.Salas_Chat", b =>
                {
                    b.Navigation("ListaMensagensRecebidas");

                    b.Navigation("ListaRegistadosSalasChat");
                });

            modelBuilder.Entity("mobu_backend.Models.Utilizador_Registado", b =>
                {
                    b.Navigation("ListaMensagensEnviadas");

                    b.Navigation("ListaPedidosEnviados");

                    b.Navigation("ListaPedidosRecebidos");

                    b.Navigation("ListaSalasDeChat");

                    b.Navigation("ListaSalasJogo");
                });
#pragma warning restore 612, 618
        }
    }
}