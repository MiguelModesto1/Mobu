namespace mobu_backend.ApiModels
{
    /// <summary>
    /// Modelo da API para os grupos desconhecidos
    /// </summary>
    public class UnkownGroup
    {
        /// <summary>
        /// ID do grupo desconhecido
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do grupo desconhecido
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// URL da imagem do avatar do grupo
        /// </summary>
        public string ImageURL { get; set; }
    }
}
