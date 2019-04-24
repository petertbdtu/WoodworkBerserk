using Microsoft.Xna.Framework.Content;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    interface IGameClient
    {
        void Connect();
        void Disconnect();
        void SendAction(PlayerAction action);
        State GetState();
    }
}
