## Feather HTTP

[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Ffeatherhttp%2Fframework%2Fshield%2FFeatherHttp%2Flatest&label=FeatherHttp)](https://f.feedz.io/featherhttp/framework/packages/FeatherHttp/latest/download)

A lightweight low ceremony APIs for .NET Core applications.

- Built on the same primitives as .NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing .NET Core middleware and frameworks

### Hello World

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var app = WebApplication.Create(args);

app.MapGet("/", async http =>
{
    await http.Response.WriteAsync("Hello World");
});

await app.RunAsync();
```

## Tutorial

The [tutorial](https://github.com/featherhttp/tutorial) will walk you through building an HTTP API for a todo application using FeatherHttp.

## Using CI Builds

To use the `dotnet new` template, use the following command

```
dotnet new -i FeatherHttp.Templates::{version} --nuget-source https://f.feedz.io/featherhttp/framework/nuget/index.json
```

Once you've installed the template, run:

```
dotnet new feather -n {name}
``` 

This will create a new project using FeatherHttp.

To use CI builds add the following nuget feed:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear />
        <add key="featherhttp" value="https://f.feedz.io/featherhttp/framework/nuget/index.json" />
        <add key="NuGet.org" value="https://api.nuget.org/v3/index.json" />
    </packageSources>
</configuration>
```

See the list of [versions](https://f.feedz.io/featherhttp/framework/nuget/v3/packages/FeatherHttp/index.json)
