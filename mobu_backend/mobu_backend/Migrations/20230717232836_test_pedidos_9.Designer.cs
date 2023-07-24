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
    [Migration("20230717232836_test_pedidos_9")]
    partial class test_pedidos_9
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
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
                    b.Property<int>("IDMensagem")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<string>("ConteudoMsg")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("EstadoMensagem")
                        .HasColumnType("int");

                    b.Property<int>("RemetenteFK")
                        .HasColumnType("int");

                    b.HasKey("IDMensagem", "SalaFK");

                    b.HasIndex("RemetenteFK");

                    b.HasIndex("SalaFK");

                    b.ToTable("Mensagem");
                });

            modelBuilder.Entity("mobu_backend.Models.Pedidos_Amizade", b =>
                {
                    b.Property<int>("RemetenteFK")
                        .HasColumnType("int");

                    b.Property<int>("DestinatarioFK")
                        .HasColumnType("int");

                    b.Property<int>("EstadoPedido")
                        .HasColumnType("int");

                    b.HasKey("RemetenteFK", "DestinatarioFK");

                    b.HasIndex("DestinatarioFK");

                    b.ToTable("Pedidos_Amizade");
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

                    b.ToTable("Registados_Salas_Chat");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Jogo", b =>
                {
                    b.Property<int>("UtilizadorFK")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<bool>("IsFundador")
                        .HasColumnType("bit");

                    b.HasKey("UtilizadorFK", "SalaFK");

                    b.HasIndex("SalaFK");

                    b.ToTable("Registados_Salas_Jogo");
                });

            modelBuilder.Entity("mobu_backend.Models.Sala_Jogo_1_Contra_1", b =>
                {
                    b.Property<int>("IDSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDSala"));

                    b.HasKey("IDSala");

                    b.ToTable("Sala_Jogo_1_Contra_1");
                });

            modelBuilder.Entity("mobu_backend.Models.Salas_Chat", b =>
                {
                    b.Property<int>("IDSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDSala"));

                    b.Property<string>("NomeSala")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("SeGrupo")
                        .HasColumnType("bit");

                    b.HasKey("IDSala");

                    b.ToTable("Salas_Chat");
                });

            modelBuilder.Entity("mobu_backend.Models.Utilizador_Anonimo", b =>
                {
                    b.Property<int>("IDUtilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDUtilizador"));

                    b.Property<string>("EnderecoIPv4")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("EnderecoIPv6")
                        .HasMaxLength(39)
                        .HasColumnType("nvarchar(39)");

                    b.Property<string>("Fotografia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeUtilizador")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("IDUtilizador");

                    b.ToTable("Utilizador_Anonimo");

                    b.HasData(
                        new
                        {
                            IDUtilizador = 3,
                            EnderecoIPv4 = "192.168.1.1",
                            EnderecoIPv6 = "",
                            NomeUtilizador = "guest3"
                        },
                        new
                        {
                            IDUtilizador = 4,
                            EnderecoIPv4 = "192.168.1.2",
                            EnderecoIPv6 = "",
                            NomeUtilizador = "guest4"
                        },
                        new
                        {
                            IDUtilizador = 5,
                            EnderecoIPv4 = "",
                            EnderecoIPv6 = "2001:818:dfba:c100:1464:bee0:19fb:f940",
                            NomeUtilizador = "guest5"
                        });
                });

            modelBuilder.Entity("mobu_backend.Models.Utilizador_Registado", b =>
                {
                    b.Property<int>("IDUtilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDUtilizador"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Fotografia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeUtilizador")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(96)
                        .HasColumnType("nvarchar(96)");

                    b.HasKey("IDUtilizador");

                    b.ToTable("Utilizador_Registado");

                    b.HasData(
                        new
                        {
                            IDUtilizador = 1,
                            Email = "teste1@teste.com",
                            NomeUtilizador = "teste1",
                            Password = "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A"
                        },
                        new
                        {
                            IDUtilizador = 2,
                            Email = "teste2@teste.com",
                            NomeUtilizador = "teste2",
                            Password = "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A"
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
                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "Remetente")
                        .WithMany("ListaMensagensEnviadas")
                        .HasForeignKey("RemetenteFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Salas_Chat", "Sala")
                        .WithMany("ListaMensagensRecebidas")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Remetente");

                    b.Navigation("Sala");
                });

            modelBuilder.Entity("mobu_backend.Models.Pedidos_Amizade", b =>
                {
                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "DestinatarioPedido")
                        .WithMany("ListaPedidosRecebidos")
                        .HasForeignKey("DestinatarioFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "RemetentePedido")
                        .WithMany("ListaPedidosEnviados")
                        .HasForeignKey("RemetenteFK")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinatarioPedido");

                    b.Navigation("RemetentePedido");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Chat", b =>
                {
                    b.HasOne("mobu_backend.Models.Salas_Chat", "Sala")
                        .WithMany("ListaRegistadosSalasChat")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "Utilizador")
                        .WithMany("ListaSalasDeChat")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sala");

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("mobu_backend.Models.Registados_Salas_Jogo", b =>
                {
                    b.HasOne("mobu_backend.Models.Sala_Jogo_1_Contra_1", "Sala")
                        .WithMany("ListaRegistados")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.Utilizador_Registado", "Utilizador")
                        .WithMany("ListaSalasJogo")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sala");

                    b.Navigation("Utilizador");
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
