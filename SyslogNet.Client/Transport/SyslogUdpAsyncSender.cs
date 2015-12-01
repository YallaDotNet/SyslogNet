using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using System;
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

        protected override async Task DoSendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            var datagramBytes = Serialize(message, serializer);
            await udpClient.SendAsync(datagramBytes).ConfigureAwait(false);
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }
    }
}
