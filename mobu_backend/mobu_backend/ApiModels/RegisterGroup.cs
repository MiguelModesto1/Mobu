namespace mobu_backend.Api_models
{
    /// <summary>
    /// Modelo da API para o registo de grupos
    /// </summary>
    public class RegisterGroup
    {
        /// <summary>
        /// ID do administrador do grupo
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Nome de grupo a registar
        /// </summary>
        public string NomeSala { get; set; }

        /// <summary>
        /// Avatar a registar
        /// </summary>
        public IFormFile Avatar { get; set; }
    }
}
