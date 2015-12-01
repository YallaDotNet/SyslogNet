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
    public enum MessageTransfer
    {
        OctetCounting = 0,
        NonTransparentFraming = 1
    }

    public abstract class SyslogTcpAsyncSenderBase : SyslogAsyncSenderBase, ISyslogMessageAsyncSender, IDisposable
    {
        private const byte Delimiter = 32; // Space
        private const byte Trailer = 10; // LF

        private readonly bool secure;
        private readonly MessageTransfer messageTransfer;

        private TcpSocketClient tcpClient = null;

        protected SyslogTcpAsyncSenderBase(string hostname, int port, bool secure, MessageTransfer messageTransfer)
            : base(hostname, port)
        {
            this.secure = secure;
            this.messageTransfer = messageTransfer;
        }

        protected override async Task ConnectAsync(string hostname, int port)
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

        public override async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await SendAsync(message, serializer, true, cancellationToken);
        }

        public override async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
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

        protected override void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            base.Serialize(message, serializer, stream);
            stream.WriteByte(Trailer); // LF
        }

        private async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, bool flush, CancellationToken cancellationToken)
        {
            if (TransportStream == null)
            {
                throw new IOException("No transport stream exists");
            }

            var bytes = Serialize(message, serializer);

            if (messageTransfer.Equals(MessageTransfer.OctetCounting))
            {
                bytes = PrependLength(bytes);
            }

            await TransportStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);

            if (flush)
                await TransportStream.FlushAsync(cancellationToken);
        }

        private static byte[] PrependLength(byte[] datagramBytes)
        {
            var messageLength = datagramBytes.Length.ToString();
            var messageLengthByteCount = Encoding.ASCII.GetByteCount(messageLength);
            var bytes = new byte[messageLengthByteCount + datagramBytes.Length + 1];
            Encoding.ASCII.GetBytes(messageLength, 0, messageLength.Length, bytes, 0);
            bytes[messageLengthByteCount] = Delimiter; // Space
            datagramBytes.CopyTo(bytes, messageLengthByteCount + 1);
            return bytes;
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

    public sealed class SyslogTcpAsyncSender : SyslogTcpAsyncSenderBase
    {
        public SyslogTcpAsyncSender(string hostname, int port, MessageTransfer messageTransfer = MessageTransfer.OctetCounting)
            : base(hostname, port, false, messageTransfer)
        {
        }
    }

    public sealed class SyslogSecureTcpAsyncSender : SyslogTcpAsyncSenderBase
    {
        public SyslogSecureTcpAsyncSender(string hostname, int port)
            : base(hostname, port, true, MessageTransfer.NonTransparentFraming)
        {
        }
    }
}
