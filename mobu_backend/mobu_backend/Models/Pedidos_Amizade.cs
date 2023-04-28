using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mobu_backend.Models
{
    [PrimaryKey(nameof(RemetenteFK),nameof(DestinatarioFK))]
    public class Pedidos_Amizade
    {

        /// <summary>
        /// Utilizador que envia o pedido de amizade
        /// </summary>
        public Utilizador_Registado REMETENTE_PEDIDO { get; set; }

        /// <summary>
        /// Utilizador que recebe o pedido de amizade
        /// </summary>
        public Utilizador_Registado DESTINATARIO_PEDIDO { get; set; }

        /// <summary>
        /// Estado do pedido de amizade
        /// 1 - a enviar
        /// 2 - enviado
        /// 3 - recebido
        /// 4 - aceite
        /// 5 - recusado
        /// </summary>
        [EnumDataType(typeof(Estados_Pedido))]
        public Estados_Pedido Estado_pedido { get; set; }

        /// <summary>
        /// Chave forasteira que referencia o ID do Remetente
        /// </summary>
        public int RemetenteFK;

		/// <summary>
		/// Chave forasteira que referencia o ID do destinatario
		/// </summary>
		public int DestinatarioFK;

		public enum Estados_Pedido
		{
			A_Enviar = 1,
			Enviado = 2,
			Recebido = 3,
			Aceite = 4,
            Recusado = 5
		}
	}
}
