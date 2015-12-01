using System;

namespace SyslogNet.Client.Transport
{
    class SyslogTransportException : Exception
    {
        public SyslogTransportException(string message)
            : base(message)
        {
        }
    }
}
