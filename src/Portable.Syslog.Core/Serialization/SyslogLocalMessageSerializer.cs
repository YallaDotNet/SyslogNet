using System;
using System.Text;
using System.IO;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// Local syslog message serializer.
    /// </summary>
    public class SyslogLocalMessageSerializer : SyslogMessageSerializerBase<SyslogLocalMessageSerializer>, ISyslogMessageSerializer
    {
        static SyslogLocalMessageSerializer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogLocalMessageSerializer"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>This constructor is obsolete and will be removed in a future version.</strong>
        /// Use <see cref="SyslogMessageSerializerBase{SyslogLocalMessageSerializer}.Default"/> instead.
        /// </para>
        /// <para>
        /// The default behaviour is to produce no BOM in local syslog messages.
        /// </para>
        /// </remarks>
        [Obsolete("Use SyslogLocalMessageSerializer.Default instead.")]
        public SyslogLocalMessageSerializer()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogLocalMessageSerializer"/> class.
        /// </summary>
        /// <param name="useBOM">
        /// <c>true</c> if the serializer should produce a BOM in local syslog messages.
        /// </param>
        public SyslogLocalMessageSerializer(bool useBOM)
            : base(new UTF8Encoding(encoderShouldEmitUTF8Identifier: useBOM))
        {
        }

        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stream">Stream.</param>
        /// <exception cref="ArgumentNullException">
        /// Missing <paramref name="message"/> or <paramref name="stream"/> value.
        /// </exception>
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
