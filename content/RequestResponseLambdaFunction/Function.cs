using System.Collections.Generic;
using System.IO;
using Amazon.Lambda.Core;
using Kralizek.Lambda;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EMG
{
    public class Function : RequestResponseFunction<string, string>
    {
        protected override void Configure(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", true, false);
            builder.AddObject(new {
                //#if (AddLoggly)
                Loggly = new {
                    ApplicationName = "EMG RequestResponseLambdaFunction",
                    ApiKey = "test"
                }
                //#endif
            });
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
            RegisterHandler<ToUpperStringRequestResponseHandler>(services);

            // Use this method to register your services. Exactly like in ASP.NET Core
            //#if (ConfigureAWS)
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //#endif
        }
    }
}
