using System;
using System.Collections.Generic;

namespace SyslogNet.Client
{
    /// <summary>
    /// RFC 5424 structured data element.
    /// </summary>
#pragma warning disable 612, 618
    public class StructuredDataElement : IStructuredDataElement
#pragma warning restore 612, 618
    {
        /// <summary>
        /// Private enterprise number.
        /// </summary>
        /// RFC 5424 specifies that you must provide a private enterprise number.
        /// If none specified, the example number reserved for documentation will be used (see RFC).
        /// <remarks><strong>This field is obsolete and will be removed in a future version.</strong></remarks>
        [Obsolete("This field will be removed in a future version.")]
        public const string DefaultPrivateEnterpriseNumber = "32473";

        private readonly string sdId;
        private readonly Dictionary<string, string> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredDataElement"/> class.
        /// </summary>
        /// <param name="sdId">Structured data identifier.</param>
        /// <param name="parameters">Structured data parameters.</param>
        /// <exception cref="ArgumentNullException">
        /// Missing <paramref name="sdId"/> or <paramref name="parameters"/> value.
        /// </exception>
        public StructuredDataElement(string sdId, IDictionary<string, string> parameters)
        {
            if (sdId == null)
                throw new ArgumentNullException("sdId");
            if (parameters == null)
                throw new ArgumentNullException("parameters");

#pragma warning disable 612, 618
            this.sdId = sdId.Contains("@") ? sdId : sdId + "@" + DefaultPrivateEnterpriseNumber;
#pragma warning restore 612, 618
            this.parameters = new Dictionary<string, string>(parameters);
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
        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        Dictionary<string, string> IStructuredDataElement.Parameters
        {
            get { return parameters; }
        }
    }
}
