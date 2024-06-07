namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para os pedidos de amizade pendentes
    /// </summary>
    public class PendingRequests
    {
        /// <summary>
        /// ID do destinatario (ou dono) do pedido pendente
        /// </summary>
        public int DestID { get; set; }

        /// <summary>
        /// ID do remetente do pedido de amizade
        /// </summary>
        public int RemID { get; set; }

        /// <summary>
        /// Nome do remetente do pedido de amizade
        /// </summary>
        public string RemName { get; set; }

        /// <summary>
        /// URL da imagem de avatar do remetente
        /// </summary>
        public string ImageURL { get; set; }
    }
}
