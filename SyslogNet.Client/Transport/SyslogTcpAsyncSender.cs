using Sockets.Plugin;
using SyslogNet.Client.Serialization;
using SyslogNet.Client.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
	public enum MessageTransfer {
		OctetCounting		= 0,
		NonTransparentFraming	= 1
	}

	public sealed class SyslogTcpAsyncSender : ISyslogMessageAsyncSender, IDisposable
	{
        private const byte Delimiter = 32; // Space
        private const byte Trailer = 10; // LF

        private readonly String hostname;
        private readonly int port;
        private readonly MessageTransfer messageTransfer;
        private readonly bool secure;

		private TcpSocketClient tcpClient = null;

        public SyslogTcpAsyncSender(string hostname, int port, MessageTransfer messageTransfer = MessageTransfer.OctetCounting, bool secure = false)
		{
			this.hostname = hostname;
			this.port = port;
            this.secure = secure;

            if (!messageTransfer.Equals(MessageTransfer.OctetCounting) && secure)
            {
                throw new SyslogTransportException("Non-Transparent-Framing can not be used with TLS transport");
            }
            this.messageTransfer = messageTransfer;
		}

		public async Task ConnectAsync()
		{
			using (this)
			{
				tcpClient = new TcpSocketClient();
                await tcpClient.ConnectAsync(hostname, port, secure).ConfigureAwait(false);
			}
		}

        public async Task DisconnectAsync()
        {
            await tcpClient.DisconnectAsync().ConfigureAwait(false);
            Dispose();
        }

        public async Task ReconnectAsync()
        {
            await DisconnectAsync();
            await ConnectAsync();
        }

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            await SendAsync(message, serializer, true, CancellationToken.None);
        }

        public async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
		{
			await SendAsync(message, serializer, true, cancellationToken);
		}

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer)
        {
            await SendAsync(messages, serializer, CancellationToken.None);
        }

        public async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
		{
			foreach (SyslogMessage message in messages)
			{
                await SendAsync(message, serializer, false, cancellationToken);
			}

            await TransportStream.FlushAsync(cancellationToken);
		}

		public void Dispose()
		{
			if (tcpClient != null)
			{
				tcpClient.Dispose();
				tcpClient = null;
			}
		}

        private async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, bool flush, CancellationToken cancellationToken)
        {
            if (TransportStream == null)
            {
                throw new IOException("No transport stream exists");
            }

            var datagramBytes = Serialize(message, serializer);

            if (messageTransfer.Equals(MessageTransfer.OctetCounting))
            {
                var messageLengthString = datagramBytes.Length.ToString();
                var messageLengthBytes = Serialize(messageLengthString);
                await TransportStream.WriteAsync(messageLengthBytes, 0, messageLengthBytes.Length, cancellationToken).ConfigureAwait(false);
            }

            await TransportStream.WriteAsync(datagramBytes, 0, datagramBytes.Length, cancellationToken);

            if (flush)
                await TransportStream.FlushAsync(cancellationToken);
        }

        private static byte[] Serialize(SyslogMessage message, ISyslogMessageSerializer serializer)
        {
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(message, memoryStream);
                memoryStream.WriteByte(Trailer); // LF
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }

        private static byte[] Serialize(string s)
        {
            var buffer = new byte[Encoding.ASCII.GetByteCount(s) + 1];
            Encoding.ASCII.GetBytes(s, 0, s.Length, buffer, 0);
            buffer[buffer.Length - 1] = Delimiter; // Space
            return buffer;
        }

        private Stream TransportStream
        {
            get
            {
                return tcpClient != null
                    ? tcpClient.WriteStream
                    : null;
            }
        }
    }
}
