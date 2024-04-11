using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mobu_backend.Models
{
    public class DestinatarioPedidosAmizade
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
        public UtilizadorRegistado RemetentePedido { get; set; }

        /// <summary>
        /// ID do Utilizador que recebe o pedido de amizade
        /// </summary>
        [Display(Name = "Destinatário")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int IDDestinatarioPedido { get; set; }

        /// <summary>
        /// Data e hora do ultimo estado do pedido
        /// </summary>
        [Required]
        [Display(Name = "Data-Hora do pedido")]
        public DateTime DataHoraPedido { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do Remetente
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Remetente")]
        [ForeignKey(nameof(RemetentePedido))]
        public int RemetenteFK { get; set; }
    }
}
