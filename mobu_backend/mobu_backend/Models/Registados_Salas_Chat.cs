using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
	[PrimaryKey(nameof(UtilizadorFK), nameof(SalaFK))]
	public class Registados_Salas_Chat
	{
		/// <summary>
		/// Utilizador Registado na Sala
		/// </summary>
		[Required]
		public Utilizador_Registado Utilizador { get; set; }

		/// <summary>
		/// Sala de chat
		/// </summary>
		[Required]
		public Salas_Chat Sala { get; set; }
		
		/// <summary>
		/// Guarda o valor que representa se o utilizador 
		/// e administrador da sala ou nao
		/// 
		/// true - Administrador
		/// false - nao administrador
		/// </summary>
		public bool IsAdmin { get; set; }

		/// <summary>
		/// Chave forasteira que referencia o ID do Utilizador
		/// </summary>
		[ForeignKey(nameof(Utilizador))]
		[Required]
		public int UtilizadorFK { get; set; }

		/// <summary>
		/// Chave forasteira que referencia o ID da sala
		/// </summary>
		[ForeignKey(nameof(Sala))]
		[Required]
		public int SalaFK { get; set; }
	}
}