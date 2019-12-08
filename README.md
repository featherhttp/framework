## Feather HTTP

A super lightweight low ceremony APIs for ASP.NET Core applications.

- Built on the same primitives as ASP.NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing ASP.NET Core middleware and frameworks

### Hello World

```C#
using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplicationHost.CreateDefaultBuilder(args);

        builder.UseUrls("http://localhost:3000");

        var app = builder.Build();

        app.UseRouting();

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
using FeatherHttp;
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
        var builder = WebApplicationHost.CreateDefaultBuilder(args);
        
        builder.UseUrls("http://localhost:3000");

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseRouting();

        app.MapControllers();
        
        await app.RunAsync();
    }
}
```

### Carter

```C#
using FeatherHttp;
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
        var builder = WebApplicationHost.CreateDefaultBuilder(args);
        
        builder.UseUrls("http://localhost:3000");

        builder.Services.AddCarter();

        var app = builder.Build();

        app.UseRouting();

        app.MapCarter();

        await app.RunAsync();
    }
}
```

### SignalR

```C#
using FeatherHttp;
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
        var builder = WebApplicationHost.CreateDefaultBuilder(args);
        
        builder.UseUrls("http://localhost:3000");

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseRouting();

        app.MapHub<Chat>("/chat");
        
        await app.RunAsync();
    }
}
```

### GRPC

```C#
using FeatherHttp;
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
        var builder = WebApplicationHost.CreateDefaultBuilder(args);
        
        builder.UseUrls("https://localhost:3000");

        builder.Services.AddGrpc();

        var host = builder.Build();

        app.UseRouting();

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
using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplicationHost.CreateDefaultBuilder(args);

        builder.Configuration.AddYamlFile("appsettings.yml", optional: true);

        builder.UseSerilog();

        builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.UseUrls("http://localhost:3000");

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouter();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await app.RunAsync();
    }
}
```