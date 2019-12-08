using FeatherHttp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplicationHost.CreateDefaultBuilder(args);

        builder.Configuration.AddYamlFile("appsettings.yml", optional: true);

        builder.UseSerilog();

        builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.UseUrls("http://localhost:3000");

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });

        await app.RunAsync();
    }
}