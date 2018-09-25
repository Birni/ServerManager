using Rcon.Extensions;
using System;
using System.Text;


namespace Rcon
{
    /// <summary>
    /// RCON Packet implementation
    /// </summary>
    public class RconPacket
    {
        private const int sizeIndex = 0;
        private const int idIndex = 4;
        private const int typeIndex = 8;
        private const int bodyIndex = 12;
        private string _body;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Packet Type</param>
        /// <param name="content">Packet content</param>
        public RconPacket(PacketType type, string content)
        {
            if (type == null)
                throw new ArgumentException("Error: type parameter must not be null");

            Type = type;
            _body = content ?? string.Empty;
            Id = Environment.TickCount;
        }

        private RconPacket() { }

        /// <summary>
        /// Gets the packet size according to RCON Protocol
        /// </summary>
        /// <remarks>This value is equal to 10 (fixed bytes) + body lenght. The 4 bytes for the Size field are not added</remarks>
        public int Size
        {
            get { return _body.Length + 10; }
        }

        /// <summary>
        /// Gets or Sets the packet id
        /// </summary>
        /// <remarks>This value can be set to any integer. By default is set to the current Environment.TickCount property</remarks>
        public int Id { get; set; }

        /// <summary>
        /// Gets the Packet Type
        /// </summary>
        public PacketType Type { get; internal set; }

        /// <summary>
        /// Gets the Packet body
        /// </summary>
        public string Body
        {
            get { return _body; }
            private set { _body = value; }
        }

        /// <summary>
        /// Gets the bytes composing this Packet as defined in RCON Protocol
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] buffer = new byte[Size + 4];
            Size.ToLittleEndian().CopyTo(buffer, sizeIndex);
            Id.ToLittleEndian().CopyTo(buffer, idIndex);
            Type.Value.ToLittleEndian().CopyTo(buffer, typeIndex);
            Encoding.UTF8.GetBytes(Body).CopyTo(buffer, bodyIndex);
            return buffer;
        }

        /// <summary>
        /// Create a new RconPacket from a byte array
        /// </summary>
        /// <param name="buffer">Input buffer</param>
        /// <returns>Parsed RconPacket</returns>
        /// <exception cref="System.ArgumentException">When buffer is null or its size is less than 14</exception>
        public static RconPacket FromBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 14)
            {
                throw new ArgumentException("Invalid packet");
            }
            RconPacket packet = new RconPacket()
            {
                Body = Encoding.UTF8.GetString(buffer, bodyIndex, buffer.ToInt32(sizeIndex) - 10),
                Id = buffer.ToInt32(idIndex)
            };
            return packet;
        }
    }
}
