using System.Collections.Generic;
using System.IO;
using Amazon.Lambda.Core;
using Kralizek.Lambda;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EMG
{
    public class Function : EventFunction<string>
    {
        private static readonly Dictionary<string, string> Settings = new Dictionary<string, string>
        {
            //#if (AddLoggly)
            ["Loggly:ApplicationName"] = "EMG EventLambdaFunction",
            ["Loggly:ApiKey"] = "test",
            //#endif
        };

        protected override void Configure(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", true, false);
            builder.AddInMemoryCollection(Settings);
            builder.AddEnvironmentVariables();
        }

        protected override void ConfigureLogging(ILoggingBuilder logging, IExecutionEnvironment executionEnvironment)
        {
            logging.AddConfiguration(Configuration.GetSection("Logging"));
            logging.AddLambdaLogger(Configuration, "Logging");
            //#if (AddLoggly)
            logging.AddLoggly(Configuration.GetSection("Loggly"));
            //#endif
        }

        protected override void ConfigureServices(IServiceCollection services, IExecutionEnvironment executionEnvironment)
        {
            // You need this line to register your handler
            RegisterHandler<StringEventHandler>(services);

            // Use this method to register your services. Exactly like in ASP.NET Core
            //#if (ConfigureAWS)
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //#endif
        }
    }
}
