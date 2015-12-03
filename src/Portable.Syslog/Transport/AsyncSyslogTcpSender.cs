using SyslogNet.Client.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous syslog Octet-Counting TCP sender.
    /// </summary>
    public class AsyncSyslogTcpSender : AsyncSyslogTcpSenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogTcpSender"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        public AsyncSyslogTcpSender(string hostname, int port)
            : this(hostname, port, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogTcpSender"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="secure"><c>true</c> to use secure transport.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        protected AsyncSyslogTcpSender(string hostname, int port, bool secure)
            : base(hostname, port, secure)
        {
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="datagramBytes">Byte array.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        protected override Task WriteAsync(byte[] datagramBytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            var messageLength = datagramBytes.Length.ToString();
            var messageLengthByteCount = serializer.Encoding.GetByteCount(messageLength);
            var bytes = new byte[messageLengthByteCount + datagramBytes.Length + 1];
            serializer.Encoding.GetBytes(messageLength, 0, messageLength.Length, bytes, 0);
            bytes[messageLengthByteCount] = 32; // Space
            datagramBytes.CopyTo(bytes, messageLengthByteCount + 1);
            return base.WriteAsync(bytes, serializer, cancellationToken);
        }
    }
}
