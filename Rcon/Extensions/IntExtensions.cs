﻿using System;


namespace Rcon.Extensions
{
    /// <summary>
    /// Extensions for handling LittleEndian conversion
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Converts an Int32 value to a Little Endian 4 bytes array 
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Array containing the Little Endian bytes</returns>
        public static byte[] ToLittleEndian(this int value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
        /// <summary>
        /// Converts a 4 bytes Little Endian array to an Int32
        /// </summary>
        /// <param name="buffer">Origin array</param>
        /// <param name="startIndex">Index of the 4 bytes value inside origin array</param>
        /// <returns>The Int32 value</returns>
        public static int ToInt32(this byte[] buffer, int startIndex)
        {
            if (startIndex + 4 > buffer.Length)
                throw new ArgumentException("Error: not enough bytes to perform the conversion");

            var bytes = new byte[4];
            Array.Copy(buffer, startIndex, bytes, 0, 4);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
