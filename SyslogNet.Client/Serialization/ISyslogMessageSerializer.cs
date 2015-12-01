using System.IO;
using System.Text;

namespace SyslogNet.Client.Serialization
{
	public interface ISyslogMessageSerializer
	{
		void Serialize(SyslogMessage message, Stream stream);
        Encoding Encoding { get; }
    }
}