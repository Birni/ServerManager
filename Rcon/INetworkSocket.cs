using System.Threading.Tasks;


namespace Rcon
{
    /// <summary>
    /// Shared interface for the Network Socket
    /// </summary>
    /// <remarks>Since this is a Portable Class Library (PCL), concrete implementations of this inteface will be defined in specific assemblies targeting the different platforms</remarks>
    public interface INetworkSocket
    {
        /// <summary>
        /// Connect the socket to the remote endpoint
        /// </summary>
        /// <param name="host">remote host address</param>
        /// <param name="port">remote host port</param>
        /// <returns>True if the connection was successfully; False if the connection is already estabilished</returns>
        Task<bool> ConnectAsync(string host, int port);
        /// <summary>
        /// Close the connection to the remote endpoint
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// Send a Rcon command to the remote server
        /// </summary>
        /// <param name="data">Rcon command data</param>
        /// <returns>The response to the command sent</returns>
        Task<byte[]> SendDataAndReadResponseAsync(byte[] data);
        /// <summary>
        /// Gets whether the connection is opened or not
        /// </summary>
        bool IsConnected { get; }
    }
}
