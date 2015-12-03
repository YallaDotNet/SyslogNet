using System;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Asynchronous syslog secure TCP sender.
    /// </summary>
    public sealed class AsyncSyslogSecureTcpSender : AsyncSyslogTcpSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSyslogSecureTcpSender"/> class.
        /// </summary>
        /// <param name="hostname">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <exception cref="ArgumentNullException">Missing <paramref name="hostname"/> value.</exception>
        public AsyncSyslogSecureTcpSender(string hostname, int port)
            : base(hostname, port, true)
        {
        }
    }
}
