﻿namespace mobu_backend.Hubs.Chat
{
    public interface IChatClient
    {
        Task<string> GetMessage();
        Task ReceiveMessage(string user, string meassage,int idMsg);
    }
}
