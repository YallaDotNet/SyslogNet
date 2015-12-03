namespace SyslogNet.Client
{
    /// <summary>
    /// Message facility.
    /// </summary>
    public enum Facility
    {
        /// <summary>
        /// Kernel messages
        /// </summary>
        KernelMessages = 0,
        /// <summary>
        /// User-level messages
        /// </summary>
        UserLevelMessages = 1,
        /// <summary>
        /// Mail system
        /// </summary>
        MailSystem = 2,
        /// <summary>
        /// System daemons
        /// </summary>
        SystemDaemons = 3,
        /// <summary>
        /// Security or authorization messages
        /// </summary>
        SecurityOrAuthorizationMessages1 = 4,
        /// <summary>
        /// Messages generated internally by syslogd
        /// </summary>
        InternalMessages = 5,
        /// <summary>
        /// Line printer subsystem
        /// </summary>
        LinePrinterSubsystem = 6,
        /// <summary>
        /// Network news subsystem
        /// </summary>
        NetworkNewsSubsystem = 7,
        /// <summary>
        /// UUCP subsystem
        /// </summary>
        UUCPSubsystem = 8,
        /// <summary>
        /// Clock daemon
        /// </summary>
        ClockDaemon1 = 9,
        /// <summary>
        /// Security or authorization messages
        /// </summary>
        SecurityOrAuthorizationMessages2 = 10,
        /// <summary>
        /// FTP daemon
        /// </summary>
        FTPDaemon = 11,
        /// <summary>
        /// NTP subsystem
        /// </summary>
        NTPSubsystem = 12,
        /// <summary>
        /// Log audit
        /// </summary>
        LogAudit = 13,
        /// <summary>
        /// Log alert
        /// </summary>
        LogAlert = 14,
        /// <summary>
        /// Clock daemon
        /// </summary>
        ClockDaemon2 = 15,
        /// <summary>
        /// Local use 0
        /// </summary>
        LocalUse0 = 16,
        /// <summary>
        /// Local use 1
        /// </summary>
        LocalUse1 = 17,
        /// <summary>
        /// Local use 2
        /// </summary>
        LocalUse2 = 18,
        /// <summary>
        /// Local use 3
        /// </summary>
        LocalUse3 = 19,
        /// <summary>
        /// Local use 4
        /// </summary>
        LocalUse4 = 20,
        /// <summary>
        /// Local use 5
        /// </summary>
        LocalUse5 = 21,
        /// <summary>
        /// Local use 6
        /// </summary>
        LocalUse6 = 22,
        /// <summary>
        /// Local use 7
        /// </summary>
        LocalUse7 = 23
    }
}
