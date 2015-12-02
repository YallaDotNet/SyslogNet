using System;
using System.Text;
using System.IO;

namespace SyslogNet.Client.Serialization
{
    public class SyslogLocalMessageSerializer : SyslogMessageSerializerBase, ISyslogMessageSerializer
    {
        private static readonly Lazy<SyslogLocalMessageSerializer> _lazy;

        static SyslogLocalMessageSerializer()
        {
#pragma warning disable 618
            _lazy = new Lazy<SyslogLocalMessageSerializer>(() => new SyslogLocalMessageSerializer());
#pragma warning restore 618
        }

        public static SyslogLocalMessageSerializer Default
        {
            get { return _lazy.Value; }
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
            // Local syslog serialization only cares about the Message string
            if (!String.IsNullOrWhiteSpace(message.Message))
            {
                byte[] streamBytes = Encoding.GetBytes(message.Message);
                stream.Write(streamBytes, 0, streamBytes.Length);
            }
        }
    }
}
