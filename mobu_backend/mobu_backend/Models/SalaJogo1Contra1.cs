using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
	public class SalaJogo1Contra1
	{

		public SalaJogo1Contra1()
		{
			ListaRegistados = new HashSet<RegistadosSalasJogo>();
		}

		/// <summary>
		/// ID_Sala de jogo
		/// </summary>
		[Display(Name = "ID da Sala")]
		[Key]
		public int IDSala { get; set; }

		public ICollection<RegistadosSalasJogo> ListaRegistados { get; set; }
	}
}
