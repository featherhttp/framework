using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World";
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplicationHost.CreateDefaultBuilder(args);

        builder.UseUrls("http://localhost:3000");

        var startup = new Startup();
        startup.ConfigureServices(builder.Services);

        var host = builder.Build();

        startup.Configure(host.ApplicationBuilder, host.Environment);

        await host.RunAsync();
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var routes = app.UseRouter();

        routes.MapControllers();
    }
}