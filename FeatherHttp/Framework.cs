
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace FeatherHttp
{
    public static class HttpApplication
    {
        public static IApplicationBuilder Create(Action<IServiceCollection> configure = null)
        {
            var diagnoticListener = new DiagnosticListener("FeatherHttp");
            var services = new ServiceCollection()
                            .AddRouting()
                            .AddLogging()
                            .AddOptions()
                            .AddSingleton(diagnoticListener)
                            .AddSingleton<DiagnosticSource>(diagnoticListener)
                            .AddSingleton<IServer, KestrelServer>()
                            .AddSingleton<IConnectionListenerFactory, SocketTransportFactory>()
                            .AddSingleton<IWebHostEnvironment, WebHostEnvironment>();

            services.AddOptions<KestrelServerOptions>()
                    .Configure<IServiceProvider>((o, sp) =>
                    {
                        o.ApplicationServices = sp;
                    });


            configure?.Invoke(services);

            return new ApplicationBuilder(services.BuildServiceProvider());
        }

        public static async Task<HttpServer> StartServerAsync(this IApplicationBuilder app, params string[] addresses)
        {
            var server = app.ApplicationServices.GetRequiredService<IServer>();
            var factory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            var feature = server.Features.Get<IServerAddressesFeature>();

            foreach (var address in addresses)
            {
                feature.Addresses.Add(address);
            }

            await server.StartAsync(new Application(factory, app.UseEndpoints(e => { }).Build()), default);
            return new HttpServer(server);
        }

        private class Application : IHttpApplication<HttpContext>
        {
            private readonly RequestDelegate _requestDelegate;
            private readonly IServiceScopeFactory _serviceScopeFactory;
            public Application(IServiceScopeFactory serviceScopeFactory, RequestDelegate requestDelegate)
            {
                _requestDelegate = requestDelegate;
                _serviceScopeFactory = serviceScopeFactory;
            }

            public HttpContext CreateContext(IFeatureCollection contextFeatures)
            {
                var context = new DefaultHttpContext(contextFeatures);
                context.ServiceScopeFactory = _serviceScopeFactory;
                return context;
            }

            public void DisposeContext(HttpContext context, Exception exception)
            {

            }

            public Task ProcessRequestAsync(HttpContext context)
            {
                return _requestDelegate(context);
            }
        }

        private class WebHostEnvironment : IWebHostEnvironment
        {
            public WebHostEnvironment()
            {
                WebRootPath = "wwwroot";
                ContentRootPath = Directory.GetCurrentDirectory();
                ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
                WebRootFileProvider = new PhysicalFileProvider(Path.Combine(ContentRootPath, WebRootPath));
            }

            public IFileProvider WebRootFileProvider { get; set; }
            public string WebRootPath { get; set; }
            public string ApplicationName { get; set; }
            public IFileProvider ContentRootFileProvider { get; set; }
            public string ContentRootPath { get; set; }
            public string EnvironmentName { get; set; }
        }
    }

    public static class ApplicationBuilderExtensions
    {
        private const string EndpointRouteBuilder = "__EndpointRouteBuilder";

        public static IEndpointRouteBuilder Router(this IApplicationBuilder app)
        {
            app.UseRouting();
            return (IEndpointRouteBuilder)app.Properties[EndpointRouteBuilder];
        }
    }

    public class HttpServer
    {
        private IServer _server;

        public HttpServer(IServer server)
        {
            _server = server;
        }

        public ICollection<string> Addresses => _server.Features.Get<IServerAddressesFeature>().Addresses;

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _server.StopAsync(cancellationToken);
        }
    }
}
