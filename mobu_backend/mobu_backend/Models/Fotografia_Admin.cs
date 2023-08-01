using System.ComponentModel.DataAnnotations.Schema;

namespace mobu_backend.Models
{
    public class Fotografia_Admin
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do ficheiro com a fotografia
        /// </summary>
        public string NomeFicheiro { get; set; }

        /// <summary>
        /// nome do local onde a fotografia foi tirada
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// data da fotografia
        /// </summary>
        public DateTime DataFotografia { get; set; }

    }
}
