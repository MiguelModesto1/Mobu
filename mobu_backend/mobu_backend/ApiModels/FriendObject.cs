namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para recolher dados de amigos
    /// </summary>
    public class FriendObject
    {
        /// <summary>
        /// ID do item da lista de amigos
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// ID do amigo
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Nome do amigo
        /// </summary>
        public string FriendName { get; set; }

        /// <summary>
        /// E-mail do amigo
        /// </summary>
        public string FriendEmail { get; set; }

        /// <summary>
        /// Sala privada em comum com o amigo
        /// </summary>
        public int CommonRoomId { get; set; }

        /// <summary>
        /// URL do avatar do amigo
        /// </summary>
        public string ImageURL { get; set; }
        
        /// <summary>
        /// Array de mensagens trocadas com o amigo
        /// </summary>
        public Messages[] Messages { get; set; }
    }
}
