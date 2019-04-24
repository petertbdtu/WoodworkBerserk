using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WoodworkBerserk.Message;

namespace WoodworkBerserk.Client
{
    class WBClient
    {
        IPAddress server;
        int ownPort, otherPort;
        UdpClient udpClient;
        IServerMessageCallback listener;
        bool keepConnected;
        Thread t;

        public WBClient(IPAddress server, int ownPort, int otherPort)
        {
            this.server = server;
            this.ownPort = ownPort;
            this.otherPort = otherPort;
        }

        public bool Connect(IServerMessageCallback listener)
        {
            try
            {
                udpClient = new UdpClient(ownPort);
                udpClient.Connect(server, otherPort);
                Console.WriteLine("Connected");
                this.listener = listener;
                keepConnected = true;
                t = new Thread(Receive);
                t.Start();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }
        private void Receive()
        {
            while (keepConnected)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(server, 0);
                try
                {
                    listener.Call(ServerMessage.Parse(udpClient.Receive(ref RemoteIpEndPoint)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public void StopListening()
        {
            keepConnected = false;
            // Is this necessary? Feels safer.
            t.Join();
        }

        public void Send(ClientMessage msg)
        {
            udpClient.Send(msg.Bytes(), msg.NumBytes());
        }
    }
}
