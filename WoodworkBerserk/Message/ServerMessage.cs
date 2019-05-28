using System;
using System.Text;

namespace WoodworkBerserk.Message
{
    enum ServerMessageType : byte
    {
        Invalid = 0,
        Disconnect = 1,
        Update = 2, // used to confirm connection establishment as well.
        Accepted = 3
    }
    abstract class ServerMessage
    {
        public static ServerMessage Parse(byte[] data)
        {
            ServerMessageType type = (ServerMessageType)Enum.ToObject(typeof(ServerMessageType), data[0]);
            ServerMessage msg;
            switch (type)
            {
                case ServerMessageType.Disconnect:
                    msg = new ServerMessageDisconnect(data);
                    break;
                case ServerMessageType.Update:
                    msg = new ServerMessageUpdate(data);
                    break;
                case ServerMessageType.Accepted:
                    msg = new ServerMessageAccepted(data);
                    break;
                default:
                    msg = new ServerMessageInvalid(data);
                    break;
            }

            return msg;
        }
        abstract public ServerMessageType GetServerMessageType();
    }
    class ServerMessageInvalid : ServerMessage
    {
        public byte[] Data { get; }
        public ServerMessageInvalid(byte[] data) { this.Data = data; }
        public override ServerMessageType GetServerMessageType()
        {
            return ServerMessageType.Invalid;
        }
    }

    class ServerMessageDisconnect : ServerMessage
    {
        public byte[] Data { get; }
        public ServerMessageDisconnect(byte[] data) { this.Data = data; }
        public override ServerMessageType GetServerMessageType()
        {
            return ServerMessageType.Disconnect;
        }
    }

    class ServerMessageAccepted : ServerMessage
    {
        public byte[] Data { get; }
        public ServerMessageAccepted(byte[] data)
        {
            int position = 0;
            int switcher = 0;
            string usertext = "";
            string passtext = "";
            for(int i = 0; i > data.Length; i++, position++)
            {
                if (BitConverter.ToString(data, position) == " ")
                    switcher = 1;
                if (switcher == 0)
                    usertext += BitConverter.ToChar(data, position);
                else if (switcher == 1)
                    passtext += BitConverter.ToChar(data, position);
            }
        }
        public override ServerMessageType GetServerMessageType()
        {
            return ServerMessageType.Accepted;
        }
    }

    class ServerMessageUpdate : ServerMessage
    {
        public int PlayerId { get; }
        // TODO move map data to client
        // int mapId { get; set } // replace map stuff below with this.
        public int MapWidth { get; }
        public int MapHeight { get; }
        public int[] TerrainData { get; }
        public int[] EntitiesData { get; }
        public ServerMessageUpdate(byte[] data)
        {
            int pos = 0;
            // data[0] is the type.
            pos += 1;
            PlayerId = BitConverter.ToInt32(data, pos);
            pos += 4;
            MapWidth = BitConverter.ToInt32(data, pos);
            pos += 4;
            MapHeight = BitConverter.ToInt32(data, pos);
            pos += 4;
            TerrainData = new int[MapWidth*MapHeight];
            for (int i = 0; i < TerrainData.Length; i++)
            {
                TerrainData[i] = BitConverter.ToInt32(data, pos);
                pos += 4;
            }
            // TODO carefully handle entity data
            EntitiesData = new int[(data.Length - pos) / 4];
            for (int i = 0; i < EntitiesData.Length; i++)
            {
                EntitiesData[i] = BitConverter.ToInt32(data, pos);
                pos += 4;
            }
        }
        public override ServerMessageType GetServerMessageType()
        {
            return ServerMessageType.Update;
        }
    }
}
