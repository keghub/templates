using System;
using EMG.Wcf;
using Nybus;
using Nybus.Utils;

namespace EMG.WcfWindowsServiceWithNybus
{

    /*
        Class used to start Nybus bus and WCF service host when the Windows Service starts.
    */ 
    public class ServiceHostWrapper<T> where T : class
    {
        private readonly IBus _bus;
        private readonly WcfServiceHost<T> _serviceHost;

        public ServiceHostWrapper(IBus bus, WcfServiceHost<T> serviceHost)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _serviceHost = serviceHost ?? throw new ArgumentNullException(nameof(serviceHost));
        }

        public void Start()
        {
            _bus.Start().WaitAndUnwrapException();
            _serviceHost.Start();
        }

        public void Stop()
        {
            _serviceHost.Stop();
            _bus.Stop().WaitAndUnwrapException();
        }
    }
}