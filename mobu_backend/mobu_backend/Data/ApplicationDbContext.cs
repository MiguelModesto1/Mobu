using System.Buffers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Models;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol;

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

			//Adicao de dados de teste

			/*string passStr = "123qwe#";
			var utf8 = new UTF8Encoding(false, true);
			byte[] pass = utf8.GetBytes(passStr);
			Console.WriteLine(utf8.GetString(pass));
			byte[] result;
			SHA384 shaManager = SHA384.Create();
			result = shaManager.ComputeHash(pass);
            Console.WriteLine(Convert.ToHexString(result));

			modelBuilder.Entity<Utilizador_Registado>()
				.HasData(
					new Utilizador_Registado
					{
						IDUtilizador = 1,
						NomeUtilizador = "teste1",
						Email = "teste1@teste.com",
						Password = Convert.ToHexString(result)
					},
					new Utilizador_Registado
					{
						IDUtilizador = 2,
						NomeUtilizador = "teste2",
						Email = "teste2@teste.com",
						Password = Convert.ToHexString(result)
					}
				); ;

			modelBuilder.Entity<Utilizador_Anonimo>()
				.HasData(
					new Utilizador_Anonimo
					{
						IDUtilizador = 3,
						NomeUtilizador = "guest3",
						EnderecoIPv4 = "192.168.1.1",
						EnderecoIPv6 = ""
					},
					new Utilizador_Anonimo
					{
						IDUtilizador = 4,
						NomeUtilizador = "guest4",
						EnderecoIPv4 = "192.168.1.2",
						EnderecoIPv6 = ""
					},
                    new Utilizador_Anonimo
                    {
                        IDUtilizador = 5,
                        NomeUtilizador = "guest5",
						EnderecoIPv4 = "",
                        EnderecoIPv6 = "2001:818:dfba:c100:1464:bee0:19fb:f940"
                    }
                );*/

			/*modelBuilder.Entity<Destinatario_Pedidos_Amizade>()
				.HasData(
					new Destinatario_Pedidos_Amizade ()
					{
						IDDestinatarioPedido = 2,
						RemetenteFK = 1,
						EstadoPedido = (Destinatario_Pedidos_Amizade.EstadosPedido)1
					}
				);*/

		}

		//definir tabelas da BD

		public DbSet<Utilizador_Registado> Utilizador_Registado { get; set; }
		public DbSet<Utilizador_Anonimo> Utilizador_Anonimo { get; set; }
		public DbSet<Salas_Chat> Salas_Chat { get; set; }
		public DbSet<Sala_Jogo_1_Contra_1> Sala_Jogo_1_Contra_1 { get; set; }
		public DbSet<Registados_Salas_Jogo> Registados_Salas_Jogo { get; set; }
		public DbSet<Destinatario_Pedidos_Amizade> Destinatario_Pedidos_Amizade { get; set; }
		public DbSet<Registados_Salas_Chat> Registados_Salas_Chat { get; set; }
		public DbSet<Mensagem> Mensagem { get; set; }
		public DbSet<Amigo> Amigo { get; set; }
		public DbSet<Admin> Admin { get; set; }
	}
}
