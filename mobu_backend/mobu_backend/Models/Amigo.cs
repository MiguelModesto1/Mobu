using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
    public class Amigo
    {

        /// <summary>
        /// ID da Amizade
        /// </summary>
        [Display(Name = "ID da Amizade")]
        [Key]

        public int IDAmizade { get; set; }

        /// <summary>
        /// ID do amigo
        /// </summary>
        [Display(Name = "ID do Amigo/a")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int IDAmigo { get; set; }

        /// <summary>
        /// Variável que determina se o amigo está bloqueado
        /// </summary>
        [Display(Name = "Bloqueado")]
        [Required(ErrorMessage = "O Bloqueio é de preenchimento obrigatório")]
        [DefaultValue(false)]
        public bool Bloqueado { get; set; }
        
        /// <summary>
        /// Dono da lista de amizades concretizadas
        /// </summary>
        [Display(Name = "Dono da Lista")]
        public UtilizadorRegistado DonoListaAmigos { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o dono da lista
        /// </summary>
        [ForeignKey(nameof(DonoListaAmigos))]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Dono da Lista")]
        public int DonoListaFK { get; set; }

    }
}
