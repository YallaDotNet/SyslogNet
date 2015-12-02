namespace SyslogNet.Client.Transport
{
    /// <summary>
    /// Message transfer.
    /// </summary>
    public enum MessageTransfer
    {
        /// <summary>
        /// Octet Counting
        /// </summary>
        OctetCounting = 0,

        /// <summary>
        /// Non-Transparent-Framing
        /// </summary>
        NonTransparentFraming = 1
    }
}
