using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

class Chat : Hub
{
    public Task Send(string message) => Clients.All.SendAsync("Send", message);
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        var app = builder.Build();
        
        app.MapHub<Chat>("/chat");

        await app.RunAsync();
    }
}