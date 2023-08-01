using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace mobu_backend.Models
{
    public class Admin
    {
        public Admin()
        {
            Fotografia = new Fotografia_Admin();
        }

        /// <summary>
        /// ID do administrador
        /// </summary>
        [Key]
        [Display(Name = "ID do Administrador")]
        public int IDAdmin { get; set; }

        /// <summary>
        /// Nome de utilizador do administrador
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Nome do Administrador")]
        [StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [RegularExpression("[0-9A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
                          ErrorMessage = "No {0} só são aceites letras")]
        public string NomeAdmin { get; set; }

        /// <summary>
        /// Hash da password do administrador
        /// </summary>
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [Display(Name = "Palavra-passe")]
        public string Password { get; set; }

        /// <summary>
        /// Email do administrador
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [MinLength(6, ErrorMessage = " O {0} tem de ter, no mínimo, {1} caracteres")]
        [StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        [Display(Name = "Email do Administrador")]
        public string Email { get; set; }

        
        [ForeignKey(nameof(Fotografia))]
        public int IDFotografia { get; set; }
        public Fotografia_Admin Fotografia { get; set; }


    }
}
