using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace mobu_backend.Models
{
    public class Admin
    {

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
        [NotMapped]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A {0} tem de ter, no mínimo, {1} caracteres")]
        [MaxLength(100, ErrorMessage = "A {0} pode ter até {1} caracteres")]
        [Display(Name = "Palavra-passe")]
        public string Password { get; set; }

        /// <summary>
        /// Data de juncao a plataforma
        /// </summary>
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        [Display(Name = "Data de junção")]
        public DateTime DataJuncao { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNasc { get; set; }

        /// <summary>
        /// Email do administrador
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [MinLength(6, ErrorMessage = "O {0} tem de ter, no mínimo, {1} caracteres")]
        [StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        [Display(Name = "Email do Administrador")]
        public string Email { get; set; }

        /// <summary>
        /// Elelmento de ligacao entre a Tabela dos Administradores
        /// no modelo logico e na tabela de Users do Identity
        /// </summary>
        public string AuthenticationID { get; set; }


        /// <summary>
        /// Nome do ficheiro com a fotografia
        /// </summary>
        [Display(Name = "Nome da fotografia")]
        public string NomeFotografia { get; set; }

        /// <summary>
        /// data da fotografia
        /// </summary>
        [Display(Name = "Data da fotografia")]
        public DateTime DataFotografia { get; set; }


    }
}
