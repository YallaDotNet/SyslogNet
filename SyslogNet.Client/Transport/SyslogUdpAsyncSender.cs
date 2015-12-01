using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    public sealed class SyslogUdpAsyncSender : SyslogAsyncSenderBase, ISyslogMessageAsyncSender, IDisposable
    {
        private readonly UdpSocketClient udpClient;

        public SyslogUdpAsyncSender(string hostname, int port)
            : base(hostname, port)
        {
            this.udpClient = new UdpSocketClient();
        }

        protected override async Task ConnectAsync(string hostname, int port)
        {
            await udpClient.ConnectAsync(hostname, port).ConfigureAwait(false);
        }

        public async Task DisconnectAsync()
        {
            await udpClient.DisconnectAsync().ConfigureAwait(false);
        }

        public Task ReconnectAsync()
        {
            // UDP is connectionless
            return new TaskCompletionSource<object>().Task;
        }

        public override async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            var datagramBytes = Serialize(message, serializer);
            await udpClient.SendAsync(datagramBytes).ConfigureAwait(false);
        }

        public override async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            foreach (SyslogMessage message in messages)
            {
                await SendAsync(message, serializer);
            }
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }

        private static byte[] Serialize(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(message, memoryStream);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
