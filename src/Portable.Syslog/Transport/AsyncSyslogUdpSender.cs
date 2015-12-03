using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous syslog UDP sender.
    /// </summary>
    public sealed class AsyncSyslogUdpSender : AsyncSyslogSenderBase
    {
        private readonly UdpSocketClient udpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogUdpSender"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        public AsyncSyslogUdpSender(string hostname, int port)
            : base(hostname, port)
        {
            udpClient = new UdpSocketClient();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The sender has been disposed.</exception>
        public override void Dispose()
        {
            udpClient.Dispose();
        }

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <returns>Asynchronous task.</returns>
        protected override async Task ConnectAsync(string hostname, int port)
        {
            await udpClient.ConnectAsync(hostname, port).ConfigureAwait(false);
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public override async Task DisconnectAsync()
        {
            await udpClient.DisconnectAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public override Task ReconnectAsync()
        {
            // UDP is connectionless
            return new TaskCompletionSource<object>().Task;
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
        protected override async Task WriteAsync(byte[] datagramBytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await udpClient.SendAsync(datagramBytes).ConfigureAwait(false);
        }
    }
}
