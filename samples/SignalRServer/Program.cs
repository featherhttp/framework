using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<Chat>("/chat");

await app.RunAsync();

class Chat : Hub
{
    public Task Send(string message) => Clients.All.SendAsync("Send", message);
}