using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Extensions.Configuration;
using Nybus;
using Nybus.Configuration;
using Nybus.MassTransit;
using Nybus.Utils;

namespace EMG.WcfWindowsServiceWithNybus.Installers
{
    public class NybusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBus>().UsingFactory((IBusBuilder builder) => builder.Build()).LifeStyle.Singleton);

            container.Register(Component.For<IBusBuilder>().ImplementedBy<NybusBusBuilder>().OnCreate(ConfigureSubscriptions).LifeStyle.Singleton);

            container.Register(Component.For<IBusEngine>().ImplementedBy<MassTransitBusEngine>().LifeStyle.Singleton);

            container.Register(Component.For<NybusOptions>()
                .Forward<INybusOptions>()
                .OnCreate(o =>
                {
                    o.UseCastleWindsor(container.Kernel);
                }));

            container.Register(Component.For<MassTransitConnectionDescriptor>()
                .UsingFactoryMethod(kernel =>
                {
                    var configuration = kernel.Resolve<IConfigurationRoot>();
                    var connectionString = configuration.GetConnectionString("ServiceBus");

                    return MassTransitConnectionDescriptor.Parse(connectionString);
                }));

            container.Register(Component.For<MassTransitOptions>().OnCreate(ConfigureMassTransit).LifeStyle.Singleton);

            container.Register(Component.For<IClock>().Instance(Clock.Default));
        }

        private void ConfigureSubscriptions(IBusBuilder builder)
        {
            // Here you can subscribe to Commands and Events

            /* This will subscribe the event `MessageReceived` to any IEventHandler<MessageReceived> available */
            //builder.SubscribeToEvent<MessageReceived>();

            /* This will subscribe the event `MessageReceived` to an instance of MessageReceivedHandler */
            //builder.SubscribeToEvent<MessageReceivedHandler, MessageReceived>();
        }

        private void ConfigureMassTransit(MassTransitOptions options)
        {
            options.CommandQueueStrategy = new StaticQueueStrategy("WcfWindowsServiceWithNybus");

            options.EventQueueStrategy = new PrefixedTemporaryQueueStrategy("WcfWindowsServiceWithNybus");

            options.ServiceBusFactory = new RabbitMqServiceBusFactory(Environment.ProcessorCount);
        }
    }
}
 