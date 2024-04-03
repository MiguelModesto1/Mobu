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
    [Migration("20240402131439_add_block_2")]
    partial class add_block_2
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

            modelBuilder.Entity("mobu_backend.Models.Admin", b =>
                {
                    b.Property<int>("IDAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDAdmin"));

                    b.Property<string>("AuthenticationID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataFotografia")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataJuncao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataNasc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomeAdmin")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("NomeFotografia")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IDAdmin");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("mobu_backend.Models.Amigo", b =>
                {
                    b.Property<int>("IDAmizade")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDAmizade"));

                    b.Property<bool>("Bloqueado")
                        .HasColumnType("bit");

                    b.Property<int>("DonoListaFK")
                        .HasColumnType("int");

                    b.Property<int>("IDAmigo")
                        .HasColumnType("int");

                    b.HasKey("IDAmizade");

                    b.HasIndex("DonoListaFK");

                    b.ToTable("Amigo");
                });

            modelBuilder.Entity("mobu_backend.Models.DestinatarioPedidosAmizade", b =>
                {
                    b.Property<int>("IDPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDPedido"));

                    b.Property<DateTime>("DataHoraPedido")
                        .HasColumnType("datetime2");

                    b.Property<int>("IDDestinatarioPedido")
                        .HasColumnType("int");

                    b.Property<int>("RemetenteFK")
                        .HasColumnType("int");

                    b.HasKey("IDPedido");

                    b.HasIndex("RemetenteFK");

                    b.ToTable("DestinatarioPedidosAmizade");
                });

            modelBuilder.Entity("mobu_backend.Models.Mensagem", b =>
                {
                    b.Property<int>("IDMensagemSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDMensagemSala"));

                    b.Property<string>("ConteudoMsg")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("DataHoraMsg")
                        .HasColumnType("datetime2");

                    b.Property<int>("IDMensagem")
                        .HasColumnType("int");

                    b.Property<int>("RemetenteFK")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.HasKey("IDMensagemSala");

                    b.HasIndex("RemetenteFK");

                    b.HasIndex("SalaFK");

                    b.ToTable("Mensagem");
                });

            modelBuilder.Entity("mobu_backend.Models.RegistadosSalasChat", b =>
                {
                    b.Property<int>("IDRegisto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDRegisto"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<int>("UtilizadorFK")
                        .HasColumnType("int");

                    b.HasKey("IDRegisto");

                    b.HasIndex("SalaFK");

                    b.HasIndex("UtilizadorFK");

                    b.ToTable("RegistadosSalasChat");
                });

            modelBuilder.Entity("mobu_backend.Models.RegistadosSalasJogo", b =>
                {
                    b.Property<int>("IDRegisto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDRegisto"));

                    b.Property<bool>("IsFundador")
                        .HasColumnType("bit");

                    b.Property<int>("Pontos")
                        .HasColumnType("int");

                    b.Property<int>("SalaFK")
                        .HasColumnType("int");

                    b.Property<int>("UtilizadorFK")
                        .HasColumnType("int");

                    b.HasKey("IDRegisto");

                    b.HasIndex("SalaFK");

                    b.HasIndex("UtilizadorFK");

                    b.ToTable("RegistadosSalasJogo");
                });

            modelBuilder.Entity("mobu_backend.Models.SalaJogo1Contra1", b =>
                {
                    b.Property<int>("IDSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDSala"));

                    b.HasKey("IDSala");

                    b.ToTable("SalaJogo1Contra1");
                });

            modelBuilder.Entity("mobu_backend.Models.SalasChat", b =>
                {
                    b.Property<int>("IDSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDSala"));

                    b.Property<DateTime?>("DataFotografia")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomeFotografia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeSala")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("SeGrupo")
                        .HasColumnType("bit");

                    b.HasKey("IDSala");

                    b.ToTable("SalasChat");
                });

            modelBuilder.Entity("mobu_backend.Models.UtilizadorAnonimo", b =>
                {
                    b.Property<int>("IDUtilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDUtilizador"));

                    b.HasKey("IDUtilizador");

                    b.ToTable("UtilizadorAnonimo");
                });

            modelBuilder.Entity("mobu_backend.Models.UtilizadorRegistado", b =>
                {
                    b.Property<int>("IDUtilizador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IDUtilizador"));

                    b.Property<string>("AuthenticationID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataFotografia")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataJuncao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataNasc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomeFotografia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeUtilizador")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("IDUtilizador");

                    b.ToTable("UtilizadorRegistado");
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

            modelBuilder.Entity("mobu_backend.Models.Amigo", b =>
                {
                    b.HasOne("mobu_backend.Models.UtilizadorRegistado", "DonoListaAmigos")
                        .WithMany("ListaAmigos")
                        .HasForeignKey("DonoListaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonoListaAmigos");
                });

            modelBuilder.Entity("mobu_backend.Models.DestinatarioPedidosAmizade", b =>
                {
                    b.HasOne("mobu_backend.Models.UtilizadorRegistado", "RemetentePedido")
                        .WithMany("ListaDetinatarios")
                        .HasForeignKey("RemetenteFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RemetentePedido");
                });

            modelBuilder.Entity("mobu_backend.Models.Mensagem", b =>
                {
                    b.HasOne("mobu_backend.Models.UtilizadorRegistado", "Remetente")
                        .WithMany("ListaMensagensEnviadas")
                        .HasForeignKey("RemetenteFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.SalasChat", "Sala")
                        .WithMany("ListaMensagensRecebidas")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Remetente");

                    b.Navigation("Sala");
                });

            modelBuilder.Entity("mobu_backend.Models.RegistadosSalasChat", b =>
                {
                    b.HasOne("mobu_backend.Models.SalasChat", "Sala")
                        .WithMany("ListaRegistadosSalasChat")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.UtilizadorRegistado", "Utilizador")
                        .WithMany("ListaSalasDeChat")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sala");

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("mobu_backend.Models.RegistadosSalasJogo", b =>
                {
                    b.HasOne("mobu_backend.Models.SalaJogo1Contra1", "Sala")
                        .WithMany("ListaRegistados")
                        .HasForeignKey("SalaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mobu_backend.Models.UtilizadorRegistado", "Utilizador")
                        .WithMany("ListaSalasJogo")
                        .HasForeignKey("UtilizadorFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sala");

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("mobu_backend.Models.SalaJogo1Contra1", b =>
                {
                    b.Navigation("ListaRegistados");
                });

            modelBuilder.Entity("mobu_backend.Models.SalasChat", b =>
                {
                    b.Navigation("ListaMensagensRecebidas");

                    b.Navigation("ListaRegistadosSalasChat");
                });

            modelBuilder.Entity("mobu_backend.Models.UtilizadorRegistado", b =>
                {
                    b.Navigation("ListaAmigos");

                    b.Navigation("ListaDetinatarios");

                    b.Navigation("ListaMensagensEnviadas");

                    b.Navigation("ListaSalasDeChat");

                    b.Navigation("ListaSalasJogo");
                });
#pragma warning restore 612, 618
        }
    }
}
