using SyslogNet.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogNet.Client.Transport
{
	public interface ISyslogMessageAsyncSender : IDisposable
	{
        Task ConnectAsync();
        Task DisconnectAsync();
        Task ReconnectAsync();
        Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer);
        Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);
        Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer);
        Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken);
    }
}
