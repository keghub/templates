using System;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using EMG.Wcf;
using EMG.Wcf.Installers;
using Nybus.Logging;
using Topshelf;
using Topshelf.CastleWindsor;

namespace EMG.WcfWindowsServiceWithDiscovery
{
    class Program
    {
        private static readonly string ServiceName = "EMG.WcfWindowsServiceWithDiscovery";

        static void Main(string[] args)
        {
            using (var container = CreateContainer())
            {
                var loggerFactory = container.Resolve<ILoggerFactory>();
                var logger = loggerFactory.CreateCurrentClassLogger();

                var service = HostFactory.New(configuration =>
                {
                    configuration.UseWindsorContainer(container);

                    configuration.Service<WcfServiceHost<WcfWindowsServiceWithDiscovery>>(svc =>
                    {
                        svc.BeforeStartingService(sc => sc.RequestAdditionalTime(TimeSpan.FromMinutes(1)));
                        svc.BeforeStoppingService(sc => sc.RequestAdditionalTime(TimeSpan.FromMinutes(1)));

                        svc.ConstructUsingWindsorContainer();

                        svc.WhenStarted(host =>
                        {
                            logger.LogInformation($"Starting {ServiceName}");
                            host.Start();
                            logger.LogInformation($"{ServiceName} started");
                        });
                        svc.WhenStopped(host =>
                        {
                            logger.LogInformation($"Stopping {ServiceName}");
                            host.Stop();
                            logger.LogInformation($"{ServiceName} stopped");
                        });
                    });

                    configuration.SetDisplayName("EMG WcfWindowsServiceWithDiscovery");
                    configuration.SetServiceName(ServiceName);

                    // Set a more descriptive text about the service
                    //configuration.SetDescription("A service for EMG");

                    configuration.EnableServiceRecovery(rc => rc.RestartService(1).RestartService(5).RestartService(10).SetResetPeriod(1));

                    configuration.RunAsLocalSystem();
                    configuration.StartAutomaticallyDelayed();
                    configuration.SetStopTimeout(TimeSpan.FromMinutes(5));
                });

                try
                {
                    service.Run();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex);
                }
                
                container.Release(loggerFactory);
            }
        }

        private static IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Install(FromAssembly.InThisApplication());

            container.AddFacility<WcfFacility>();

            container.Install(new WcfInstaller<WcfWindowsServiceWithDiscovery>());

            return container;
        }
    }
}