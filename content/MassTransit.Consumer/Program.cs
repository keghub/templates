using MassTransit;
using MassTransit.BusFactory;
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

    var rabbitMqConnection = RabbitMqConnectionDescriptor.Parse(context.Configuration.GetSection("RabbitMq").Value);

    services.AddMassTransit(x =>
    {
        x.AddConsumer<MessageConsumer<ExampleMessage>>();

        x.UsingRabbitMq((context, config) =>
        {
            config.Host(rabbitMqConnection.Host, h =>
            {
                h.Username(rabbitMqConnection.UserName);
                h.Password(rabbitMqConnection.Password);
            });
            config.ConfigureEndpoints(context);
        });     
    });
    services.AddTransient<IMessageHandler<ExampleMessage>, ExampleMessageHandler<ExampleMessage>>();

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