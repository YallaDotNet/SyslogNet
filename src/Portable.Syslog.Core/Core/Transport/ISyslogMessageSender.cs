using System;
using System.Collections.Generic;
using SyslogNet.Client.Serialization;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Message sender.
    /// </summary>
	public interface ISyslogMessageSender : IDisposable
	{
        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from the remote host.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Reconnects to the remote host.
        /// </summary>
		void Reconnect();

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="serializer">Serializer.</param>
        void Send(SyslogMessage message, ISyslogMessageSerializer serializer);

        /// <summary>
        /// Sends a collection of messages.
        /// </summary>
        /// <param name="messages">Messages.</param>
        /// <param name="serializer">Serializer.</param>
        void Send(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer);
	}
}