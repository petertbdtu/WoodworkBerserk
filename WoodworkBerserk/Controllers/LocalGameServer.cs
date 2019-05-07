﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    [Synchronization]
    class LocalGameServer : IGameServer
    {
        private State state;
        private int playerId;
        private Texture2D texture;
        private Timer timer;
        private PlayerAction currentAction;

        /**
         * Setup default state
         */
        public void Connect(ContentManager content)
        {
            /*
             * LocalGameServer only makes 1 entity, which is the player
             * As practice it maintains playerId in a way which we expect
             * the actual OnlineGameServer connection to do.
             */
            timer = new Timer(333);
            timer.Elapsed += OnTick;
            timer.Start();

            playerId = 0;
            int[,] terrain = new int[20, 20];
            for (int i = 0; i < 20 * 20; i++)
            {
                terrain[i / 20, i % 20] = 0;
            }
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            entities.Add(playerId, new Entity(new Vector2(5, 5), 1));
            entities.Add(1, new Entity(new Vector2(1, 1), 1));
            state = new State(0, terrain, entities, playerId);
        }

        public void Disconnect()
        {
            timer.Close();
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
            currentAction = action;
        }

        private void OnTick(Object source, ElapsedEventArgs e)
        {
            Entity player;
            state.entities.TryGetValue(playerId, out player);
            switch (currentAction)
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

            Send(PlayerAction.Nothing);
        }
    }
}