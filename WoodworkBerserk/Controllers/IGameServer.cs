using Microsoft.Xna.Framework.Content;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    interface IGameServer
    {
        void Connect(ContentManager content);
        void Disconnect();
        void Send(PlayerAction action);
        State Receive();
    }
}
