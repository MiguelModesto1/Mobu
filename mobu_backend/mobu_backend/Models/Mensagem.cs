using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mobu_backend.Models
{
    public class Mensagem
    {

        /// <summary>
        /// ID da relacao Sala <-> Mensagem
        /// </summary>
        [Key]
        [Display(Name = "ID Mensagem-Sala")]
        public int IDMensagemSala { get; set; }

        /// <summary>
        /// ID da Mensagem
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "ID da Mensagem")]
        public int IDMensagem { get; set; }

        /// <summary>
        /// O remetente
        /// </summary>
        [Display(Name = "Utilizador Registado")]
        public Utilizador_Registado Remetente { get; set; }

        /// <summary>
        /// Sala de destino da mensagem
        /// </summary>
        public Salas_Chat Sala { get; set; }

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
        public DateTime DataHoraMsg {  get; set; }

        /*/// <summary>
        /// Estado da mensagem;
        /// 1 - A enviar;
        /// 2 - Enviada;
        /// 3 - Recebida;
        /// 4 - Vista;
        /// </summary>
        [EnumDataType(typeof(EstadosMensagem))]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Estado da Mensagem")]
        public EstadosMensagem EstadoMensagem { get; set; }*/

        /// <summary>
        /// Chave forasteira que referencia o ID do utilizador
        /// remetente
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [ForeignKey(nameof(Remetente))]
        [Display(Name = "Remetente")]
        public int RemetenteFK { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala de
        /// chat
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [ForeignKey(nameof(Sala))]
        [Display(Name = "Sala")]
        public int SalaFK { get; set; }

        /*/// <summary>
        /// Possíveis estados da mensagem
        /// </summary>
        public enum EstadosMensagem
        {
            A_Enviar = 1,
            Enviada = 2,
            Recebida = 3,
            Vista = 4
        }*/
    }

    
}
