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
		/// Este método é executado imediatamente antes 
		/// da criação do Modelo.
		/// É utilizado para adicionar as últimas instruções
		/// à criação do modelo
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Amizade>().HasKey(f => new { f.DestinatarioFK, f.RemetenteFK });
			modelBuilder.Entity<Amizade>()
			.HasOne(u => u.Destinatario)              
			.WithMany(u => u.PedidosAmizadeRecebidos)       
			.HasForeignKey(u => u.DestinatarioFK)
			.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Amizade>()
			.HasOne(u => u.Remetente)              
			.WithMany(u => u.PedidosAmizadeEfetuados)       
			.HasForeignKey(u => u.RemetenteFK)
			.OnDelete(DeleteBehavior.Restrict);

		}

		

		//definir tabelas da BD

		public DbSet<UtilizadorRegistado> UtilizadorRegistado { get; set; }
		public DbSet<Amizade> Amizade { get; set; }
		public DbSet<SalasChat> SalasChat { get; set; }
		public DbSet<RegistadosSalasChat> RegistadosSalasChat { get; set; }
		public DbSet<Mensagem> Mensagem { get; set; }
	}
}
