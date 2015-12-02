using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous delegate sender.
    /// </summary>
    public class AsyncSyslogDelegateSender : IAsyncSyslogSender
    {
        private readonly ISyslogMessageSender innerSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogDelegateSender"/> class.
        /// </summary>
        /// <param name="innerSender">Inner sender.</param>
        public AsyncSyslogDelegateSender(ISyslogMessageSender innerSender)
        {
            if (innerSender == null)
                throw new ArgumentNullException("innerSender");

            this.innerSender = innerSender;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            innerSender.Dispose();
        }

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public Task ConnectAsync()
        {
            return RunAsync(innerSender.Connect);
        }

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public Task DisconnectAsync()
        {
            return RunAsync(innerSender.Disconnect);
        }

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        public Task ReconnectAsync()
        {
            return RunAsync(innerSender.Reconnect);
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        public Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            return RunAsync(() => innerSender.Send(message, serializer));
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
        public Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return SendAsync(message, serializer);
        }

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        public Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            return RunAsync(() => innerSender.Send(messages, serializer));
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
        public Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return SendAsync(messages, serializer);
        }

        private static async Task RunAsync(Action action)
        {
            await Task.Factory.StartNew(action).ConfigureAwait(false);
        }
    }
}
