using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddYamlFile("appsettings.yml", optional: true);

builder.Host.UseSerilog((context, configuration)
    => configuration
        .Enrich
        .FromLogContext()
        .WriteTo
        .Console()
    );

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    // Register services using Autofac specific methods here
});

var app = builder.Build();

app.Listen("http://localhost:3000");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/", async http =>
{
    await http.Response.WriteAsync("Hello World");
});

await app.RunAsync();