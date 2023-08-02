using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        [Display(Name = "ID do Utilizador")]
        [Key]
        public int IDUtilizador { get; set; }

        /// <summary>
        /// Nome do utilizador anonimo ('guest' + nextFreeId)
        /// </summary>
        [Display(Name = "Nome do Utilizador")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		[StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
		[RegularExpression("guest.+")]
		public string NomeUtilizador { get; set; }

        /// <summary>
        /// Endereco IPv4 do dispositivo do utilizador anonimo
        /// </summary>
		[Display(Name = "Endereço IPv4")]
        [StringLength(15, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
	    [RegularExpression("(1{0,1}[0123456789]{1,2}\u002E|2[01234][0123456789]\u002E|25[012345]\u002E){3}(1{0,1}[0123456789]{1,2}|2[01234][0123456789]|25[012345])")]
        public string? EnderecoIPv4 { get; set; }


        /// <summary>
        /// Endereco IPv6 do dispositivo do utilizador anonimo
        /// </summary>
        [Display(Name = "Endereço IPv6")]
        [StringLength(39, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [RegularExpression("([0-9A-Fa-f]{0,4}:){2,7}([0-9A-Fa-f]{0,4}){0,1}")]
        public string? EnderecoIPv6 { get; set; }

    }
}
