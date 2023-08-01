﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
	public class Registados_Salas_Jogo
	{

		/// <summary>
		/// ID do registo do Utilizador na sala de jogo
		/// </summary>
		[Key]
		public int IDRegisto { get; set; }

		/// <summary>
		/// Utilizador Registado na Sala
		/// </summary>
		public Utilizador_Registado Utilizador { get; set; }

		/// <summary>
		/// Sala de chat
		/// </summary>
		public Sala_Jogo_1_Contra_1 Sala { get; set; }

        /// <summary>
        /// Guarda o valor que representa se o utilizador 
        /// e fundador da sala ou nao
        /// 
        /// true - Fundador
        /// false - Convidado
        /// </summary>
        [Display(Name = "Fundador")]
        public bool IsFundador { get; set; }

		/// <summary>
		/// Chave forasteira que referencia o ID do Utilizador
		/// </summary>
		[ForeignKey(nameof(Utilizador))]
		[Required]
        [Display(Name = "Utilizador")]
        public int UtilizadorFK { get; set; }

		/// <summary>
		/// Chave forasteira que referencia o ID da sala
		/// </summary>
		[ForeignKey(nameof(Sala))]
		[Required]
        [Display(Name = "Sala")]
        public int SalaFK { get; set; }
	}
}