using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    public class Amizade
    {
        /// <summary>
        /// Chave primaria
        /// </summary>
        [Key]
        public int IdAmizade { get; set; }

        /// <summary>
        /// Referencia ao dono da lista de amigos, proveniente do autorrelacionamento
        /// </summary>
        public int DonoListaAmigosFK { get; set; }

        [Display(Name = "Dono da lista")]
        public UtilizadorRegistado DonoListaAmigos { get; set; }

        /// <summary>
        /// Referencia o amigo, proveniente do autorrelacionamento
        /// </summary>
        public int AmigoFK { get; set; }
        public UtilizadorRegistado Amigo { get; set; }

    }
}
