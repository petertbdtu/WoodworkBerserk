using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    class AsyncGameClient : IGameClient
    {
        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        
        Socket client;
        byte[] received;


        private State state;
        private int playerId;
        private Texture2D texture;
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



            try
            {
                // Establish the remote endpoint for the socket.  
                IPAddress ipAddress = IPAddress.Loopback;
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void Disconnect()
        {
            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
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


            // Send test data to the remote device.
            byte[] msg = { 5, 250, 33, 9, 0 };
            NewSend(client, msg);
            sendDone.WaitOne();
        }

        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Remaining data
            public int bytesLeft = 0;
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Trace.WriteLine("Socket connected to "+
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        private void NewReceive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                if (state.bytesLeft == 0)
                {
                    // New transmission.
                    state.bytesLeft = state.buffer[0];
                }

                state.bytesLeft -= bytesRead;

                if (state.bytesLeft == 0)
                {
                    received = new byte[state.buffer[0]];
                    for (int i = 0; i < received.Length; i++)
                        received[i] = state.buffer[i];
                    receiveDone.Set();
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                }
            }
        }

        private void NewSend(Socket client, byte[] data)
        {
            // Begin sending the data to the remote device.  
            client.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Trace.WriteLine("Sent "+ bytesSent+" bytes to server.");

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
