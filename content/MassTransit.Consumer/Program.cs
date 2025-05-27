using Delfi.MassTransit.BusExtensions;
using MassTransit;
using MassTransit.Publisher.Consumers;
using MassTransit.Publisher.Handlers;
using MassTransit.Publisher.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder();

builder.ConfigureHostConfiguration(configuration =>
{
    configuration.SetBasePath(Directory.GetCurrentDirectory());
    configuration.AddJsonFile("hostsettings.json", true);
    configuration.AddEnvironmentVariables();
    configuration.AddCommandLine(args);
});

builder.ConfigureAppConfiguration((context, configuration) =>
{
    configuration.SetBasePath(Directory.GetCurrentDirectory());
    configuration.AddJsonFile("appsettings.json", true);
    configuration.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
    configuration.AddEnvironmentVariables();
    configuration.AddCommandLine(args);
    configuration.AddAppSecrets();
});

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<MassTransitHostedService>();

#if (RabbitMq)
    var rabbitMqConnection = RabbitMqConnectionDescriptor.Parse(context.Configuration.GetSection("RabbitMq").Value!);

    services.AddMassTransit(x =>
    {
        //Register additional consumers/messages here
        x.AddConsumer<MessageConsumer<ExampleMessage>>();

        x.UsingRabbitMq((ctx, config) =>
        {
            config.Host(rabbitMqConnection.Host, h =>
            {
                h.Username(rabbitMqConnection.UserName);
                h.Password(rabbitMqConnection.Password);
            });

            config.ReceiveEndpoint("queue-name", e =>
            {
                e.ConfigureConsumer<MessageConsumer<ExampleMessage>>(ctx);
            });
        });     
    });
#endif

#if (ActiveMq)
    var activeMqConfigSection = context.Configuration.GetSection("ActiveMQ");
    var activeMqConfig = new ActiveMqConsumerConfig();

    activeMqConfigSection.Bind(activeMqConfig);
    
    services.AddActiveMqBusConsumer(activeMqConfig, x =>
    {
        //Register additional consumers/messages here
        x.AddConsumer<MessageConsumer<ExampleMessage>>();
    });
#endif

    //Configure other services here
    services.AddTransient<IMessageHandler<ExampleMessage>, ExampleMessageHandler>();
});

builder.ConfigureLogging((context, logging) =>
{
    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
    logging.AddConsole();

    if (context.HostingEnvironment.IsProduction())
    {
        logging.AddLoggly(context.Configuration.GetSection("Loggly"));
    }
});

var host = builder.Build();
await host.RunAsync();