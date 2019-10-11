using System;
using System.Collections.Generic;
using EMG.Utilities.Hosting;
#if (WCF)
using System.ServiceModel;
using EMG.Utilities.ServiceModel;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#if (NybusCurrent)
using Nybus;
#endif
#if (NybusLegacy)
using Nybus;
using Nybus.Configuration;
#endif
using Topshelf;

namespace EMG.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = HostFactory.New(configure =>
            {
                configure.HostService(ConfigureAppConfiguration, ConfigureServices, ConfigureLogging);

                configure.SetDisplayName("EMG WindowsService");

                configure.SetServiceName("EMG.WindowsService");

                configure.SetDescription("A Windows service created by EMG");

                configure.EnableServiceRecovery(rc => rc.RestartService(TimeSpan.FromMinutes(1))
                                                        .RestartService(TimeSpan.FromMinutes(5))
                                                        .RestartService(TimeSpan.FromMinutes(10))
                                                        .SetResetPeriod(1));

                configure.RunAsLocalService();

                configure.StartAutomaticallyDelayed();

                configure.SetStopTimeout(TimeSpan.FromMinutes(5));
            });

            host.Run();
        }

        private static void ConfigureAppConfiguration(IConfigurationBuilder configuration, IServiceEnvironment environment)
        {
            var settings = new Dictionary<string, string>
            {
#if (NybusLegacy)
                ["ConnectionStrings:ServiceBus"] = "host=rabbitmq://localhost/;username=guest;password=guest",
#endif
                ["Loggly:ApiKey"] = "asd-lol",
                ["Loggly:ApplicationName"] = "EMG.WindowsService"
            };

            configuration.AddInMemoryCollection(settings);

            configuration.AddJsonFile("appsettings.json", true, true);
            configuration.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);

            configuration.AddEnvironmentVariables();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IServiceEnvironment environment)
        {
#if (WCF)
            // https://github.com/emgdev/dotnet-utils/tree/master/docs/libraries/ServiceModel
            services.AddWcfService<TestService>(service =>
            {
                service.AddBasicHttpEndpoint(typeof(ITestService), configuration.GetSection("WindowsService:TestService:BasicHttp").GetBasicHttpEndpointAddress(), binding => binding.WithNoSecurity()).Discoverable();

                //service.AddNamedPipeEndpoint(typeof(ITestService), configuration.GetSection("WindowsService:TestService:NamedPipe").GetNamedPipeEndpointAddress(), binding => binding.WithNoSecurity());

                //service.AddNetTcpEndpoint(typeof(ITestService), configuration.GetSection("WindowsService:TestService:NetTcp").GetNetTcpEndpointAddress(), binding => binding.WithNoSecurity());

                service.AddMetadataEndpoints();

                service.AddExecutionLogging();
            });

            if (environment.IsProduction())
            {
                services.AddDiscovery<NetTcpBinding>(configuration.GetSection("WCF:AnnouncementService:NetTcp").GetNetTcpEndpointAddress(), TimeSpan.FromSeconds(5), binding => binding.WithNoSecurity());
            }
#endif

#if (NybusCurrent)
            services.AddNybus(nybus =>
            {
                nybus.UseConfiguration(configuration);

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
                    options.CommandQueueStrategy = new StaticQueueStrategy("WindowsService");

                    options.EventQueueStrategy = new PrefixedTemporaryQueueStrategy("WindowsService");

                    options.ServiceBusFactory = new RabbitMqServiceBusFactory(Environment.ProcessorCount);
                }
            });

            services.RegisterCommandHandler<TestCommand, TestCommandHandler>();
#endif

#if (AWS)
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            //services.AddAWSService<ISomeAmazonServiceClient>();
#endif
        }

        private static void ConfigureLogging(ILoggingBuilder logging, IConfiguration configuration, IServiceEnvironment environment)
        {
            logging.AddConfiguration(configuration.GetSection("Logging"));

            logging.AddConsole();

            logging.AddLoggly(configuration.GetSection("Loggly"));
        }
    }
}
