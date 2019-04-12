using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    class LocalGameServer : IGameServer
    {
        private State state;

        /**
         * Setup default state
         */
        public void Connect()
        {
            int[,] terrain = new int[20, 20];
            for (int i = 0; i < 20 * 20; i++)
            {
                terrain[i / 20, i % 20] = 0;
            }
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            entities.Add(0, new Entity(new Vector2(5, 5), 1, true));
            state = new State(0, terrain, entities);
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
            //throw new NotImplementedException();
        }
    }
}
