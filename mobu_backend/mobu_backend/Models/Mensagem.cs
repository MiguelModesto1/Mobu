using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
    [PrimaryKey(nameof(ID_Mensagem), nameof(salaFK))]
    public class Mensagem
    {
        /// <summary>
        /// ID da Mensagem
        /// </summary>
        [Required]
        public int ID_Mensagem { get; set; }

        /// <summary>
        /// ID do remetente
        /// </summary>
        [Required]
        public Utilizador_Registado REMETENTE { get; set; }

        /// <summary>
        /// ID de Sala de destino da mensagem
        /// </summary>
        [Required]
        public Salas_Chat SALA { get; set; }

        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        [Required]
		[StringLength(256, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		public string Conteudo_Msg { get; set; }

        /// <summary>
        /// Estado da mensagem;
        /// 1 - A enviar;
        /// 2 - Enviada;
        /// 3 - Recebida;
        /// 4 - Vista;
        /// </summary>
        [EnumDataType(typeof(Estados_Mensagem))]
        public Estados_Mensagem Estado_Mensagem { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do utilizador
        /// remetente
        /// </summary>
        [Required]
        [ForeignKey(nameof(REMETENTE))]
        public int remetenteFK { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala de
        /// chat
        /// </summary>
        [Required]
        [ForeignKey(nameof(SALA))]
        public int salaFK { get; set; }

        public enum Estados_Mensagem
        {
            A_Enviar = 1,
            Enviada = 2,
            Recebida = 3,
            Vista = 4
        }
    }

    
}
