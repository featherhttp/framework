using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// A builder for web applications and services.
    /// </summary>
    public class WebApplicationBuilder
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly WebHostBuilder _webHostBuilder;

        /// <summary>
        /// Creates a <see cref="WebApplicationBuilder"/>.
        /// </summary>
        public WebApplicationBuilder() : this(new HostBuilder())
        {

        }

        internal WebApplicationBuilder(IHostBuilder hostBuilder)
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
            Server = _webHostBuilder = new WebHostBuilder();
            Host = _hostBuilder;
        }

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public IConfigurationBuilder Configuration { get; }

        /// <summary>
        /// A collection of configuration providers for the host to compose. This is a more advanced settings since there are only some settings that apply to the host.
        /// </summary>
        public IConfigurationBuilder HostConfiguration { get; }

        /// <summary>
        /// A collection of logging providers for the applicaiton to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging { get; }

        /// <summary>
        /// A builder for configuring web specific properties. 
        /// </summary>
        public IWebHostBuilder Server { get; }

        /// <summary>
        /// A builder for configure host specific properties.
        /// </summary>
        public IHostBuilder Host { get; }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties => _hostBuilder.Properties;

        /// <summary>
        /// Builds the <see cref="WebApplication"/>.
        /// </summary>
        /// <returns>A configured <see cref="WebApplication"/>.</returns>
        public WebApplication Build()
        {
            WebApplication sourcePipeline = null;

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
                            var routes = (IEndpointRouteBuilder)destinationPipeline.Properties[WebApplication.EndpointRouteBuilder];
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

            return sourcePipeline = new WebApplication(host);
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
}
