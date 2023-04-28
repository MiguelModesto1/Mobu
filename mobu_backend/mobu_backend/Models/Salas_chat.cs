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
        [Required]
        [Key]
        public int ID_Sala { get; set; }

        /// <summary>
        /// Nome da sala de chat
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
						  ErrorMessage = "No {0} só são aceites letras")]
		public string Nome_sala { get; set; }

        /// <summary>
        /// Atributo que verifica se a sala pertence a um grupo ou apenas a 2 pessoas
        /// </summary>
        [Required]
        public bool Se_grupo { get; set; }
        public ICollection<Mensagem> ListaMensagensRecebidas { get; set; }
        public ICollection<Registados_Salas_Chat> ListaRegistadosSalasChat { get; set; }
    }
}
