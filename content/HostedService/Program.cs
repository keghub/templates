using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if (WCF)
using System.ServiceModel;
using EMG.Utilities.ServiceModel;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if (NybusCurrent)
using Nybus;
#endif
#if (NybusLegacy)
using Nybus;
using Nybus.Configuration;
#endif

namespace EMG.Hosted_Service
{
    class Program
    {
        static Task Main(string[] args) => CreateHostBuilder(args)
                                               .Build()
#if (RequiresWindows)
                                               .AddDefaultExitHandler()
#endif
                                               .RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                              .ConfigureHostConfiguration(ConfigureHost)
                              .ConfigureAppConfiguration(ConfigureApplication)
                              .ConfigureLogging(ConfigureLogging)
                              .ConfigureServices(ConfigureServices);

            return builder;
        }

        private static void ConfigureHost(IConfigurationBuilder configuration)
        {
            configuration.AddJsonFile("hostsettings.json", true, false);
        }

        private static void ConfigureApplication(HostBuilderContext context, IConfigurationBuilder configuration)
        {
            configuration.AddObject(new
            {
#if (NybusLegacy)
                ConnectionStrings = new
                {
                    ServiceBus = "host=rabbitmq://localhost/;username=guest;password=guest"
                },
#endif
                Loggly = new
                {
                    ApiKey = "asd-lol",
                    ApplicationName = "EMG.Hosted_Service"
                }
            });
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                logging.SetMinimumLevel(LogLevel.Trace);
            }

            if (context.HostingEnvironment.IsProduction())
            {
                logging.AddLoggly(context.Configuration.GetSection("Loggly"));
            }
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
#if (WCF)
            // https://github.com/emgdev/dotnet-utils/tree/master/docs/libraries/ServiceModel
            services.AddWcfService<TestService>(service =>
            {
                service.AddBasicHttpEndpoint(typeof(ITestService), context.Configuration.GetSection("HostedService:TestService:BasicHttp").GetBasicHttpEndpointAddress(), binding => binding.WithNoSecurity()).Discoverable();

                //service.AddNamedPipeEndpoint(typeof(ITestService), configuration.GetSection("HostedService:TestService:NamedPipe").GetNamedPipeEndpointAddress(), binding => binding.WithNoSecurity());

                //service.AddNetTcpEndpoint(typeof(ITestService), configuration.GetSection("HostedService:TestService:NetTcp").GetNetTcpEndpointAddress(), binding => binding.WithNoSecurity());

                service.AddMetadataEndpoints();

                service.AddExecutionLogging();
            });

            if (context.HostingEnvironment.IsProduction())
            {
                services.AddDiscovery<NetTcpBinding>(context.Configuration.GetSection("WCF:AnnouncementService:NetTcp").GetNetTcpEndpointAddress(), TimeSpan.FromSeconds(5), binding => binding.WithNoSecurity());
            }

#endif
#if (NybusCurrent)
            services.AddNybus(nybus =>
            {
                nybus.UseConfiguration(context.Configuration);

                nybus.UseRabbitMqBusEngine(rabbitMq => rabbitMq.UseConfiguration());

                nybus.SubscribeToCommand<TestCommand>();
            });

            services.AddHostedService<NybusHostedService>();

            services.AddCommandHandler<TestCommandHandler>();

#endif
#if (NybusLegacy)
            services.AddNybusLegacyWithMassTransit(new NybusLegacyConfiguration
            {
                ConnectionStringName = "ServiceBus",
                SubscriptionsConfigurator = builder =>
                {
                    builder.SubscribeToCommand<TestCommand>();
                },
                MassTransitConfigurator = options =>
                {
                    options.CommandQueueStrategy = new StaticQueueStrategy("Hosted_Service");

                    options.EventQueueStrategy = new PrefixedTemporaryQueueStrategy("Hosted_Service");

                    options.ServiceBusFactory = new RabbitMqServiceBusFactory(Environment.ProcessorCount);
                }
            });

            services.RegisterCommandHandler<TestCommand, TestCommandHandler>();

#endif
#if (AWS)
            services.AddDefaultAWSOptions(context.Configuration.GetAWSOptions());

            //services.AddAWSService<ISomeAmazonServiceClient>();
#endif

        }

    }
}
