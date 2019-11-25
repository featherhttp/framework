
using System;
using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World MVC";
}

class Program
{
    static async Task Main(string[] args)
    {
        var app = HttpApplication.Create(services =>
        {
            services.AddControllers();
        });

        var routes = app.Router();

        routes.MapControllers();

        var server = await app.StartServerAsync("http://localhost:3000");

        Console.WriteLine($"Listening on {string.Join(", ", server.Addresses)}");
        Console.ReadLine();
    }
}
