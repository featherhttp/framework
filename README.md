## Feather HTTP

[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fdavidfowl%2Ffeatherhttp%2Fshield%2FFeatherHttp%2Flatest&label=FeatherHttp)](https://f.feedz.io/davidfowl/featherhttp/packages/FeatherHttp/latest/download)

A lightweight low ceremony APIs for ASP.NET Core applications.

- Built on the same primitives as ASP.NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing ASP.NET Core middleware and frameworks

### Hello World

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);

        var app = builder.Build();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await app.RunAsync();
    }
}
```

### ASP.NET Core Controllers


```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World";
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);
        
        builder.Services.AddControllers();
        
        var app = builder.Build();

        app.MapControllers();
        
        await app.RunAsync();
    }
}
```

### Carter

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Carter;

public class HomeModule : CarterModule
{
    public HomeModule()
    {
        Get("/", async (req, res) => await res.WriteAsync("Hello from Carter!"));
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);

        builder.Services.AddCarter();

        var app = builder.Build();

        app.MapCarter();

        await app.RunAsync();
    }
}
```

### SignalR

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

class Chat : Hub
{
    public Task Send(string message) => Clients.All.SendAsync("Send", message);
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);
        
        builder.Services.AddSignalR();

        var app = builder.Build();
        
        app.MapHub<Chat>("/chat");
        
        await app.RunAsync();
    }
}
```

### GRPC

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Core;
using Greet;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);
       
        builder.Services.AddGrpc();

        var app = builder.Build();
        
        app.Listen("https://localhost:3000");

        app.MapGrpcService<GreeterService>();
        
        await app.RunAsync();
    }
}
```

### Uber Example

- Serilog for logging
- Autofac for DI
- Yaml configuration provider

```C#
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);

        builder.Configuration.AddYamlFile("appsettings.yml", optional: true);

        builder.Host.UseSerilog((context, configuration) 
            => configuration
                .Enrich
                .FromLogContext()
                .WriteTo
                .Console()
            );

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.Host.ConfigureContainer<ContainerBuilder>(b =>
        {
            // Register services using Autofac specific methods here
        });

        var app = builder.Build();
        
        app.Listen("http://localhost:3000");
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Be explicit about the routing middleware
        app.UseRouting();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await app.RunAsync();
    }
}
```

## Using CI Builds

To use CI builds add the following nuget feed:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear />
        <add key="featherhttp" value="https://f.feedz.io/davidfowl/featherhttp/nuget/index.json" />
        <add key="NuGet.org" value="https://api.nuget.org/v3/index.json" />
    </packageSources>
</configuration>
```
