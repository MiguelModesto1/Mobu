using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
    [PrimaryKey(nameof(UtilizadorFK),nameof(SalaFK))]
    public class RegistadosSalasChat
    {

        /// <summary>
        /// Guarda o valor que representa se o utilizador 
        /// e administrador da sala ou nao
        /// 
        /// true - Utilizador
        /// false - nao administrador
        /// </summary>
        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do Utilizador
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Utilizador")]
        public int UtilizadorFK { get; set; }
        /// <summary>
        /// Utilizador Registado na Sala
        /// </summary>
        public UtilizadorRegistado Utilizador { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID da sala
        /// </summary>
        [ForeignKey(nameof(Sala))]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Sala")]
        public int SalaFK { get; set; }
        /// <summary>
        /// Sala de chat
        /// </summary>
        public SalasChat Sala { get; set; }
    }
}