using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous sender.
    /// </summary>
    public interface IAsyncSyslogSender : IDisposable
    {
        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        Task ConnectAsync();

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        Task DisconnectAsync();

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        Task ReconnectAsync();

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer);

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <returns>Asynchronous task.</returns>
        Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer);

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Asynchronous task.</returns>
        Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);
    }
}
