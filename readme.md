# Kurejito

**Kurejito: Write your app now. Choose your bank later.**

Kurejito is a standard API for incorporating card payments into your .NET or Mono application.

* Write your app against a clean API, using built-in test implementations to refine your payment system.
* Deploy your app against any supported payment provider with minimal code changes - or write your own provider.
* Switch providers easily if your current payment provider goes under, goes offline, or just plain lets you down.

_Kurejito is Japanese for "credit" - or rather, it's the English word as pronounced when written in katakana._  [@dylanbeattie](http://twitter.com/dylanbeattie/status/4526129926901760)

Sparked by this [tweet](http://twitter.com/dylanbeattie/status/4143251615383552) and galvanised by the existence of [Active Merchant](http://www.activemerchant.org/).

## Draft 0.1 Plan
_Keep it simple.  One assembly, basic functionality, no dependencies. Build, discuss, re-design, re-factor, rinse-repeat._

### Build the minimum viable implementation
* Datacash [@dylanbeattie](http://twitter.com/dylanbeattie)
* SagePay [@dylanbeattie](http://twitter.com/dylanbeattie)
* PayPal NVP [@bentayloruk](http://twitter.com/bentayloruk)
* TBC @you-yes-you-know-you-want-to-get-involved

### Tech
.NET 3.5, C#, Moq, Should, xUnit, Visual Studio 2010 sln.

### What can you do?
We'd love to get as many people involved as possible.  Right now it is early days and we're just hacking the basics, so it's difficult to divvy up tasks.  Suggestions would be:

* Follow the [#kurejito hashtag](http://twitter.com/#search?q=%23kurejito)
* Tell us about good code with a friendly license that we might want to look at.
* Anything else you can think of that will make this a success!

### For Kurejito developers

* [Kurejito Developers Google Group](http://groups.google.com/group/kurejito)
* [Kurejito IRC channel on Freenode](http://webchat.freenode.net/)

### What Goes Where

    /src
        /Kurejito.sln                  Visual Studio 2010 solution file
        /Kurejito.5.1.ReSharper	       Resharper project settings
        /Kurejito                      Core project source code
        /Kurejio.Tests                 Unit and integration tests for Kurejito
    /lib                               Libraries and binaries (Moq, xUnit, &c) used by Kurejito
    /doc                               Project documentation intended for eventual distribution
    /etc                               All the rest - notes, artwork, specs, legalities, and so on.








