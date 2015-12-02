using System;
using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyTitle("SyslogNet.Client.Text")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("Xamarin Inc.")]
[assembly: AssemblyProduct("SyslogNet.Client")]
[assembly: AssemblyCopyright("Copyright © 2014-2015 Xamarin Inc. (www.xamarin.com)")]
[assembly: AssemblyTrademark("Xamarin Inc.")]
[assembly: AssemblyCulture("")]

[assembly: CLSCompliant(true)]

[assembly: AssemblyVersion("1.0.*")]
