using mobu_backend.ApiModels;

namespace mobu_backend.Hubs.Objects
{
    /// <summary>
    /// Modelo do Hub para recolher dados de amigos
    /// </summary>
    public class FriendObjectFromHub
    {
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
    }
}
