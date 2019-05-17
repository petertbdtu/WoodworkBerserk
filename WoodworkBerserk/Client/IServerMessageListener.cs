using WoodworkBerserk.Message;

namespace WoodworkBerserk.Client
{
    interface IServerMessageCallback
    {
        void Call(ServerMessage msg);
    }
}
