﻿using Microsoft.Xna.Framework.Content;
using System;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    class GameServer : IGameServer
    {

        /**
         * Start thread which receives state from server & maintains
         */
        public void Connect(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        /**
         * Retrieve current state from this object
         */
        public State Receive()
        {
            throw new NotImplementedException();
        }

        /**
         * Send actions to server
         */
        public void Send(PlayerAction action)
        {
            throw new NotImplementedException();
        }
    }
}
