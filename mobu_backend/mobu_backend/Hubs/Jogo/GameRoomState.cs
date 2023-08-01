namespace mobu_backend.Hubs.Jogo
{
    public class GameRoomState
    {
        /// <summary>
        /// ID da sala
        /// </summary>
        public int IDSala { get; set; }

        /// <summary>
        /// pontos do Fundador
        /// </summary>
        public int PontosFundador { get; set; }

        /// <summary>
        /// pontos do Convidado
        /// </summary>
        public int PontosConvidado { get; set; }
    }
}