using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Base asynchronous syslog TCP sender.
    /// </summary>
    public abstract class AsyncSyslogTcpSenderBase : AsyncSyslogSenderBase
    {
        private readonly bool secure;
        private TcpSocketClient tcpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogTcpSenderBase"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="secure"><c>true</c> to use secure transport.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        protected AsyncSyslogTcpSenderBase(string hostname, int port, bool secure)
            : base(hostname, port)
        {
            this.secure = secure;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (tcpClient != null)
            {
                tcpClient.Dispose();
                tcpClient = null;
            }
        }

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <returns>Asynchronous task.</returns>
        protected override async Task ConnectAsync(string hostname, int port)
        {
            using (this)
            {
                tcpClient = new TcpSocketClient();
                await tcpClient.ConnectAsync(hostname, port, secure).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public override async Task DisconnectAsync()
        {
            await tcpClient.DisconnectAsync().ConfigureAwait(false);
            Dispose();
        }

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public override async Task ReconnectAsync()
        {
            await DisconnectAsync();
            await ConnectAsync();
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="ArgumentNullException">
        /// Missing <paramref name="message"/> or <paramref name="serializer"/> value.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        public override async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await base.SendAsync(message, serializer, cancellationToken);
            await TransportStream.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="ArgumentNullException">
        /// Missing <paramref name="messages"/> or <paramref name="serializer"/> value.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        public override async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await base.SendAsync(messages, serializer, cancellationToken);
            await TransportStream.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="stream">Stream.</param>
        protected override void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            base.Serialize(message, serializer, stream);
            stream.WriteByte(10); // LF
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        protected override async Task WriteAsync(byte[] bytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            if (TransportStream == null)
                throw new IOException("No transport stream exists");

            await TransportStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        }

        private Stream TransportStream
        {
            get
            {
                return tcpClient != null
                    ? tcpClient.WriteStream
                    : null;
            }
        }
    }
}
