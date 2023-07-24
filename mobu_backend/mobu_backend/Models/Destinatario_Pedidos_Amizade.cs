using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    public class Destinatario_Pedidos_Amizade
    {
        /// <summary>
        /// ID do Pedido
        /// </summary>
        [Display(Name = "ID do Pedido")]
        [Key]
        public int IDPedido { get; set; }

        /// <summary>
        /// Utilizador que envia o pedido de amizade
        /// </summary>
        [Display(Name = "Remetente")]
        public Utilizador_Registado RemetentePedido { get; set; }

        /// <summary>
        /// ID do Utilizador que recebe o pedido de amizade
        /// </summary>
        [Display(Name = "Destinatário")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int IDDestinatarioPedido { get; set; }


        /// <summary>
        /// Estado do pedido de amizade
        /// 1 - a enviar
        /// 2 - enviado
        /// 3 - recebido
        /// 4 - aceite
        /// 5 - recusado
        /// </summary>
        [EnumDataType(typeof(EstadosPedido))]
        [Display(Name = "Estado do Pedido")]
        public EstadosPedido EstadoPedido { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do Remetente
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Remetente")]
        [ForeignKey(nameof(RemetentePedido))]
        public int RemetenteFK { get; set; }

        public enum EstadosPedido
        {
            A_Enviar = 1,
            Enviado = 2,
            Recebido = 3,
            Aceite = 4,
            Recusado = 5
        }
    }
}
