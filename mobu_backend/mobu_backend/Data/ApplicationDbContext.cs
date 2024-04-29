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

			//modelBuilder.Entity<Amizade>().HasKey(f => new { f.DonoListaAmigosFK, f.AmigoFK });
			modelBuilder.Entity<Amizade>()
			.HasOne(u => u.DonoListaAmigos)              
			.WithMany(u => u.Amigos)       
			.HasForeignKey(u => u.DonoListaAmigosFK)
			.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Amizade>()
			.HasOne(u => u.Amigo)              
			.WithMany(u => u.DonosAmigos)       
			.HasForeignKey(u => u.AmigoFK)
			.OnDelete(DeleteBehavior.Restrict);

			//modelBuilder.Entity<PedidosAmizade>().HasKey(f => new { f.DonoListaPedidosFK, f.RemetenteFK });
			modelBuilder.Entity<PedidosAmizade>()
			.HasOne(u => u.DonoListaPedidos)              
			.WithMany(u => u.PedidosRecebidos)       
			.HasForeignKey(u => u.DonoListaPedidosFK)
			.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<PedidosAmizade>()
			.HasOne(u => u.Remetente)              
			.WithMany(u => u.DonosPedidosRecebidos)       
			.HasForeignKey(u => u.RemetenteFK)
			.OnDelete(DeleteBehavior.Restrict);

		}

		

		//definir tabelas da BD

		public DbSet<UtilizadorRegistado> UtilizadorRegistado { get; set; }
		public DbSet<Amizade> Amizade { get; set; }
		public DbSet<PedidosAmizade> PedidosAmizade { get; set; }
		public DbSet<SalasChat> SalasChat { get; set; }
		public DbSet<RegistadosSalasChat> RegistadosSalasChat { get; set; }
		public DbSet<Mensagem> Mensagem { get; set; }
	}
}
