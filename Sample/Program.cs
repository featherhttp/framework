using System;
using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World MVC";
}

public class Chat : Hub
{
    public Task Send(string message) => Clients.All.SendAsync("Send", message);
}

class Program
{
    static async Task Main(string[] args)
    {
        var app = HttpApplication.Create(services =>
        {
            services.AddControllers();
            services.AddSignalR();
        });

        var routes = app.Router();

        routes.MapControllers();
        routes.MapHub<Chat>("/chat");

        var server = await app.StartServerAsync("http://localhost:3000");

        Console.WriteLine($"Listening on {string.Join(", ", server.Addresses)}");
        Console.ReadLine();
    }
}
