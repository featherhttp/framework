using FeatherHttp;
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

        builder.UseUrls("http://localhost:3000");

        var app = builder.Build();

        app.UseDeveloperExceptionPage();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await app.RunAsync();
    }
}