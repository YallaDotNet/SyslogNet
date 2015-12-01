using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
    public sealed class SyslogUdpAsyncSender : ISyslogMessageAsyncSender, IDisposable
    {
        private readonly string hostname;
        private readonly int port;
        private readonly UdpSocketClient udpClient;

        public SyslogUdpAsyncSender(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
            this.udpClient = new UdpSocketClient();
        }

        public async Task ConnectAsync()
        {
            await udpClient.ConnectAsync(hostname, port);
        }

        public async Task DisconnectAsync()
        {
            await udpClient.DisconnectAsync();
        }

        public Task ReconnectAsync()
        {
            // UDP is connectionless
            return new TaskCompletionSource<object>().Task;
        }

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            var datagramBytes = Serialize(message, serializer);
            await udpClient.SendAsync(datagramBytes);
        }

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            // UdpSocketClient is cancellationless
            await SendAsync(message, serializer);
        }

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            foreach (SyslogMessage message in messages)
            {
                await SendAsync(message, serializer);
            }
        }

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            // UdpSocketClient is cancellationless
            await SendAsync(messages, serializer);
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
