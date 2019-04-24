using WoodworkBerserk.Message;

namespace WoodworkBerserk.Client
{
    class ServerMessageHandler : IServerMessageCallback
    {
        private object messagesLock = new object();
        ServerMessage latestMessage = null;

        public void Call(ServerMessage msg)
        {
            lock (messagesLock)
            {
                latestMessage = msg;
            }
        }

        public ServerMessage GetLatestServerMessage()
        {
            lock (messagesLock)
            {
                ServerMessage serverMessage = latestMessage;
                latestMessage = null;
                return serverMessage;
            }
        }
    }
}
