namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para a edicao de perfil de grupo
    /// </summary>
    public class GroupProfileEdit
    {
        /// <summary>
        /// Id do grupo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id de um administrador dp grupo
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Nome do grupo
        /// </summary>
        public string NomeSala { get; set; }

        /// <summary>
        /// IFormFile com a fotografia do grupo
        /// </summary>
        public IFormFile Avatar { get; set; }
        
    }
}
