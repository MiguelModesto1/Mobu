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

			//Adicao de dados de teste

			/*string passStr = "123qwe#";
			var utf8 = new UTF8Encoding(false, true);
			byte[] pass = utf8.GetBytes(passStr);
			Console.WriteLine(utf8.GetString(pass));
			byte[] result;
			SHA384 shaManager = SHA384.Create();
			result = shaManager.ComputeHash(pass);
			Console.WriteLine(Convert.ToHexString(result));

			modelBuilder.Entity<UtilizadorRegistado>()
				.HasData(
					new UtilizadorRegistado
					{
						IDUtilizador = 1,
						NomeUtilizador = "teste1",
						Email = "teste1@teste.com",
						Password = Convert.ToHexString(result)
					},
					new UtilizadorRegistado
					{
						IDUtilizador = 2,
						NomeUtilizador = "teste2",
						Email = "teste2@teste.com",
						Password = Convert.ToHexString(result)
					}
				); ;

			modelBuilder.Entity<UtilizadorAnonimo>()
				.HasData(
					new UtilizadorAnonimo
					{
						IDUtilizador = 3,
						NomeUtilizador = "guest3",
						EnderecoIPv4 = "192.168.1.1",
						EnderecoIPv6 = ""
					},
					new UtilizadorAnonimo
					{
						IDUtilizador = 4,
						NomeUtilizador = "guest4",
						EnderecoIPv4 = "192.168.1.2",
						EnderecoIPv6 = ""
					},
					new UtilizadorAnonimo
					{
						IDUtilizador = 5,
						NomeUtilizador = "guest5",
						EnderecoIPv4 = "",
						EnderecoIPv6 = "2001:818:dfba:c100:1464:bee0:19fb:f940"
					}
				);*/

			/*modelBuilder.Entity<DestinatarioPedidosAmizade>()
				.HasData(
					new DestinatarioPedidosAmizade ()
					{
						IDDestinatarioPedido = 2,
						RemetenteFK = 1,
						EstadoPedido = (DestinatarioPedidosAmizade.EstadosPedido)1
					}
				);*/
		}

		

		//definir tabelas da BD

		public DbSet<UtilizadorRegistado> UtilizadorRegistado { get; set; }
		public DbSet<SalasChat> SalasChat { get; set; }
		public DbSet<DestinatarioPedidosAmizade> DestinatarioPedidosAmizade { get; set; }
		public DbSet<RegistadosSalasChat> RegistadosSalasChat { get; set; }
		public DbSet<Mensagem> Mensagem { get; set; }
		public DbSet<Amigo> Amigo { get; set; }
	}
}
