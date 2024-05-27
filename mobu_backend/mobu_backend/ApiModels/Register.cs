namespace mobu_backend.Api_models
{
    /// <summary>
    /// Modelo da API para o registo
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Email a registar
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password a registar
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Nome de utilizador a registar
        /// </summary>
        public string NomeUtilizador { get; set; }

        /// <summary>
        /// Data de nascimento a registar
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Avatar a registar
        /// </summary>
        public IFormFile Avatar { get; set; }
    }
}
