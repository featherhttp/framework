using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplicationHost.CreateDefaultBuilder(args);

        var app = builder.Build();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World!");
        });

        await app.RunAsync();
    }
}
