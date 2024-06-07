namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para as menagens
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// ID da mensagm
        /// </summary>
        public int IDMensagem { get; set; }

        /// <summary>
        /// ID do remetente da mensagem
        /// </summary>
        public int IDRemetente { get; set; }

        /// <summary>
        /// URL do avatar do remetente
        /// </summary>
        public string URLImagemRemetente { get; set; }

        /// <summary>
        /// Nome do remetente
        /// </summary>
        public string NomeRemetente { get; set; }

        /// <summary>
        /// Conteudo da mensagem
        /// </summary>
        public string ConteudoMsg { get; set; }
    }
}
