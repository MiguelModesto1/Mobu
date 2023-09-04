using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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
        [MinLength(6, ErrorMessage = "O {0} tem de ter, no mínimo, {1} caracteres")]
		[StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        [Display(Name = "Email do Utilizador")]
        public string Email { get; set; }

        /// <summary>
        /// Hash da password do utilizador registado
        /// </summary>
        [NotMapped]
        [Display(Name = "Palavra-passe")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A {0} tem de ter, no mínimo, {1} caracteres")]
        [MaxLength(100, ErrorMessage = "A {0} pode ter até {1} caracteres")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public string Password { get; set; }

        /// <summary>
        /// Data de juncao a plataforma
        /// </summary>
        [Display(Name = "Data de junção")]
        public DateTime DataJuncao { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNasc { get; set; }

        /// <summary>
        /// Elemento de ligacao entre a Tabela dos Registados
        /// no modelo logico e na tabela de Users do Identity
        /// </summary>
        public string AuthenticationID { get; set; }

        /// <summary>
        /// Nome do ficheiro com a fotografia
        /// </summary>
        [Display(Name = "Nome da fotografia")]
        public string NomeFotografia{ get; set; }

        /// <summary>
        /// data da fotografia
        /// </summary>
        [Display(Name = "Data da fotografia")]
        public DateTime DataFotografia { get; set; }

        public ICollection<Registados_Salas_Chat> ListaSalasDeChat { get; set; }
        public ICollection<Mensagem> ListaMensagensEnviadas { get; set; }
		public ICollection<Destinatario_Pedidos_Amizade> ListaDetinatarios { get; set; }
		public ICollection<Amigo> ListaAmigos { get; set; }
		public ICollection<Registados_Salas_Jogo> ListaSalasJogo { get; set; }
        
    }
}
