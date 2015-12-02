using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    public abstract class AsyncSyslogSenderBase : IAsyncSyslogSender
    {
        protected readonly string hostname;
        protected readonly int port;

        protected AsyncSyslogSenderBase(string hostname, int port)
        {
            if (hostname == null)
                throw new ArgumentNullException("hostname");

            this.hostname = hostname;
            this.port = port;
        }

        public abstract void Dispose();

        public async Task ConnectAsync()
        {
            await ConnectAsync(hostname, port);
        }

        public abstract Task DisconnectAsync();

        public abstract Task ReconnectAsync();

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            await SendAsync(message, serializer, CancellationToken.None);
        }

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            await SendAsync(messages, serializer, CancellationToken.None);
        }

        public virtual async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await DoSendAsync(message, serializer, cancellationToken);
        }

        public virtual async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");

            foreach (SyslogMessage message in messages)
                await DoSendAsync(message, serializer, cancellationToken);
        }

        protected abstract Task ConnectAsync(string hostname, int port);

        protected virtual void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            serializer.Serialize(message, stream);
        }

        protected abstract Task WriteAsync(byte[] datagramBytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);

        private async Task DoSendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            var bytes = Serialize(message, serializer);
            await WriteAsync(bytes, serializer, cancellationToken);
        }

        private byte[] Serialize(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (serializer == null)
                throw new ArgumentNullException("serializer");

            using (var memoryStream = new MemoryStream())
            {
                Serialize(message, serializer, memoryStream);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
