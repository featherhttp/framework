using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var app = WebApplication.Create(args);

app.MapGet("/", async http =>
{
    await http.Response.WriteAsJsonAsync(new { message = "Hello World" });
});

await app.RunAsync();