using System;
using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyProduct("SyslogNet.Client")]

#if !TEXT
[assembly: AssemblyCompany("Andrew Smith")]
[assembly: AssemblyCopyright("Copyright © Andrew Smith 2013")]
[assembly: AssemblyTrademark("")]
#endif

[assembly: AssemblyCulture("")]

[assembly: CLSCompliant(true)]

#if !TRANSPORT
#pragma warning disable 1699
[assembly: AssemblyKeyFile("../SyslogNet.snk")]
#pragma warning restore 1699
#endif
