namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para recolher dados de grupos
    /// </summary>
    public class GroupObject
    {   
        /// <summary>
        /// ID do item da lista de salas
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// ID da sala
        /// </summary>
        public int IDSala { get; set; }

        /// <summary>
        /// Nome da sala
        /// </summary>
        public string NomeSala { get; set; }

        /// <summary>
        /// URL do avatar da sala
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// Array de mensagens trocadas com o amigo
        /// </summary>
        public Messages[] Mensagens { get; set; }
    }
}