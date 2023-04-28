using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    /// <summary>
    /// Representa os dados de um utilizador registado
    /// </summary>
    public class Utilizador_Registado
    {
        public Utilizador_Registado()
        {
            ListaSalasDeChat = new HashSet<Registados_Salas_Chat>();
            ListaMensagensEnviadas = new HashSet<Mensagem>();
            ListaPedidosEnviados = new HashSet<Pedidos_Amizade>();
            ListaPedidosRecebidos = new HashSet<Pedidos_Amizade>();
            ListaSalasJogo = new HashSet<Registados_Salas_Jogo>();
		}
        /// <summary>
        /// ID para a tabela do utilizador registado (PK)
        /// </summary>
        [Required]
        [Key]
        public int ID_Utilizador { get; set; }

        /// <summary>
        /// Nome do utilizador registado
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
						  ErrorMessage = "No {0} só são aceites letras")]
		public string NomeUtilizador { get; set; }

        /// <summary>
        /// Email do utilizador registado
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [MinLength(6, ErrorMessage = " O {0} tem de ter, no mínimo, {1} caracteres")]
		[StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        public string Email { get; set; }

        /// <summary>
        /// Hash da password do utilizador registado
        /// </summary>
        [Required]
        [MaxLength(96)]
        [MinLength(96)]
        public string passwHash { get; set; }
        public ICollection<Registados_Salas_Chat> ListaSalasDeChat { get; set; }
        public ICollection<Mensagem> ListaMensagensEnviadas { get; set; }
		public ICollection<Pedidos_Amizade> ListaPedidosEnviados { get; set; }
		public ICollection<Pedidos_Amizade> ListaPedidosRecebidos { get; set; }
		public ICollection<Registados_Salas_Jogo> ListaSalasJogo { get; set; }
	}
}
