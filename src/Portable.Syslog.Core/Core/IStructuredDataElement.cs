using System;
using System.Collections.Generic;

namespace SyslogNet.Client
{
    /// <summary>
    /// RFC 5424 structured data element.
    /// </summary>
    [Obsolete("This interface will be removed in a future version.")]
    public interface IStructuredDataElement
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>Structured data identifier.</value>
        string SdId { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>Structured data parameters.</value>
        Dictionary<string, string> Parameters { get; }
    }
}
