namespace mobu_backend.ApiModels
{
    public class Messages
    {
        public int IDMensagem { get; set; }

        public int IDRemetente { get; set; }

        public string URLImagemRemetente { get; set; }

        public string NomeRemetente { get; set; }

        public string ConteudoMsg { get; set; }
    }
}
