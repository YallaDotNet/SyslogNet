using System;
using System.IO;

namespace SyslogNet.Client.Serialization
{
    public static class SyslogMessageSerializerExtensions
    {
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
