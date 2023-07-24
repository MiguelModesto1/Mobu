using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
	public class Registados_Salas_Chat
	{

        /// <summary>
        /// ID do registo do Utilizador na sala de chat
        /// </summary>
        [Key]
        public int IDRegisto { get; set; }

		/// <summary>
		/// Utilizador Registado na Sala
		/// </summary>
		public Utilizador_Registado Utilizador { get; set; }

		/// <summary>
		/// Sala de chat
		/// </summary>
		public Salas_Chat Sala { get; set; }

        /// <summary>
        /// Guarda o valor que representa se o utilizador 
        /// e administrador da sala ou nao
        /// 
        /// true - Administrador
        /// false - nao administrador
        /// </summary>
        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }

		/// <summary>
		/// Chave forasteira que referencia o ID do Utilizador
		/// </summary>
		[ForeignKey(nameof(Utilizador))]
		[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Utilizador")]
        public int UtilizadorFK { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala
        /// </summary>
        [ForeignKey(nameof(Sala))]
		[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Sala")]
        public int SalaFK { get; set; }
	}
}