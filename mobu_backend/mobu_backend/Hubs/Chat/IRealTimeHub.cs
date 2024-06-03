using mobu_backend.ApiModels;
using mobu_backend.Hubs.Objects;

namespace mobu_backend.Hubs.Chat
{
    /// <summary>
    /// Define um hub em tempo real para a comunicação entre cliente e servidor.
    /// </summary>
    public interface IRealTimeHub
    {
        /// <summary>
        /// Obtém uma mensagem em tempo real.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona e retorna uma mensagem em forma de string.</returns>
        Task<string> GetMessage();

        /// <summary>
        /// Adiciona uma conexão a um grupo específico.
        /// </summary>
        /// <param name="connectionId">O ID da conexão que será adicionada ao grupo.</param>
        /// <param name="roomId">O ID da sala (grupo) a qual a conexão será adicionada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddedToGroup(string connectionId, string roomId);

        /// <summary>
        /// Remove uma conexão de um grupo específico.
        /// </summary>
        /// <param name="connectionId">O ID da conexão que será removida do grupo.</param>
        /// <param name="roomId">O ID da sala (grupo) da qual a conexão será removida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task RemovedFromGroup(string connectionId, string roomId);

        /// <summary>
        /// Recebe uma mensagem em tempo real.
        /// </summary>
        /// <param name="itemId">O ID do item associado à mensagem.</param>
        /// <param name="isGroup">Indica se a mensagem é para um grupo.</param>
        /// <param name="message">A mensagem a ser recebida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveMessage(int itemId, bool isGroup, Messages message);

        /// <summary>
        /// Notifica a conexão privada do cliente sobre uma mensagem.
        /// </summary>
        /// <param name="message">A mensagem privada a ser enviada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task OnConnectedAsyncPrivate(string message);

        /// <summary>
        /// Notifica a receção de um bloqueio de um utilizador.
        /// </summary>
        /// <param name="fromUser">O utilizador que bloqueou.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveBlock(string fromUser);

        /// <summary>
        /// Notifica a receção de um desbloqueio de um utilizador.
        /// </summary>
        /// <param name="fromUser">O utilizador que desbloqueou.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveUnblock(string fromUser);

        /// <summary>
        /// Notifica a receção de uma entrada de um utilizador.
        /// </summary>
        /// <param name="fromUser">O utilizador que entrou.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveEntry(string fromUser);

        /// <summary>
        /// Notifica a receção da saída de um utilizador.
        /// </summary>
        /// <param name="itemId">O ID do item utilizador que saiu.</param>
        /// <param name="fromUser">O utilizador que saiu.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveLeaving(string itemId, string fromUser);

        /// <summary>
        /// Recebe um pedido de um utilizador.
        /// </summary>
        /// <param name="user">O utilizador que recebe um pedido.</param>
        /// <param name="fromUsername">O utilizador que fez um pedido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveRequest(string user, string fromUsername);

        /// <summary>
        /// Recebe uma resposta a um pedido de um utilizador.
        /// </summary>
        /// <param name="user">O utilizador que recebeu um pedido.</param>
        /// <param name="reply">A resposta à solicitação (verdadeiro para aceitar, falso para recusar).</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task ReceiveRequestReply(FriendObjectFromHub user, bool reply);

        /// <summary>
        /// Recebe uma expulsao de um grupo
        /// </summary>
        /// <param name="itemId">Indice do item do utilizador expulso.</param>
        /// <param name="message">Mensagem que informa que o utilizador foi expulso.</param>
        /// <returns></returns>
        Task ReceiveExpelling(string itemId, string message);
    }
}
