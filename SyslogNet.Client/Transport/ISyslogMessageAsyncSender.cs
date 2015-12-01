using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyslogNet.Client.Serialization;
using System.Threading;

namespace SyslogNet.Client.Transport
{
	public interface ISyslogMessageAsyncSender : IDisposable
	{
        Task ConnectAsync();
        Task DisconnectAsync();
        Task ReconnectAsync();
        Task SendAsync(SyslogMessage message, ISyslogMessageSerializer serializer, CancellationToken cancellationToken = default(CancellationToken));
        Task SendAsync(IEnumerable<SyslogMessage> messages, ISyslogMessageSerializer serializer, CancellationToken cancellationToken = default(CancellationToken));
	}
}