using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    public class PedidosAmizade
    {

        /// <summary>
        /// Chave primaria
        /// </summary>
        [Key]
        [Display(Name = "ID do pedido")]
        public int IdPedido { get; set; }

        /// <summary>
        /// Referencia ao dono da lista de remetentes de pedidos de amizade, proveniente do autorrelacionamento
        /// </summary>
        public int DonoListaPedidosFK { get; set; }
        
        [Display(Name = "Dono da lista")]
        public UtilizadorRegistado DonoListaPedidos { get; set; }

        /// <summary>
        /// Referencia ao remetente do pedido de amizade, proveniente do autorrelacionamento
        /// </summary>
        public int RemetenteFK { get; set; }
        public UtilizadorRegistado Remetente { get; set; }
    }
}
