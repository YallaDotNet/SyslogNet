using System;
using System.Collections.Generic;

namespace SyslogNet.Client
{
    /// <summary>
    /// RFC 5424 structured data element.
    /// </summary>
    public class StructuredDataElement
    {
        /// <summary>
        /// Private enterprise number.
        /// </summary>
        /// RFC 5424 specifies that you must provide a private enterprise number.
        /// If none specified, the example number reserved for documentation will be used (see RFC).
        [Obsolete]
        public const string DefaultPrivateEnterpriseNumber = "32473";

        private readonly string sdId;
        private readonly Dictionary<string, string> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredDataElement"/> class.
        /// </summary>
        /// <param name="sdId">Structured data identifier.</param>
        /// <param name="parameters">Structured data parameters.</param>
        public StructuredDataElement(string sdId, Dictionary<string, string> parameters)
        {
            if (sdId == null)
                throw new ArgumentNullException("sdId");
            if (parameters == null)
                throw new ArgumentNullException("parameters");

#pragma warning disable 612
            this.sdId = sdId.Contains("@") ? sdId : sdId + "@" + DefaultPrivateEnterpriseNumber;
#pragma warning restore 612
            this.parameters = parameters;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>Structured data identifier.</value>
        public string SdId
        {
            get { return sdId; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>Structured data parameters.</value>
        public Dictionary<string, string> Parameters
        {
            get { return parameters; }
        }
    }
}
