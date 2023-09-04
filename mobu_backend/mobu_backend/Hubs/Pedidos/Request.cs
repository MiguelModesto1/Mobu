
namespace mobu_backend.Hubs.Pedidos
{
    public class Request
    {

        /// <summary>
        /// ID do remetente na BD
        /// </summary>
        public string IDRemetente { get; set; }

        /// <summary>
        /// ID do destinatario na BD
        /// </summary>
        public string IDDestinatario { get; set; }

        /// <summary>
        /// Estado do pedido
        /// </summary>
        public ReqState EstadoPedido { get; set; }

        /// <summary>
        /// Enumeracao de estados do pedido
        /// </summary>
        public enum ReqState { A_Enviar = 1, Enviado = 2, Recebido = 3, Aceite = 4, Recusado = 5 }
    }
}