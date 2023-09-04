namespace mobu_backend.Hubs.Jogo
{
    public interface IGameHub
    {
        Task<GameRoomState> GetGameRoomState();

        Task ReceiveGameRoomState(GameRoomState gameRoomState);

        Task ReceiveChallenge(string user, bool interested);
    }
}
