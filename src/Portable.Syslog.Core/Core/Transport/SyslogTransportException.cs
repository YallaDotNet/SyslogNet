using System;

namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Transport exception.
    /// </summary>
    public class SyslogTransportException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogTransportException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SyslogTransportException(string message)
            : base(message)
        {
        }
    }
}
