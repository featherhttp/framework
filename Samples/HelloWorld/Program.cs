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