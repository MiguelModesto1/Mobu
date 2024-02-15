using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
	public class Salas_Chat
	{
		public Salas_Chat()
		{
			ListaMensagensRecebidas = new HashSet<Mensagem>();
			ListaRegistadosSalasChat = new HashSet<Registados_Salas_Chat>();
			
		}
		/// <summary>
		/// ID para a tabela das Salas de chat (PK)
		/// </summary>
		[Display(Name = "ID da Sala")]
		[Key]
		public int IDSala { get; set; }

		/// <summary>
		/// Nome da sala de chat
		/// </summary>
		[Display(Name = "Nome da Sala")]
		[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
						  ErrorMessage = "No {0} só são aceites letras")]
		public string NomeSala { get; set; }

		/// <summary>
		/// Atributo que verifica se a sala pertence a um grupo ou apenas a 2 pessoas
		/// </summary>
		[Required]
		[Display(Name = "Sala de Grupo")]
		public bool SeGrupo { get; set; }

		/// <summary>
		/// Nome do ficheiro com a fotografia
		/// </summary>
		[Display(Name = "Nome da fotografia")]
		public string? NomeFotografia { get; set; }

		/// <summary>
		/// data da fotografia
		/// </summary>
		[Display(Name = "Data da fotografia")]
		public DateTime? DataFotografia { get; set; }


		public ICollection<Mensagem> ListaMensagensRecebidas { get; set; }
		public ICollection<Registados_Salas_Chat> ListaRegistadosSalasChat { get; set; }
	}
}
