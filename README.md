## Feather HTTP

A super lightweight low ceremony APIs for ASP.NET Core applications.

- Built on the same primitives as ASP.NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing ASP.NET Core middleware and frameworks


### Hello World Sample

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

        var host = builder.Build();

        var app = host.ApplicationBuilder;

        var routes = app.UseRouter();

        routes.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await host.RunAsync();
    }
}
```

### ASP.NET Core Controller


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

        var host = builder.Build();

        var app = host.ApplicationBuilder;

        var routes = app.UseRouter();

        routes.MapControllers();
        
        await host.RunAsync();
    }
}
```