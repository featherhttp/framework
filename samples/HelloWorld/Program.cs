using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

class Program
{
    static async Task Main(string[] args)
    {
        var app = WebApplication.Create(args);

        app.MapGet("/", async context =>
        {
            await context.Response.WriteJsonAsync(new { message = "Hello World" });
        });

        await app.RunAsync();
    }
}