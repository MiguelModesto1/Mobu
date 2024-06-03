namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para as pessoas desconhecidas
    /// </summary>
    public class UnkownPerson
    {
        /// <summary>
        /// ID da pessoa desconhecida
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da pessoa desconhecida
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// E-mail da pessoa desconhecida
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// URL da imagem do avatar da pessoa desconhecida
        /// </summary>
        public string ImageURL { get; set; }
    }
}
