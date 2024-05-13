namespace mobu_backend.ApiModels
{
    internal class GroupObject
    {
        public int ItemId { get; set; }
        public int IDSala { get; set; }
        public string NomeSala { get; set; }
        public string ImageURL { get; set; }
        public Messages[] Mensagens { get; set; }
    }
}