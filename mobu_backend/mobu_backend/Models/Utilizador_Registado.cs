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
            ListaDetinatarios = new HashSet<Destinatario_Pedidos_Amizade>();
            ListaAmigos = new HashSet<Amigo>();
            ListaSalasJogo = new HashSet<Registados_Salas_Jogo>();
		}
        /// <summary>
        /// ID para a tabela do utilizador registado (PK)
        /// </summary>
        [Key]
        [Display(Name = "ID do Utilizador")]
        public int IDUtilizador { get; set; }

        /// <summary>
        /// Nome do utilizador registado
        /// </summary>
        [Display(Name = "Nome do Utilizador")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("[0-9A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
						  ErrorMessage = "No {0} só são aceites letras")]
		public string NomeUtilizador { get; set; }

        /// <summary>
        /// Email do utilizador registado
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [MinLength(6, ErrorMessage = " O {0} tem de ter, no mínimo, {1} caracteres")]
		[StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        [Display(Name = "Email do Utilizador")]
        public string Email { get; set; }

        /// <summary>
        /// Hash da password do utilizador registado
        /// </summary>
        [Display(Name = "Palavra-passe")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [MaxLength(96)]
        [MinLength(96)]
        public string Password { get; set; }

        /// <summary>
        /// Fotografia
        /// </summary>
        public string Fotografia { get; set; }



        public ICollection<Registados_Salas_Chat> ListaSalasDeChat { get; set; }
        public ICollection<Mensagem> ListaMensagensEnviadas { get; set; }
		public ICollection<Destinatario_Pedidos_Amizade> ListaDetinatarios { get; set; }
		public ICollection<Amigo> ListaAmigos { get; set; }
		public ICollection<Registados_Salas_Jogo> ListaSalasJogo { get; set; }
	}
}
