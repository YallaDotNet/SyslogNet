using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Base asynchronous sender.
    /// </summary>
    public abstract class AsyncSyslogSenderBase : IAsyncSyslogSender
    {
        private readonly string hostname;
        private readonly int port;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogSenderBase"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <exception cref="ArgumentNullException">missing hostname value.</exception>
        protected AsyncSyslogSenderBase(string hostname, int port)
        {
            if (hostname == null)
                throw new ArgumentNullException("hostname");

            this.hostname = hostname;
            this.port = port;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public async Task ConnectAsync()
        {
            await ConnectAsync(hostname, port);
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public abstract Task DisconnectAsync();

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public abstract Task ReconnectAsync();

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            await SendAsync(message, serializer, CancellationToken.None);
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        public virtual async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await DoSendAsync(message, serializer, cancellationToken);
        }

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            await SendAsync(messages, serializer, CancellationToken.None);
        }

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        /// <exception cref="OperationCanceledException">
        /// The token has had cancellation requested.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The associated <see cref="CancellationTokenSource"/> has been disposed.
        /// </exception>
        public virtual async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");

            foreach (SyslogMessage message in messages)
                await DoSendAsync(message, serializer, cancellationToken);
        }

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <returns>Asynchronous task.</returns>
        protected abstract Task ConnectAsync(string hostname, int port);

        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="stream">Stream.</param>
        protected virtual void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            serializer.Serialize(message, stream);
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
        protected abstract Task WriteAsync(byte[] datagramBytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);

        private async Task DoSendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            var bytes = Serialize(message, serializer);
            await WriteAsync(bytes, serializer, cancellationToken);
        }

        private byte[] Serialize(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (serializer == null)
                throw new ArgumentNullException("serializer");

            using (var memoryStream = new MemoryStream())
            {
                Serialize(message, serializer, memoryStream);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
