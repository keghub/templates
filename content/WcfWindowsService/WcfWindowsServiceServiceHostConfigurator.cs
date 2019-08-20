using System;
using EMG.Wcf;
using EMG.Wcf.Extensions;

namespace EMG.WcfWindowsService
{
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
    public class WcfWindowsServiceServiceHostConfigurator : IServiceHostConfigurator<WcfWindowsService>
    {
        private readonly WcfHostingOptions _options;

        public WcfWindowsServiceServiceHostConfigurator(WcfHostingOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(IServiceHost<WcfWindowsService> host)
        {
            /*
                You can customize WcfHostingOptions to receive more settings.
            */

            /* Adds a BasicHttp endpoint on port 9999 with suffix: "basic". Uses default configuration. */
            //host.AddBasicHttpEndpoint(typeof(IWcfWindowsService), 9999, hostname: _options.AnnouncedHost, configureBinding: binding => binding.UseDefaults());

            /* Adds a WSHttp endpoint on port 9999 with suffix: "ws". Uses default configuration. */
            //host.AddWSHttpEndpoint(typeof(IWcfWindowsService), 9999, hostname: _options.AnnouncedHost, configureBinding: binding => binding.UseDefaults());

            /* Adds a WebHttp endpoint on port 9999 with suffix: "web". Uses default configuration. */
            //host.AddWebHttpEndpoint(typeof(IWcfWindowsService), 9999, hostname: _options.AnnouncedHost, configureBinding: binding => binding.UseDefaults());

            /* Adds a NamedPipe endpoint with suffix: "echo". Uses default configuration. */
            //host.AddNamedPipeEndpoint(typeof(IWcfWindowsService), "echo", configureBinding: binding => binding.UseDefaults());

            /* Adds a NetTcp endpoint on port 9998. Uses default configuration. */
            host.AddNetTcpEndpoint(typeof(IWcfWindowsService), 9998, hostname: _options.AnnouncedHostName, configureBinding: binding => binding.UseDefaults());

            /* Adds service metadata on the same port of the first HTTP endpoint, otherwise the port must be provided */
            
            /* Use this if you have an HTTP endpoint */
            //host.AddMetadata();
            
            /* Use this if you don't have any HTTP endpoint */
            //host.AddMetadata(9999); // the port number must differ

            /* 
                Adds support for WS-Discovery to a registry hosted on a NetTcp binding.
                The service is announced every 5 seconds. Also, the binding used by the client is set to use the default customization.
            */
            host.AddNetTcpDiscovery(new Uri("net.tcp://localhost:8001/Announcement"), TimeSpan.FromSeconds(5), binding => binding.UseDefaults());
        }
    }

    public class WcfHostingOptions
    {
        public string AnnouncedHostName { get; set; }
    }
}