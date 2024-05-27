namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para o membro do grupo
    /// </summary>
    public class GroupMember
    {
        /// <summary>
        /// ID do item do membro
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// ID do membro
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome de utilizador do membro
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// URL da imagem do avatar do membro
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// booleano que avalia administacao do grupo
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
