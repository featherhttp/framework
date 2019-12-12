
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public class WebApplicationHostBuilder : IHostBuilder
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly WebHostBuilder _webHostBuilder;
        
        public WebApplicationHostBuilder() : this(new HostBuilder())
        {

        }

        public WebApplicationHostBuilder(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;

            Services = new ServiceCollection();

            // HACK: MVC and Identity do this horrible thing to get the hosting environment as an instance
            // from the service collection before it is built. That needs to be fixed...
            Environment = new WebHostEnvironment();
            Services.AddSingleton(Environment);

            // REVIEW: Since the configuration base is tied to the content root, it needs to be specified as part of 
            // builder creation. It's not changing in the current design.
            Configuration = new ConfigurationBuilder().SetBasePath(Environment.ContentRootPath);
            HostConfiguration = new ConfigurationBuilder().SetBasePath(Environment.ContentRootPath);
            Logging = new LoggingBuilder(Services);
            Http = _webHostBuilder = new WebHostBuilder();
        }

        public IWebHostEnvironment Environment { get; }

        public IServiceCollection Services { get; }

        public IConfigurationBuilder Configuration { get; }

        public IConfigurationBuilder HostConfiguration { get; }

        public ILoggingBuilder Logging { get; }

        public IWebHostBuilder Http { get; }

        public IDictionary<object, object> Properties => _hostBuilder.Properties;

        public WebApplicationHost Build()
        {
            WebApplicationHost sourcePipeline = null;

            _hostBuilder.ConfigureWebHostDefaults(web =>
            {
                _webHostBuilder.ExecuteActions(web);
                
                web.Configure(destinationPipeline =>
                {
                    // The endpoints were already added on the outside
                    if (sourcePipeline.DataSources.Count > 0)
                    {
                        // The user did not register the routing middleware so wrap the entire
                        // destination pipeline in UseRouting() and UseEndpoints(), essentially:
                        // destination.UseRouting()
                        // destination.Run(source)
                        // destination.UseEndpoints()
                        if (sourcePipeline.RouteBuilder == null)
                        {
                            destinationPipeline.UseRouting();

                            // Copy the route data sources over to the destination pipeline, this should be available since we just called
                            // UseRouting()
                            var routes = (IEndpointRouteBuilder)destinationPipeline.Properties[WebApplicationHost.EndpointRouteBuilder];
                            foreach (var ds in sourcePipeline.DataSources)
                            {
                                routes.DataSources.Add(ds);
                            }

                            // Chain the execution of the source pipeline into the destination pipeline
                            destinationPipeline.Use(next =>
                            {
                                sourcePipeline.Run(next);
                                return sourcePipeline.Build();
                            });

                            // Add a UseEndpoints at the end
                            destinationPipeline.UseEndpoints(e => { });
                        }
                        else
                        {
                            // Since we register routes into the source pipeline's route builder directly,
                            // if the user called UseRouting, we need to copy the data sources
                            foreach (var ds in sourcePipeline.DataSources)
                            {
                                sourcePipeline.RouteBuilder.DataSources.Add(ds);
                            }

                            // We then implicitly call UseEndpoints at the end of the pipeline
                            sourcePipeline.UseEndpoints(_ => { });

                            // Wire the source pipeline to run in the destination pipeline
                            destinationPipeline.Run(sourcePipeline.Build());
                        }
                    }
                    else
                    {
                        // Wire the source pipeline to run in the destination pipeline
                        destinationPipeline.Run(sourcePipeline.Build());
                    }

                    // Copy the properties to the destination app builder
                    foreach (var item in sourcePipeline.Properties)
                    {
                        destinationPipeline.Properties[item.Key] = item.Value;
                    }
                });
            });

            _hostBuilder.ConfigureServices(services =>
            {
                foreach (var s in Services)
                {
                    services.Add(s);
                }
            });

            _hostBuilder.ConfigureAppConfiguration(builder =>
            {
                foreach (var s in Configuration.Sources)
                {
                    builder.Sources.Add(s);
                }
            });

            _hostBuilder.ConfigureHostConfiguration(builder =>
            {
                foreach (var s in HostConfiguration.Sources)
                {
                    builder.Sources.Add(s);
                }
            });

            var host = _hostBuilder.Build();

            return sourcePipeline = new WebApplicationHost(host);
        }

        IHost IHostBuilder.Build()
        {
            return _hostBuilder.Build();
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureAppConfiguration(configureDelegate);
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureContainer(configureDelegate);
        }

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureHostConfiguration(configureDelegate);
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return _hostBuilder.ConfigureServices(configureDelegate);
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            return _hostBuilder.UseServiceProviderFactory(factory);
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            return _hostBuilder.UseServiceProviderFactory(factory);
        }

        private class WebHostBuilder : IWebHostBuilder
        {
            private Dictionary<string, string> _settings = new Dictionary<string, string>();
            private Action<IWebHostBuilder> _operations;

            IWebHost IWebHostBuilder.Build()
            {
                return null;
            }

            public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                _operations += b => b.ConfigureAppConfiguration(configureDelegate);
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
            {
                _operations += b => b.ConfigureServices(configureServices);
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
            {
                return ConfigureServices((WebHostBuilderContext context, IServiceCollection services) => configureServices(services));
            }

            public string GetSetting(string key)
            {
                _settings.TryGetValue(key, out var value);
                return value;
            }

            public IWebHostBuilder UseSetting(string key, string value)
            {
                _operations += b => b.UseSetting(key, value);
                _settings[key] = value;
                return this;
            }

            public void ExecuteActions(IWebHostBuilder webHostBuilder)
            {
                _operations?.Invoke(webHostBuilder);
            }
        }

        private class LoggingBuilder : ILoggingBuilder
        {
            public LoggingBuilder(IServiceCollection services)
            {
                Services = services;
            }

            public IServiceCollection Services { get; }
        }

        private class WebHostEnvironment : IWebHostEnvironment
        {
            public WebHostEnvironment()
            {
                WebRootPath = "wwwroot";
                ContentRootPath = Directory.GetCurrentDirectory();
                ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
                var webRoot = Path.Combine(ContentRootPath, WebRootPath);
                WebRootFileProvider = Directory.Exists(webRoot) ? (IFileProvider)new PhysicalFileProvider(webRoot) : new NullFileProvider();
                ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
            }

            public IFileProvider WebRootFileProvider { get; set; }
            public string WebRootPath { get; set; }
            public string ApplicationName { get; set; }
            public IFileProvider ContentRootFileProvider { get; set; }
            public string ContentRootPath { get; set; }
            public string EnvironmentName { get; set; }
        }
    }

    public class WebApplicationHost : IHost, IApplicationBuilder, IEndpointRouteBuilder
    {
        public const string EndpointRouteBuilder = "__EndpointRouteBuilder";

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
