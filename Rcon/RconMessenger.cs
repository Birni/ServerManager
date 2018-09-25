using System;
using System.Threading.Tasks;


namespace Rcon
{
    /// <summary>
    /// Rcon protocol messages handler
    /// </summary>
    public class RconMessenger
    {
        private INetworkSocket _socket;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="socket">Socket interface</param>
        /// <remarks>Since this is a Portable Class Library, there's not a concrete implementation of the Socket class</remarks>
        public RconMessenger(INetworkSocket socket)
        {
            if (socket == null)
                throw new NullReferenceException("Socket parameter must be an instance of a class implementing INetworkSocket inteface");
            _socket = socket;
        }

        /// <summary>
        /// Connect the socket to the remote endpoint
        /// </summary>
        /// <param name="host">remote host address</param>
        /// <param name="port">remote host port</param>
        /// <returns>True if the connection was successfully; False if the connection is already estabilished</returns>
        public async Task<bool> ConnectAsync(string host, int port)
        {
            if (!_socket.IsConnected)
                return await _socket.ConnectAsync(host, port);
            else
                return false;
        }

        /// <summary>
        /// Send the proper authentication packet and parse the response
        /// </summary>
        /// <param name="password">Current server password</param>
        /// <returns>True if the connection has been authenticated; False elsewhere</returns>
        /// <remarks>This method must be called prior to sending any other command</remarks>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="password"/> parameter is null or empty</exception>
        public async Task<bool> AuthenticateAsync(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password parameter must be a non null non empty string");

            var authPacket = new RconPacket(PacketType.Auth, password);
            var response = await _socket.SendDataAndReadResponseAsync(authPacket.GetBytes());
            var responsePacket = RconPacket.FromBytes(response);
            return responsePacket.Id != -1;
        }

        /// <summary>
        /// Send a command encapsulated into an Rcon message packet and get the response
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <returns>The response to this command</returns>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="command"/> parameter is null or empty</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the connection is not properly opened and authenticated</exception>
        public async Task<string> ExecuteCommandAsync(string command)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentException("command parameter must be a non null non empty string");

            if (!_socket.IsConnected)
                throw new InvalidOperationException("You must authenticate the connection before sending any command to the server");

            var commandPacket = new RconPacket(PacketType.ExecCommand, command);
            var response = await _socket.SendDataAndReadResponseAsync(commandPacket.GetBytes());
            var responsePacket = RconPacket.FromBytes(response);
            return responsePacket.Body;
        }

        /// <summary>
        /// Close the remote connection
        /// </summary>
        public void CloseConnection()
        {
            _socket.CloseConnection();
        }
    }
}
