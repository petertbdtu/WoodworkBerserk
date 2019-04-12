using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    interface IGameServer
    {
        void Connect(/*?*/);
        void Send(PlayerAction action);
        State Receive();
    }
}
