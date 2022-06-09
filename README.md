# Overview

This repo contains sample ASP.NET apps demonstrating common web app code patterns that may be useful for [Upgrade Assistant](https://github.com/dotnet/upgrade-assistant/) to automatically flag or fix (as cataloged [here](https://github.com/dotnet/upgrade-assistant/issues?q=is%3Aopen+label%3Aroslyn-fixable+label%3Aarea%3AASPNET+)).

The original app is in the *netfx* folder and has comments where the code patterns are used with the dotnet/upgrade-assistant issue # corresponding to the pattern represented (for example, `// #673 BindAttribute` appears just before the line using the include property of `BindAttribute`).

An upgraded ASP.NET Core app showing how all of the code patterns should be updated is available in the *net6* directory. There are also .old.cs files that have the code moved into the new project but *not* fixed. These are excluded from the build by default (since they don't compile) but can be enabled in case it would be useful to see how a Roslyn analyzer performs on the code patterns in the context of an ASP.NET Core app.

Of course, the app itself is fairly contrived and is not meant to be used as a sample except for showcasing some of these code patterns.
