namespace mobu_backend.ApiModels
{
    public class FriendObject
    {
        public int ItemId { get; set; }
        public int FriendId { get; set; }
        public string FriendName { get; set; }
        public string FriendEmail { get; set; }
        public int CommonRoomId { get; set; }
        public string ImageURL { get; set; }
        public Messages[] Messages { get; set; }
    }
}
