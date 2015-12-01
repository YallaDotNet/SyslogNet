using SyslogNet.Client.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    public abstract class SyslogAsyncSenderBase
    {
        protected readonly string hostname;
        protected readonly int port;

        protected SyslogAsyncSenderBase(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

        public async Task ConnectAsync()
        {
            await ConnectAsync(hostname, port);
        }

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            await SendAsync(message, serializer, CancellationToken.None);
        }

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            await SendAsync(messages, serializer, CancellationToken.None);
        }

        protected byte[] Serialize(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            using (var memoryStream = new MemoryStream())
            {
                Serialize(message, serializer, memoryStream);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }

        protected virtual void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            serializer.Serialize(message, stream);
        }

        public abstract Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);

        public abstract Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);

        protected abstract Task ConnectAsync(string hostname, int port);
    }
}
