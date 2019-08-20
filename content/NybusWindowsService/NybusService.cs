using System;
using Nybus;
using Nybus.Utils;

namespace EMG.NybusWindowsService
{
    public class NybusService
    {
        private readonly IBus _bus;

        public NybusService(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public void Start() => _bus.Start().WaitAndUnwrapException();

        public void Stop() => _bus.Stop().WaitAndUnwrapException();
    }
}