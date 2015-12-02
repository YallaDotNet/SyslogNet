using Sockets.Plugin;
using SyslogNet.Client.Serialization;
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

    public abstract class AsyncSyslogTcpSenderBase : AsyncSyslogSenderBase
    {
        private const byte Delimiter = 32; // Space
        private const byte Trailer = 10; // LF

        private readonly bool secure;
        private readonly MessageTransfer messageTransfer;

        private TcpSocketClient tcpClient = null;

        protected AsyncSyslogTcpSenderBase(string hostname, int port, bool secure, MessageTransfer messageTransfer)
            : base(hostname, port)
        {
            this.secure = secure;
            this.messageTransfer = messageTransfer;
        }

        public override void Dispose()
        {
            if (tcpClient != null)
            {
                tcpClient.Dispose();
                tcpClient = null;
            }
        }

        protected override async Task ConnectAsync(string hostname, int port)
        {
            using (this)
            {
                tcpClient = new TcpSocketClient();
                await tcpClient.ConnectAsync(hostname, port, secure).ConfigureAwait(false);
            }
        }

        public override async Task DisconnectAsync()
        {
            await tcpClient.DisconnectAsync().ConfigureAwait(false);
            Dispose();
        }

        public override async Task ReconnectAsync()
        {
            await DisconnectAsync();
            await ConnectAsync();
        }

        public override async Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await base.SendAsync(message, serializer, cancellationToken);
            await TransportStream.FlushAsync(cancellationToken);
        }

        public override async Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            await base.SendAsync(messages, serializer, cancellationToken);
            await TransportStream.FlushAsync(cancellationToken);
        }

        protected override void Serialize(SyslogMessage message, ISyslogMessageSerializer serializer, Stream stream)
        {
            base.Serialize(message, serializer, stream);
            stream.WriteByte(Trailer); // LF
        }

        protected override async Task WriteAsync(byte[] bytes, ISyslogMessageSerializer serializer, CancellationToken cancellationToken)
        {
            if (TransportStream == null)
            {
                throw new IOException("No transport stream exists");
            }

            if (messageTransfer.Equals(MessageTransfer.OctetCounting))
            {
                bytes = PrependLength(serializer, bytes);
            }

            await TransportStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        }

        private static byte[] PrependLength(ISyslogMessageSerializer serializer, byte[] datagramBytes)
        {
            var messageLength = datagramBytes.Length.ToString();
            var messageLengthByteCount = serializer.Encoding.GetByteCount(messageLength);
            var bytes = new byte[messageLengthByteCount + datagramBytes.Length + 1];
            serializer.Encoding.GetBytes(messageLength, 0, messageLength.Length, bytes, 0);
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

    public sealed class AsyncSyslogTcpSender : AsyncSyslogTcpSenderBase
    {
        public AsyncSyslogTcpSender(string hostname, int port, MessageTransfer messageTransfer = MessageTransfer.OctetCounting)
            : base(hostname, port, false, messageTransfer)
        {
        }
    }

    public sealed class AsyncSyslogSecureTcpSender : AsyncSyslogTcpSenderBase
    {
        public AsyncSyslogSecureTcpSender(string hostname, int port)
            : base(hostname, port, true, MessageTransfer.NonTransparentFraming)
        {
        }
    }
}
