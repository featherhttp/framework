
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public class WebApplicationHost : IHost, IApplicationBuilder, IEndpointRouteBuilder
    {
        internal const string EndpointRouteBuilder = "__EndpointRouteBuilder";

        private readonly IHost _host;
        private readonly List<EndpointDataSource> _dataSources = new List<EndpointDataSource>();

        public WebApplicationHost(IHost host)
        {
            _host = host;
            ApplicationBuilder = new ApplicationBuilder(host.Services);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName);
        }

        public IServiceProvider Services => _host.Services;

        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        public IWebHostEnvironment Environment => _host.Services.GetRequiredService<IWebHostEnvironment>();

        public IHostApplicationLifetime ApplicationLifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        public ILogger Logger { get; }

        public IFeatureCollection ServerFeatures => _host.Services.GetRequiredService<IServer>().Features;

        public IServiceProvider ApplicationServices { get => ApplicationBuilder.ApplicationServices; set => ApplicationBuilder.ApplicationServices = value; }

        public IDictionary<string, object> Properties => ApplicationBuilder.Properties;

        public ICollection<EndpointDataSource> DataSources => _dataSources;

        internal IEndpointRouteBuilder RouteBuilder
        {
            get
            {
                Properties.TryGetValue(EndpointRouteBuilder, out var value);
                return (IEndpointRouteBuilder)value;
            }
        }

        internal ApplicationBuilder ApplicationBuilder { get; }

        public IServiceProvider ServiceProvider => Services;

        public void Listen(params string[] urls)
        {
            var addresses = ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            addresses.Clear();
            foreach (var u in urls)
            {
                addresses.Add(u);
            }
        }

        public static WebApplicationHostBuilder CreateDefaultBuilder()
        {
            return new WebApplicationHostBuilder(Host.CreateDefaultBuilder());
        }

        public static WebApplicationHostBuilder CreateDefaultBuilder(string[] args)
        {
            return new WebApplicationHostBuilder(Host.CreateDefaultBuilder(args));
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return _host.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _host.StopAsync(cancellationToken);
        }

        public void Dispose()
        {
            _host.Dispose();
        }

        public RequestDelegate Build()
        {
            return ApplicationBuilder.Build();
        }

        public IApplicationBuilder New()
        {
            return ApplicationBuilder.New();
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            ApplicationBuilder.Use(middleware);
            return this;
        }

        public IApplicationBuilder CreateApplicationBuilder() => New();
    }
}
