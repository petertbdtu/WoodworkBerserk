using Microsoft.Xna.Framework.Content;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    interface IGameClient
    {
        bool Connect(string username, string password);
        void Disconnect();
        void SendAction(PlayerAction action);
        State GetState();
    }
}
