using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World";
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateDefaultBuilder(args);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        await app.RunAsync();
    }
}