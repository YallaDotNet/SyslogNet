using System;
using System.Collections.Generic;

namespace SyslogNet.Client
{
    /// <summary>
    /// Message.
    /// </summary>
    public class SyslogMessage
    {
        private readonly Facility facility;
        private readonly Severity severity;
        private readonly string hostName;
        private readonly string appName;
        private readonly string procId;
        private readonly string msgId;
        private readonly string message;
        private readonly IEnumerable<StructuredDataElement> structuredDataElements;
        private readonly DateTimeOffset? dateTimeOffset;

        /// <summary>
        /// Default message facility.
        /// </summary>
        [Obsolete]
        public static Facility DefaultFacility = Facility.UserLevelMessages;

        /// <summary>
        /// Default message severity.
        /// </summary>
        [Obsolete]
        public static Severity DefaultSeverity = Severity.Informational;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessage"/> class
        /// for sending a local syslog message with the default facility.
        /// </summary>
        /// <param name="severity">Message severity.</param>
        /// <param name="appName">Application name.</param>
        /// <param name="message">Message text.</param>
        public SyslogMessage(
            Severity severity,
            string appName,
            string message)
#pragma warning disable 612
            : this(DefaultFacility, severity, appName, message)
#pragma warning restore 612
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessage"/> class
        /// for sending a local syslog message.
        /// </summary>
        /// <param name="facility">Message facility.</param>
        /// <param name="severity">Message severity.</param>
        /// <param name="appName">Application name.</param>
        /// <param name="message">Message text.</param>
        public SyslogMessage(
            Facility facility,
            Severity severity,
            string appName,
            string message)
        {
            this.facility = facility;
            this.severity = severity;
            this.appName = appName;
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessage"/> class
        /// for sending a RFC 3164 message.
        /// </summary>
        /// <param name="dateTimeOffset">Message timestamp.</param>
        /// <param name="facility">Message facility.</param>
        /// <param name="severity">Message severity.</param>
        /// <param name="hostName">Host name.</param>
        /// <param name="appName">Application name.</param>
        /// <param name="message">Message text.</param>
        public SyslogMessage(
            DateTimeOffset? dateTimeOffset,
            Facility facility,
            Severity severity,
            string hostName,
            string appName,
            string message)
        {
            if (hostName == null)
                throw new ArgumentNullException("hostName");

            this.dateTimeOffset = dateTimeOffset;
            this.facility = facility;
            this.severity = severity;
            this.hostName = hostName;
            this.appName = appName;
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessage"/> class
        /// for sending a RFC 5424 message.
        /// </summary>
        /// <param name="dateTimeOffset">Message timestamp.</param>
        /// <param name="facility">Message facility.</param>
        /// <param name="severity">Message severity.</param>
        /// <param name="hostName">Host name.</param>
        /// <param name="appName">Application name.</param>
        /// <param name="procId">Process identifier.</param>
        /// <param name="msgId">Message identifier.</param>
        /// <param name="message">Message text.</param>
        /// <param name="structuredDataElements">Structured data.</param>
        public SyslogMessage(
            DateTimeOffset? dateTimeOffset,
            Facility facility,
            Severity severity,
            string hostName,
            string appName,
            string procId,
            string msgId,
            string message,
            params StructuredDataElement[] structuredDataElements)
            : this(dateTimeOffset, facility, severity, hostName, appName, message)
        {
            if (structuredDataElements == null)
                throw new ArgumentNullException("structuredDataElements");

            this.procId = procId;
            this.msgId = msgId;
            this.structuredDataElements = structuredDataElements;
        }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        /// <value>syslog protocol version.</value>
        public int Version
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the facility.
        /// </summary>
        /// <value>Message facility.</value>
        public Facility Facility
        {
            get { return facility; }
        }

        /// <summary>
        /// Gets the severity.
        /// </summary>
        /// <value>Message severity.</value>
        public Severity Severity
        {
            get { return severity; }
        }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>Message timestamp.</value>
        public DateTimeOffset? DateTimeOffset
        {
            get { return dateTimeOffset; }
        }

        /// <summary>
        /// Gets the host name.
        /// </summary>
        /// <value>Host name.</value>
        public string HostName
        {
            get { return hostName; }
        }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        /// <value>Application name.</value>
        public string AppName
        {
            get { return appName; }
        }

        /// <summary>
        /// Gets the process identifier.
        /// </summary>
        /// <value>Process identifier.</value>
        public string ProcId
        {
            get { return procId; }
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>Message identifier.</value>
        public string MsgId
        {
            get { return msgId; }
        }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>Message text.</value>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Gets the structured data.
        /// </summary>
        /// <value>Structured data elements.</value>
        public IEnumerable<StructuredDataElement> StructuredDataElements
        {
            get { return structuredDataElements; }
        }
    }
}
