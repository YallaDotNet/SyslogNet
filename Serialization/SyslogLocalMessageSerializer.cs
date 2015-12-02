using System;
using System.Text;
using System.IO;

namespace SyslogNet.Client.Serialization
{
    public class SyslogLocalMessageSerializer : SyslogMessageSerializerBase<SyslogLocalMessageSerializer>, ISyslogMessageSerializer
    {
        static SyslogLocalMessageSerializer()
        {
        }

        // Default constructor: produce no BOM in local syslog messages
        [Obsolete("Use SyslogLocalMessageSerializer.Default instead.")]
        public SyslogLocalMessageSerializer()
            : this(false)
        {
        }

        // Optionally produce a BOM in local syslog messages by passing true here
        // (This can produce problems with some older syslog programs, so default is false)
        public SyslogLocalMessageSerializer(bool useBOM)
            : base(new UTF8Encoding(encoderShouldEmitUTF8Identifier: useBOM))
        {
        }

        public void Serialize(SyslogMessage message, Stream stream)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            if (stream == null)
                throw new ArgumentNullException("stream");

            // Local syslog serialization only cares about the Message string
            if (!String.IsNullOrWhiteSpace(message.Message))
            {
                byte[] streamBytes = Encoding.GetBytes(message.Message);
                stream.Write(streamBytes, 0, streamBytes.Length);
            }
        }
    }
}
