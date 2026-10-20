using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EMG
{
    public class Function
    {
        private readonly IServiceProvider _serviceProvider;

        public Function()
        {
            var builder = new ConfigurationBuilder();
            Configure(builder);
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging(logging => ConfigureLogging(logging, configuration));
            ConfigureServices(services, configuration);

            _serviceProvider = services.BuildServiceProvider();
        }

        public async Task<string> FunctionHandlerAsync(string input, ILambdaContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestResponseHandler<string, string>>();
            return await handler.HandleAsync(input, context);
        }

        private static void Configure(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", true, false);
            builder.AddEnvironmentVariables();
        }

        private static void ConfigureLogging(ILoggingBuilder logging, IConfiguration configuration)
        {
            logging.AddConfiguration(configuration.GetSection("Logging"));
            logging.AddLambdaLogger(configuration, "Logging");
            logging.AddLoggly(configuration.GetSection("Loggly"));
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // You need this line to register your handler
            services.AddTransient<IRequestResponseHandler<string, string>, ToUpperStringRequestResponseHandler>();

            // Use this method to register your services. Exactly like in ASP.NET Core
            //#if (ConfigureAWS)
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            //#endif
        }
    }
}
