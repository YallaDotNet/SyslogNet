using System;
using System.IO;
using System.Text;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// RFC 3164 message serializer.
    /// </summary>
    public class SyslogRfc3164MessageSerializer : SyslogMessageSerializerBase<SyslogRfc3164MessageSerializer>, ISyslogMessageSerializer
    {
        static SyslogRfc3164MessageSerializer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogRfc3164MessageSerializer"/> class.
        /// </summary>
        [Obsolete("Use SyslogRfc3164MessageSerializer.Default instead.")]
        public SyslogRfc3164MessageSerializer()
            : this(new ASCIIEncoding())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogRfc3164MessageSerializer"/> class.
        /// </summary>
        /// <param name="encoding">Encoding.</param>
        /// <exception cref="ArgumentNullException">missing encoding value.</exception>
        public SyslogRfc3164MessageSerializer(Encoding encoding)
            : base(encoding)
        {
        }

        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stream">Stream.</param>
        /// <exception cref="ArgumentNullException">missing message or stream value.</exception>
        public void Serialize(SyslogMessage message, Stream stream)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            if (stream == null)
                throw new ArgumentNullException("stream");

            var priorityValue = CalculatePriorityValue(message);

            string timestamp = null;
            if (message.DateTimeOffset.HasValue)
            {
                var dt = message.DateTimeOffset.Value;
                var day = dt.Day < 10 ? " " + dt.Day : dt.Day.ToString(); // Yes, this is stupid but it's in the spec
                timestamp = String.Concat(dt.ToString("MMM "), day, dt.ToString(" HH:mm:ss"));
            }

            var headerBuilder = new StringBuilder();
            headerBuilder.Append("<").Append(priorityValue).Append(">");
            headerBuilder.Append(timestamp).Append(" ");
            headerBuilder.Append(message.HostName).Append(" ");
            headerBuilder.Append(message.AppName.IfNotNullOrWhitespace(x => x.EnsureMaxLength(32) + ":"));
            headerBuilder.Append(message.Message ?? "");

            byte[] asciiBytes = Encoding.GetBytes(headerBuilder.ToString());
            stream.Write(asciiBytes, 0, asciiBytes.Length);
        }
    }
}
