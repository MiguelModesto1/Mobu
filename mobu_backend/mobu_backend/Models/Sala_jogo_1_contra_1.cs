using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
    public class Sala_Jogo_1_Contra_1
    {

        public Sala_Jogo_1_Contra_1()
        {
            ListaRegistados = new HashSet<Registados_Salas_Jogo>();
        }

        /// <summary>
        /// ID_Sala de jogo
        /// </summary>
        [Key]
        public int ID_Sala { get; set; }
		public ICollection<Registados_Salas_Jogo> ListaRegistados { get; set; }
	}
}
