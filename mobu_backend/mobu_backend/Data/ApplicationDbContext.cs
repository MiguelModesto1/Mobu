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

			 modelBuilder.Entity<UtilizadorRegistado>()
			.HasOne(u => u.DonoListaAmigos)              
			.WithMany(u => u.ListaAmigos)       
			.HasForeignKey(u => u.DonoListaAmigosId)
			.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<UtilizadorRegistado>()
			.HasOne(u => u.DonoListaDestinatários)              
			.WithMany(u => u.ListaPedidos)       
			.HasForeignKey(u => u.DonoListaDestinatáriosId)
			.OnDelete(DeleteBehavior.Restrict);

		}

		

		//definir tabelas da BD

		public DbSet<UtilizadorRegistado> UtilizadorRegistado { get; set; }
		public DbSet<SalasChat> SalasChat { get; set; }
		public DbSet<RegistadosSalasChat> RegistadosSalasChat { get; set; }
		public DbSet<Mensagem> Mensagem { get; set; }
	}
}
