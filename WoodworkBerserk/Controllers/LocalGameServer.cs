using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    class LocalGameServer : IGameServer
    {
        private State state;
        private int playerId;

        /**
         * Setup default state
         */
        public void Connect()
        {
            /*
             * LocalGameServer only makes 1 entity, which is the player
             * As practice it maintains playerId in a way which we expect
             * the actual OnlineGameServer connection to do.
             */
            playerId = 0;
            int[,] terrain = new int[20, 20];
            for (int i = 0; i < 20 * 20; i++)
            {
                terrain[i / 20, i % 20] = 0;
            }
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            entities.Add(playerId, new Entity(new Vector2(5, 5), 1, true));
            state = new State(0, terrain, entities, playerId);
        }

        /**
         * Retrieve current state
         */
        public State Receive()
        {
            return state;
        }

        /**
         * Update state locally
         */
        public void Send(PlayerAction action)
        {
            /*
             * Not at all accurate to how the server behaves,
             * game updates are supposed to happen at a fixed
             * tick-rate, not on a input-received basis.
             */
            Entity player;
            state.entities.TryGetValue(playerId, out player);
            switch (action)
            {
                case PlayerAction.MoveUp:
                    player.position.Y--;
                    break;
                case PlayerAction.MoveDown:
                    player.position.Y++;
                    break;
                case PlayerAction.MoveRight:
                    player.position.X++;
                    break;
                case PlayerAction.MoveLeft:
                    player.position.X--;
                    break;
                case PlayerAction.Interact:
                    break;
                default:
                    break;
            }
        }
    }
}
