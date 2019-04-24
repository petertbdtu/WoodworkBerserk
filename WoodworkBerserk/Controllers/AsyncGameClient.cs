using System.Collections.Generic;
using System.Net;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;
using WoodworkBerserk.Client;
using WoodworkBerserk.Message;

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

        public void Connect()
        {
            client = new WBClient(server, PORT + 1, PORT);
            client.Connect(serverMessageHandler);
            client.Send(new ClientMessageConnect());

            // Semi-busy wait for player ID from server. If lost it never continues :(
            // TODO implement login
            /*while (ownPlayerId == -1)
            {
                ServerMessage initMessage = serverMessageHandler.GetLatestServerMessage();
                if (initMessage.GetServerMessageType() == ServerMessageAccepted)
                {
                    ownPlayerId = ((ServerMessageAccepted)initMessage).ConnectionId;
                }
                System.Threading.Thread.Sleep(100);
            }*/
            ownPlayerId = 0;
        }

        public void Disconnect()
        {
            client.Send(new ClientMessageDisconnect());
            // TODO wait until confirmation
            client.StopListening();
        }
        
        public State GetState()
        {
            // Prepare to accept new user input
            lastAction = PlayerAction.Nothing;

            State newState;

            ServerMessage message = serverMessageHandler.GetLatestServerMessage();
            if (message != null)
            {
                // TODO actually build state from message.
                newState = new State(0, new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } }, new Dictionary<int, Entity>(), 0);

                // Prepare to accept new user input
                lastAction = PlayerAction.Nothing;
            } else
            {
                newState = oldState;
            }

            return newState;
        }

        public void SendAction(PlayerAction action)
        {
            if (action != lastAction)
            {
                lastAction = action;

                ClientMessageCommand commandMessage = new ClientMessageCommand();
                commandMessage.ConnectionId = ownPlayerId;
                commandMessage.PlayerAction = action;
                client.Send(commandMessage);
            }
        }
    }
}
