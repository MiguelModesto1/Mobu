using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    public class SalasChat
    {
        public SalasChat()
        {
            ListaMensagensRecebidas = new HashSet<Mensagem>();
            ListaRegistadosSalasChat = new HashSet<RegistadosSalasChat>();

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
        /// 
        /// true - sala de grupo
        /// false - sala privada (2 pessoas)
        /// </summary>
        [Required]
        [Display(Name = "Sala de Grupo")]
        public bool SeGrupo { get; set; }

        /// <summary>
        /// Nome do ficheiro com a fotografia
        /// </summary>
        [Display(Name = "Nome da fotografia")]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? NomeFotografia { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        /// <summary>
        /// data da fotografia
        /// </summary>
        [Display(Name = "Data da fotografia")]
        public DateTime? DataFotografia { get; set; }


        public ICollection<Mensagem> ListaMensagensRecebidas { get; set; }
        public ICollection<RegistadosSalasChat> ListaRegistadosSalasChat { get; set; }
    }
}
