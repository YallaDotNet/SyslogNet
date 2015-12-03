[![Build status](https://ci.appveyor.com/api/projects/status/lnoc3a1118t9og8o?svg=true)](https://ci.appveyor.com/project/dmitry-shechtman/syslognet-yia6l)

Portable syslog
===============

Introduction
------------

Portable syslog client for .NET.

This is a fork of [SyslogNet](https://github.com/emertechie/SyslogNet), ported and documented as part of [YALLA.NET](http://YallaDotNet.github.io).

Platforms
---------

* .NET Framework 4.5
* Windows Store
* Windows Phone 8 Silverlight
* Windows Phone 8.1
* Xamarin.Android
* Xamarin.iOS
* Xamarin.Mac
* Portable Class Libraries

Serializers
-----------

* RFC 3164
* RFC 5424
* Local (Linux/BSD/OSX)

Transports
----------

* UDP
* TCP
* Secure TCP (TLS/SSL)

Installation
------------

Install the [NuGet package](https://nuget.org/packages/portable.syslog):

```PowerShell
Install-Package portable.syslog
```

**Note:** The package must be installed in the main project in order for the underlying network library to resolve to the specific platform version.

Notice
------

Copyright (c) 2013 Andrew Smith  
Portable.Text.Encoding (c) 2014-2015 Xamarin Inc.  
Portable syslog (c) 2015 Dmitry Shechtman

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
