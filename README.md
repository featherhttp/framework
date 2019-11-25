## Feather HTTP

A super lightweight low ceremony APIs for ASP.NET Core applications.

- Built on the same primitives as ASP.NET Core
- Optimized for building HTTP APIs quickly
- Take advantage of existing ASP.NET Core middleware and frameworks


Hello World Sample

```C#
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

        var server = await app.StartServerAsync("http://localhost:3000");

        Console.WriteLine($"Listening on {string.Join(", ", server.Addresses)}");
        Console.ReadLine();
    }
}
```
