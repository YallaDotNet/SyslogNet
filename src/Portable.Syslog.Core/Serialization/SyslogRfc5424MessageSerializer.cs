using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// RFC 5424 message serializer.
    /// </summary>
    public class SyslogRfc5424MessageSerializer : SyslogMessageSerializerBase<SyslogRfc5424MessageSerializer>, ISyslogMessageSerializer
    {
        /// <summary>
        /// NILVALUE.
        /// </summary>
        [Obsolete]
        public const string NilValue = "-";

        /// <summary>
        /// SD-NAME disallowed characters.
        /// </summary>
        [Obsolete]
        public static readonly HashSet<char> sdNameDisallowedChars = new HashSet<char>() { ' ', '=', ']', '"' };

        static SyslogRfc5424MessageSerializer()
        {
        }

        private readonly char[] asciiCharsBuffer = new char[255];

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogRfc5424MessageSerializer"/> class.
        /// </summary>
        [Obsolete("Use SyslogRfc5424MessageSerializer.Default instead.")]
        public SyslogRfc5424MessageSerializer()
            : this(new ASCIIEncoding())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogRfc5424MessageSerializer"/> class.
        /// </summary>
        /// <param name="encoding">Encoding.</param>
        /// <exception cref="ArgumentNullException">Missing encoding value.</exception>
        public SyslogRfc5424MessageSerializer(Encoding encoding)
            : base(encoding)
        {
        }

        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stream">Stream.</param>
        /// <exception cref="ArgumentNullException">Missing message or stream value.</exception>
        public void Serialize(SyslogMessage message, Stream stream)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            if (stream == null)
                throw new ArgumentNullException("stream");

            var priorityValue = CalculatePriorityValue(message);

            // Note: The .Net ISO 8601 "o" format string uses 7 decimal places for fractional second. Syslog spec only allows 6, hence the custom format string
            var timestamp = message.DateTimeOffset.HasValue
                ? message.DateTimeOffset.Value.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK")
                : null;

            var messageBuilder = new StringBuilder();
            messageBuilder.Append("<").Append(priorityValue).Append(">");
            messageBuilder.Append(message.Version);
#pragma warning disable 612
            messageBuilder.Append(" ").Append(timestamp.FormatSyslogField(NilValue));
            messageBuilder.Append(" ").Append(message.HostName.FormatSyslogAsciiField(NilValue, 255, asciiCharsBuffer));
            messageBuilder.Append(" ").Append(message.AppName.FormatSyslogAsciiField(NilValue, 48, asciiCharsBuffer));
            messageBuilder.Append(" ").Append(message.ProcId.FormatSyslogAsciiField(NilValue, 128, asciiCharsBuffer));
            messageBuilder.Append(" ").Append(message.MsgId.FormatSyslogAsciiField(NilValue, 32, asciiCharsBuffer));
#pragma warning restore 612

            writeStream(stream, Encoding, messageBuilder.ToString());

            // Structured data
            foreach (StructuredDataElement sdElement in message.StructuredDataElements)
            {
                messageBuilder.Clear()
                    .Append(" ")
                    .Append("[")
                    .Append(sdElement.SdId.FormatSyslogSdnameField(asciiCharsBuffer));

                writeStream(stream, Encoding, messageBuilder.ToString());

                foreach (System.Collections.Generic.KeyValuePair<string, string> sdParam in sdElement.Parameters)
                {
                    messageBuilder.Clear()
                        .Append(" ")
                        .Append(sdParam.Key.FormatSyslogSdnameField(asciiCharsBuffer))
                        .Append("=")
                        .Append("\"")
                        .Append(
                            sdParam.Value != null ?
                                sdParam.Value
                                    .Replace("\\", "\\\\")
                                    .Replace("\"", "\\\"")
                                    .Replace("]", "\\]")
                                :
                                String.Empty
                        )
                        .Append("\"");

                    writeStream(stream, Encoding, messageBuilder.ToString());
                }

                // ]
                stream.WriteByte(93);
            }

            if (!String.IsNullOrWhiteSpace(message.Message))
            {
                // Space
                stream.WriteByte(32);

                stream.Write(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);
                writeStream(stream, Encoding.UTF8, message.Message);
            }
        }

        private void writeStream(Stream stream, Encoding encoding, String data)
        {
            byte[] streamBytes = encoding.GetBytes(data);
            stream.Write(streamBytes, 0, streamBytes.Length);
        }
    }

    internal static class StringExtensions
    {
        public static string IfNotNullOrWhitespace(this string s, Func<string, string> action)
        {
            return String.IsNullOrWhiteSpace(s) ? s : action(s);
        }

        public static string FormatSyslogField(this string s, string replacementValue, int? maxLength = null)
        {
            return String.IsNullOrWhiteSpace(s)
                ? replacementValue
                : maxLength.HasValue ? EnsureMaxLength(s, maxLength.Value) : s;
        }

        public static string EnsureMaxLength(this string s, int maxLength)
        {
            return String.IsNullOrWhiteSpace(s)
                ? s
                : s.Length > maxLength ? s.Substring(0, maxLength) : s;
        }

        public static string FormatSyslogAsciiField(this string s, string replacementValue, int maxLength, char[] charBuffer, Boolean sdName = false)
        {
            s = FormatSyslogField(s, replacementValue, maxLength);

            int bufferIndex = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 33 && c <= 126)
                {
#pragma warning disable 612
                    if (!sdName || !SyslogRfc5424MessageSerializer.sdNameDisallowedChars.Contains(c))
#pragma warning restore 612
                    {
                        charBuffer[bufferIndex++] = c;
                    }
                }
            }

            return new string(charBuffer, 0, bufferIndex);
        }

        public static string FormatSyslogSdnameField(this string s, char[] charBuffer)
        {
#pragma warning disable 612
            return FormatSyslogAsciiField(s, SyslogRfc5424MessageSerializer.NilValue, 32, charBuffer, true);
#pragma warning restore 612
        }
    }
}
