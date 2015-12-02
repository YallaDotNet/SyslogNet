using System;

namespace SyslogNet.Client.Transport
{
    public class SyslogTransportException : Exception
    {
        public SyslogTransportException(string message)
            : base(message)
        {
        }
    }
}
