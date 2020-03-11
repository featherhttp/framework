using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FeatherHttp_Template
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapGet("/", async http =>
            {
                await http.Response.WriteAsync("Hello World!");
            });

            await app.RunAsync();
        }
    }
}
