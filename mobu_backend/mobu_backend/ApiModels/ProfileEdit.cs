namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para a edicao de perfil
    /// </summary>
    public class ProfileEdit
    {
        /// <summary>
        /// Id do utilizador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do utilizador
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email do utilizador
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// IFormFile com a fotografia do utilizador
        /// </summary>
        public IFormFile Avatar { get; set; }

        /// <summary>
        /// Palavra-passe nova
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Palavra-passe atual
        /// </summary>
        public string CurrPassword { get; set; }

        /// <summary>
        /// Nova data de nascimento
        /// </summary>
        public DateTime BirthDate { get; set; }
        
    }
}
