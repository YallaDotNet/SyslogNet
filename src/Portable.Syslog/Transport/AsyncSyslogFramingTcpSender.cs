using System;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous syslog Non-Transparent-Framing TCP sender.
    /// </summary>
    public sealed class AsyncSyslogFramingTcpSender : AsyncSyslogTcpSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogFramingTcpSender"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        public AsyncSyslogFramingTcpSender(string hostname, int port)
            : base(hostname, port, false)
        {
        }
    }
}
