namespace mobu_backend.Models
{
    /// <summary>
    /// Representa os dados de um utilizador anonimo
    /// </summary>
    public class Utilizador_Anonimo
    {
        /// <summary>
        /// PK para a tabela do utilizador anonimo
        /// </summary>
        public int ID_Utilizador { get; set; }

        /// <summary>
        /// Nome do utilizador anonimo ('guest' + nextFreeId)
        /// </summary>
        public string NomeUtilizador { get; set; }

        /// <summary>
        /// Endereco IP do dispositivo do utilizador anonimo
        /// </summary>
        public string EnderecoIP { get; set; }
        
    }
}
