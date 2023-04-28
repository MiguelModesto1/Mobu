using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    /// <summary>
    /// Representa os dados de um utilizador anonimo
    /// </summary>
    public class Utilizador_Anonimo
    {
        /// <summary>
        /// ID para a tabela do utilizador anonimo (PK)
        /// </summary>
        [Required]
        [Key]
        public int ID_Utilizador { get; set; }

        /// <summary>
        /// Nome do utilizador anonimo ('guest' + nextFreeId)
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("guest.+")]
		public string NomeUtilizador { get; set; }

        /// <summary>
        /// Endereco IP do dispositivo do utilizador anonimo
        /// </summary>
        [Required]
		[StringLength(15, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
	    [RegularExpression("((1{0,1}[0-9]{2}2[0-4][0-9]|25[0-5]\u002E){3}1{0,1}[0-9]{2}2[0-4][0-9]|25[0-5]|([0-9A-Fa-f]{0,4}(:|::|)){0,8})")]
		public string EnderecoIP { get; set; }
        
    }
}
