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

        app.MapCarter();

        await app.RunAsync();
    }
}