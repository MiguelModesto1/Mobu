using System.ComponentModel.DataAnnotations;

namespace mobu_backend.Hubs.Chat
{
    public class Message
    {
        /// <summary>
        /// ID da relacao Sala <-> Mensagem
        /// </summary>
        public int IDMensagemSala { get; set; }

        /// <summary>
        /// ID da Mensagem
        /// </summary>
        public int IDMensagem { get; set; }

        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        public string ConteudoMsg { get; set; }

        /// <summary>
        /// Data e hora do ultimo estado da mensagem
        /// </summary>
        public DateTime DataHoraMsg { get; set; }

        /// <summary>
        /// Estado da mensagem;
        /// 1 - A enviar;
        /// 2 - Enviada;
        /// 3 - Recebida;
        /// 4 - Vista;
        /// </summary>
        public MessageState EstadoMensagem { get; set; }

        /// <summary>
        /// ID do utilizador remetente
        /// </summary>
        public int IDRemetente { get; set; }

        /// <summary>
        /// ID da sala de chat
        /// </summary>
        public int IDSala { get; set; }

        /// <summary>
        /// Possíveis estados da mensagem
        /// </summary>
        public enum MessageState
        {
            A_Enviar = 1,
            Enviada = 2,
            Recebida = 3,
            Vista = 4
        }
    }
}