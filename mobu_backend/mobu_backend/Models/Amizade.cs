﻿using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Models
{
    public class Amizade
    {

        /// <summary>
        /// Data do pedido
        /// </summary>
        [Display(Name = "Data do pedido")]
        public DateTime DataPedido { get; set; }

        /// <summary>
        /// Data de avaliação do pedido
        /// </summary>
        [Display(Name = "Data da resposta")]
        public DateTime? DataResposta { get; set; }

        /// <summary>
        /// Amizade bloqueada ou não
        /// </summary>
        public bool? Desbloqueado { get; set; }

        /// <summary>
        /// Referência ao dono da lista de remetentes de pedidos de amizade, proveniente do autorrelacionamento
        /// </summary>
        [Display(Name = "ID do destinatário")]
        public int DestinatarioFK { get; set; }
        
        [Display(Name = "Destinatário")]
        public UtilizadorRegistado Destinatario { get; set; }

        /// <summary>
        /// Referência ao remetente do pedido de amizade, proveniente do autorrelacionamento
        /// </summary>
        [Display(Name = "ID do remetente")]
        public int RemetenteFK { get; set; }
        public UtilizadorRegistado Remetente { get; set; }
    }
}
