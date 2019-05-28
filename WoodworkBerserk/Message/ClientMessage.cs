using System;
using System.Net;
using System.Text;
using WoodworkBerserk.Controllers;

namespace WoodworkBerserk.Message
{
    enum ClientMessageType : byte
    {
        Invalid = 0,
        Connect = 1,
        Disconnect = 2,
        Command = 3
    }
    abstract class ClientMessage
    {
        public byte[] Data { get; set; }
        abstract public ClientMessageType GetClientMessageType();
    }
    class ClientMessageInvalid : ClientMessage
    {
        public ClientMessageInvalid(int assumedPlayerId)
        {
            Data = new byte[5];
            Data[0] = (byte)ClientMessageType.Invalid;
            byte[] apid = BitConverter.GetBytes(assumedPlayerId);
            Data[1] = apid[0];
            Data[2] = apid[1];
            Data[3] = apid[2];
            Data[4] = apid[3];
        }
        public override ClientMessageType GetClientMessageType()
        {
            return ClientMessageType.Invalid;
        }
    }
    class ClientMessageConnect : ClientMessage
    {
        public ClientMessageConnect(String username, String password)
        {
            int dataSize = 9 + username.Length + password.Length;
            Data = new byte[dataSize];
            Data[0] = (byte)ClientMessageType.Connect;
            byte[] apid = BitConverter.GetBytes(-1);
            Data[1] = apid[0];
            Data[2] = apid[1];
            Data[3] = apid[2];
            Data[4] = apid[3];

            int splitLoc = 9 + username.Length;
            System.Diagnostics.Debug.WriteLine("splitLoc="+splitLoc);
            byte[] splitLocBytes = BitConverter.GetBytes(splitLoc);
            Data[5] = splitLocBytes[0];
            Data[6] = splitLocBytes[1];
            Data[7] = splitLocBytes[2];
            Data[8] = splitLocBytes[3];

            byte[] nameBytes = Encoding.ASCII.GetBytes(username);
            for (int i = 0; i < username.Length; i++)
            {
                Data[9 + i] = nameBytes[i];
            }
            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            for (int i = 0; i < password.Length; i++)
            {
                Data[splitLoc + i] = passBytes[i];
            }
        }
        public override ClientMessageType GetClientMessageType()
        {
            return ClientMessageType.Connect;
        }
    }
    class ClientMessageDisconnect : ClientMessage
    {
        public ClientMessageDisconnect(int assumedPlayerId)
        {
            Data = new byte[5];
            Data[0] = (byte)ClientMessageType.Disconnect;
            byte[] apid = BitConverter.GetBytes(assumedPlayerId);
            Data[1] = apid[0];
            Data[2] = apid[1];
            Data[3] = apid[2];
            Data[4] = apid[3];
        }
        public override ClientMessageType GetClientMessageType()
        {
            return ClientMessageType.Disconnect;
        }
    }
    class ClientMessageCommand : ClientMessage
    {
        public ClientMessageCommand(int assumedPlayerId, PlayerAction playerAction)
        {
            Data = new byte[9];
            Data[0] = (byte)ClientMessageType.Command;

            byte[] idBytes = BitConverter.GetBytes(assumedPlayerId);
            Data[1] = idBytes[0];
            Data[2] = idBytes[1];
            Data[3] = idBytes[2];
            Data[4] = idBytes[3];

            byte[] actionBytes = BitConverter.GetBytes((int)playerAction);
            Data[5] = actionBytes[0];
            Data[6] = actionBytes[1];
            Data[7] = actionBytes[2];
            Data[8] = actionBytes[3];
        }
        public override ClientMessageType GetClientMessageType()
        {
            return ClientMessageType.Command;
        }
    }
}
