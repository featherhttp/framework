using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World";
}

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

        builder.Services.AddControllers();
        builder.Services.AddSignalR();

        var host = builder.Build();

        var app = host.ApplicationBuilder;

        var routes = app.UseRouter();

        routes.MapControllers();
        routes.MapHub<Chat>("/chat");
        
        await host.RunAsync();
    }
}
