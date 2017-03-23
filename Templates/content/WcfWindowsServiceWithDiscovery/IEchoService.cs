using System;
using System.ServiceModel;
using EMG.Wcf;
using EMG.Wcf.Extensions;
using Nybus.Logging;
using System.Threading.Tasks;

namespace EMG
{
    /* 
        The contract to expose.
        Can have both synchronous and asynchronous methods. WCF will create support for both cases.
        Due to C# compiler limitations, your service will have to perfectly implement the interface.
        Asynchronous methods in the interface is suggested.
    */
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string message);

        [OperationContract]
        Task<string> UpperCaseAsync(string message);
    }

    /*
        The concrete implementation of your service.
        It can implement more than one service contract.
    */
    public class EchoService : IEchoService
    {
        private readonly ILogger _logger;

        public EchoService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public string Echo(string message)
        {
            _logger.LogInformation($"Received: {message}");
            return message;
        }

        public async Task<string> UpperCaseAsync(string message)
        {
            _logger.LogInformation($"Received: {message}");
            return message?.ToUpper();
        }
    }

    /*
        This class is used to configure the service.
        Here you can register the endpoints you want your service to expose.
        Some notes: 
        * BasicHttp and WSHttp can share the same port, granted they have different suffix
        * NetTcp doesn't need a suffix but it requires a specific port
        * NamedPipe doesn't use any port but it requires a suffix
        * Each endpoint can be configured indipendently.
        * Each binding can be used more than once, granted they have unique address
        * If the service implements more than one Service Contract, each contract should be added indipendently.
        * Each binding can be customized through a Action<TBinding>
        * Default binding setting: all timeouts are set to 70 seconds, file size and object graph restrictions are set to Int32.MaxValue, SecurityMode: None.
    */
    public class EchoServiceHostConfigurator : IServiceHostConfigurator<EchoService>
    {
        public void Configure(IServiceHost<EchoService> host)
        {
            /* Adds a BasicHttp endpoint on port 9999 with suffix: "basic". Uses default configuration. */
            host.AddBasicHttpEndpoint(typeof(IEchoService), 9999, configureBinding: binding => binding.UseDefaults());

            /* Adds a WSHttp endpoint on port 9999 with suffix: "ws". Uses default configuration. */
            host.AddWSHttpEndpoint(typeof(IEchoService), 9999, configureBinding: binding => binding.UseDefaults());

            /* Adds a WebHttp endpoint on port 9999 with suffix: "web". Uses default configuration. */
            host.AddWebHttpEndpoint(typeof(IEchoService), 9999, configureBinding: binding => binding.UseDefaults());

            /* Adds a NamedPipe endpoint with suffix: "echo". Uses default configuration. */
            host.AddNamedPipeEndpoint(typeof(IEchoService), "echo", configureBinding: binding => binding.UseDefaults());

            /* Adds a NetTcp endpoint on port 9998. Uses default configuration. */
            host.AddNetTcpEndpoint(typeof(IEchoService), 9998, configureBinding: binding => binding.UseDefaults());

            /* Adds service metadata on the same port of the first HTTP endpoint, otherwise the port must be provided */
            host.AddMetadata();

            /* 
                Adds support for WS-Discovery to a registry hosted on a NetTcp binding.
                The service is announced every 5 seconds. Also, the binding used by the client is set to use the default customization.
            */
            host.AddNetTcpDiscovery(new Uri("net.tcp://localhost:8001/Announcement"), TimeSpan.FromSeconds(5), binding => binding.UseDefaults());
        }
    }
}