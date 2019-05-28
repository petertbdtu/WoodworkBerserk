using System.Collections.Generic;
using System.Net;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;
using WoodworkBerserk.Client;
using WoodworkBerserk.Message;
using System;

namespace WoodworkBerserk
{
    class AsyncGameClient : IGameClient
    {
        public static readonly IPAddress server = IPAddress.Loopback;
        public static readonly int PORT = 19567;

        private WBClient client;
        private ServerMessageHandler serverMessageHandler;
        private int ownPlayerId = -1;

        private State oldState = new State(0, new int[,] { { 0 } }, new Dictionary<int, Entity>(), 0);
        private PlayerAction lastAction;
        
        public AsyncGameClient()
        {
            serverMessageHandler = new ServerMessageHandler();
            lastAction = PlayerAction.Nothing;
        }

        public bool Connect(string username, string password)
        {
            client = new WBClient(server, PORT + 1, PORT);
            client.Connect(serverMessageHandler);
            client.Send(new ClientMessageConnect(username, password));

            // Semi-busy wait for player ID from server. If lost it never continues
            bool receivedResponse = false;
            bool gotId = false;
            while (!receivedResponse)
            {
                ServerMessage initMessage = serverMessageHandler.GetLatestServerMessage();
                if (initMessage != null)
                {
                    receivedResponse = true;
                    if (initMessage.GetServerMessageType() == ServerMessageType.Update)
                    {
                        ownPlayerId = ((ServerMessageUpdate)initMessage).PlayerId;
                        // -1 means failed to log in.
                        gotId = ownPlayerId != -1;
                    }

                }
                System.Threading.Thread.Sleep(100);
            }
            return gotId;
        }

        public void Disconnect()
        {
            client.Send(new ClientMessageDisconnect(ownPlayerId));
            // TODO wait until confirmation
            client.StopListening();
        }
        
        public State GetState()
        {
            // Prepare to accept new user input
            lastAction = PlayerAction.Nothing;

            State newState = oldState;

            ServerMessage message = serverMessageHandler.GetLatestServerMessage();
            if (message != null)
            {
                if (message.GetServerMessageType() == ServerMessageType.Update)
                {
                    ServerMessageUpdate update = (ServerMessageUpdate)message;
                    // TODO actually build state from message.
                    int playerId = update.PlayerId;
                    int[,] terrain = new int[update.MapWidth, update.MapHeight];
                    for (int x = 0; x < update.MapWidth; x++)
                    {
                        for (int y = 0; y < update.MapHeight; y++)
                        {
                            terrain[x, y] = update.TerrainData[y*update.MapWidth+x];
                        }
                    }
                    Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
                    for (int i = 0; i < update.EntitiesData.Length; i+=4)
                    {
                        int entityId = update.EntitiesData[i];
                        int textureId = update.EntitiesData[i + 1];
                        int xcoord = update.EntitiesData[i + 2];
                        int ycoord = update.EntitiesData[i + 3];
                        entities.Add(entityId, new Entity(xcoord, ycoord, textureId));
                    }
                    foreach (int k in entities.Keys)
                    {
                        Entity e;
                        entities.TryGetValue(k, out e);
                    }
                    newState = new State(0, terrain, entities, playerId);

                    // Prepare to accept new user input
                    lastAction = PlayerAction.Nothing;

                }
            }

            oldState = newState;
            return newState;
        }

        public void SendAction(PlayerAction action)
        {
            if (action != lastAction)
            {
                lastAction = action;

                ClientMessageCommand commandMessage = new ClientMessageCommand(ownPlayerId, lastAction);
                client.Send(commandMessage);
            }
        }
    }
}
