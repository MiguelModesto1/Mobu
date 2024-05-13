namespace mobu_backend.Api_models
{
    /// <summary>
    /// Modelo da API para o registo
    /// </summary>
    public class Register
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NomeUtilizador { get; set; }
        public DateTime DataNascimento { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
