using System.Text;

namespace SyslogNet.Client.Serialization
{
	public abstract class SyslogMessageSerializerBase
	{
        private readonly Encoding encoding;

        protected SyslogMessageSerializerBase(Encoding encoding)
        {
            this.encoding = encoding;
        }
		
        protected static int CalculatePriorityValue(Facility facility, Severity severity)
		{
			return ((int)facility * 8) + (int)severity;
		}

        public Encoding Encoding
        {
            get { return encoding; }
        }
	}
}