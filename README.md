## Feather HTTP Framework

A super lightweight low ceremony framework for .NET Core.

- Built on the same primitives as ASP.NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing ASP.NET Core middleware


Hello World Sample

```
using System;
using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

class Program
{
    public static async Task Main(string[] args)
    {
        var app = HttpApplication.Create();

        var routes = app.Router();

        routes.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        var server = await HttpApplication.StartServerAsync(app, "http://localhost:3000");

        Console.WriteLine($"Listening on {string.Join(", ", server.Addresses)}");
        Console.ReadLine();
    }
}
```