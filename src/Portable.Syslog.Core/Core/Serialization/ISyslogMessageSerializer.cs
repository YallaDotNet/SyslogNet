using System.IO;
using System.Text;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// syslog message serializer.
    /// </summary>
    public interface ISyslogMessageSerializer
    {
        /// <summary>
        /// Serializes a message to a stream.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stream">Stream.</param>
        void Serialize(SyslogMessage message, Stream stream);

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <value>The encoding used for serialization.</value>
        Encoding Encoding { get; }
    }
}
