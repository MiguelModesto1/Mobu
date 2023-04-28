using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Models;

namespace mobu_backend.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		/// <summary>
		/// este método é executado imediatamente antes 
		/// da criação do Modelo.
		/// É utilizado para adicionar as últimas instruções
		/// à criação do modelo
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
			base.OnModelCreating(modelBuilder);


			//Tratamento do auto-relacionamento
			//de 'Utilizador_Registado' em relacao
			//aos pedidos de amizade
			modelBuilder.Entity<Pedidos_Amizade>()
				.HasOne(e => e.REMETENTE_PEDIDO)
				.WithMany(e => e.ListaPedidosRecebidos)
				.HasForeignKey(e => e.RemetenteFK)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Pedidos_Amizade>()
				.HasOne(e => e.DESTINATARIO_PEDIDO)
				.WithMany(e => e.ListaPedidosEnviados)
				.HasForeignKey(e => e.DestinatarioFK)
				.OnDelete(DeleteBehavior.NoAction);

			//Adicao de dados de teste

			string passStr = "123qwe#";
			var utf8 = new UTF8Encoding();
			byte[] pass =utf8.GetBytes(passStr);
			Console.WriteLine(utf8.GetString(pass));
			byte[] result;
			SHA384 shaManager = SHA384.Create();
			result = shaManager.ComputeHash(pass);
			Console.WriteLine(utf8.GetString(result));
			Console.WriteLine(shaManager.GetHashCode().ToString());

			modelBuilder.Entity<Utilizador_Registado>()
				.HasData(
					new Utilizador_Registado
					{
						ID_Utilizador = 1,
						NomeUtilizador = "teste1",
						Email = "teste1@teste.com",
						passwHash = utf8.GetString(result)
					},
					new Utilizador_Registado
					{
						ID_Utilizador = 2,
						NomeUtilizador = "teste2",
						Email = "teste2@teste.com",
						passwHash = utf8.GetString(result)
					}
				);

			modelBuilder.Entity<Utilizador_Anonimo>()
				.HasData(
					new Utilizador_Anonimo
					{
						ID_Utilizador = 3,
						NomeUtilizador = "teste3",
						EnderecoIP = "192.168.1.1"
					},
					new Utilizador_Anonimo
					{
						ID_Utilizador = 4,
						NomeUtilizador = "teste4",
						EnderecoIP = "192.168.1.2"
					}
				);

		}

		//definir tabelas da BD

		public DbSet<Utilizador_Registado> Utilizador_Registado { get; set; }
		public DbSet<Utilizador_Anonimo> Utilizador_Anonimo { get; set; }
		public DbSet<Salas_Chat> Salas_Chat { get; set; }
		public DbSet<Sala_Jogo_1_Contra_1> Sala_Jogo_1_Contra_1 { get; set; }
		public DbSet<Registados_Salas_Jogo> registados_Salas_Jogo { get; set; }
		public DbSet<Pedidos_Amizade> Pedidos_Amizade { get; set; }
		public DbSet<Registados_Salas_Chat> registados_Salas_Chat { get; set; }
		public DbSet<Mensagem> mensagem { get; set; }
	}
}
