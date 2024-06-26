﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mobu_backend.Models
{
    /// <summary>
    /// Representa os dados de um utilizador registado
    /// </summary>
    public class UtilizadorRegistado
    {
        public UtilizadorRegistado()
        {
            ListaSalasDeChat = new HashSet<RegistadosSalasChat>();
            ListaMensagens = new HashSet<Mensagem>();
            PedidosAmizadeEfetuados = new HashSet<Amizade>();
            PedidosAmizadeRecebidos = new HashSet<Amizade>();
        }
        /// <summary>
        /// ID para a tabela do utilizador registado (PK)
        /// </summary>
        [Key]
        [Display(Name = "ID do Utilizador")]
        public int IDUtilizador { get; set; }

        /// <summary>
        /// Nome do utilizador registado
        /// </summary>
        [Display(Name = "Nome do Utilizador")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [RegularExpression("[0-9A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+",
                          ErrorMessage = "No {0} só são aceites letras")]
        public string NomeUtilizador { get; set; }

        /// <summary>
        /// Email do utilizador registado
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [MinLength(6, ErrorMessage = "O {0} tem de ter, no mínimo, {1} caracteres")]
        [StringLength(100, ErrorMessage = "O {0} não pode ter mais do que {1} caracteres.")]
        [EmailAddress(ErrorMessage = "Introuduza um email válido por favor")]
        [Display(Name = "Email do Utilizador")]
        public string Email { get; set; }

        /// <summary>
        /// password do utilizador registado
        /// </summary>
        [NotMapped]
        [Display(Name = "Palavra-passe")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A {0} tem de ter, no mínimo, {1} caracteres")]
        [MaxLength(100, ErrorMessage = "A {0} pode ter até {1} caracteres")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
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
        /// Elemento de ligacao entre a Tabela dos Registados
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

        /// <summary>
        /// Lista de Salas de chat 
        /// </summary>
        public ICollection<RegistadosSalasChat> ListaSalasDeChat { get; set; }

        /// <summary>
        /// Lista de mensagens
        /// </summary>
        public ICollection<Mensagem> ListaMensagens { get; set; }

        /// <summary>
        /// Lista de pedidos de amizade recebidos por um utilizador
        /// </summary>
        public ICollection<Amizade> PedidosAmizadeRecebidos { get; set; }

        /// <summary>
        /// Pedidos de amizade feitos pelo Utilizador a outros utilizadores
        /// </summary>
        public ICollection<Amizade> PedidosAmizadeEfetuados { get; set; }

    }
}
