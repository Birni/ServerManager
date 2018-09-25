
namespace Rcon
{
    /// <summary>
    /// RCON Packet type
    /// </summary>
    public class PacketType
    {
        /// <summary>
        /// Gets the value according to RCON protocol
        /// </summary>
        public int Value { get; internal set; }

        /// <summary>
        /// Gets the original RCON Packet Type name as defined in the protocol
        /// </summary>
        public string ProtocolName { get; internal set; }

        /// <summary>
        /// Auth Packet Type
        /// </summary>
        public static PacketType Auth = new PacketType() { ProtocolName = "SERVERDATA_AUTH", Value = 3 };

        /// <summary>
        /// Auth Response Packet Type
        /// </summary>
        public static PacketType AuthResponse = new PacketType() { ProtocolName = "SERVERDATA_AUTH_RESPONSE", Value = 2 };

        /// <summary>
        /// Exec Command Packet Type
        /// </summary>
        public static PacketType ExecCommand = new PacketType() { ProtocolName = "SERVERDATA_EXECCOMMAND", Value = 2 };

        /// <summary>
        /// Response Value Packet Type
        /// </summary>
        public static PacketType ResponseValue = new PacketType() { ProtocolName = "SERVERDATA_RESPONSE_VALUE", Value = 0 };
    }
}
