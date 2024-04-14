using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mobu_backend.Models
{
    public class Mensagem
    {
        /// <summary>
        /// ID da Mensagem
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "ID da Mensagem")]
        public int IDMensagem { get; set; }

        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(256, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [Display(Name = "Conteúdo da Mensagem")]
        public string ConteudoMsg { get; set; }

        /// <summary>
        /// Data e hora do ultimo estado da mensagem
        /// </summary>
        [Required]
        [Display(Name = "Data-Hora da mensagem")]
        public DateTime DataHoraMsg { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do utilizador
        /// remetente
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [ForeignKey(nameof(Remetente))]
        [Display(Name = "Remetente")]
        public int RemetenteFK { get; set; }
        /// <summary>
        /// O remetente
        /// </summary>
        [Display(Name = "Utilizador Registado")]
        public UtilizadorRegistado Remetente { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala de
        /// chat
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [ForeignKey(nameof(Sala))]
        [Display(Name = "Sala")]
        public int SalaFK { get; set; }
        /// <summary>
        /// Sala de destino da mensagem
        /// </summary>
        public SalasChat Sala { get; set; }

    }
}
