using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mobu_backend.Models
{
    [PrimaryKey(nameof(IDMensagem), nameof(SalaFK))]
    public class Mensagem
    {
        /// <summary>
        /// ID da Mensagem
        /// </summary>
        [Required]
        public int IDMensagem { get; set; }

        /// <summary>
        /// ID do remetente
        /// </summary>
        [Required]
        public Utilizador_Registado Remetente { get; set; }

        /// <summary>
        /// ID de Sala de destino da mensagem
        /// </summary>
        [Required]
        public Salas_Chat Sala { get; set; }

        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        [Required]
		[StringLength(256, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		public string ConteudoMsg { get; set; }

        /// <summary>
        /// Estado da mensagem;
        /// 1 - A enviar;
        /// 2 - Enviada;
        /// 3 - Recebida;
        /// 4 - Vista;
        /// </summary>
        [EnumDataType(typeof(EstadosMensagem))]
        public EstadosMensagem EstadoMensagem { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do utilizador
        /// remetente
        /// </summary>
        [Required]
        [ForeignKey(nameof(Remetente))]
        public int RemetenteFK { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala de
        /// chat
        /// </summary>
        [Required]
        [ForeignKey(nameof(Sala))]
        public int SalaFK { get; set; }

        public enum EstadosMensagem
        {
            A_Enviar = 1,
            Enviada = 2,
            Recebida = 3,
            Vista = 4
        }
    }

    
}
