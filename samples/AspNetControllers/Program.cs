using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

await app.RunAsync();

public class HomeController
{
    [HttpGet("/")]
    public string HelloWorld() => "Hello World";
}