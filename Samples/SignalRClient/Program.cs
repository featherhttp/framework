using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:3000/chat")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string>("Send", (message) =>
            {
                Console.WriteLine($"Recieved: '{message}'");
            });

            await connection.StartAsync();

            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                await connection.InvokeAsync("Send", "Hello World!");
            }

        }
    }
}
