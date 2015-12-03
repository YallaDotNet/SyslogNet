using System;
using System.IO;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// Message serializer extensions.
    /// </summary>
    public static class SyslogMessageSerializerExtensions
    {
        /// <summary>
        /// Serializes a message into a byte array.
        /// </summary>
        /// <param name="serializer">Serializer.</param>
        /// <param name="message">Message.</param>
        /// <returns>Byte array.</returns>
        /// <exception cref="ArgumentNullException">Missing serializer or message value.</exception>
        public static byte[] Serialize(this ISyslogMessageSerializer serializer, SyslogMessage message)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            byte[] datagramBytes;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(message, stream);

                stream.Position = 0;

                datagramBytes = new byte[stream.Length];
                stream.Read(datagramBytes, 0, (int)stream.Length);
            }
            return datagramBytes;
        }
    }
}
