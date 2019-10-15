using System;
using System.ServiceModel;
using Microsoft.Extensions.Logging;

namespace EMG.WindowsService
{
#if (WCF)
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        string Echo(string message);
    }

    public class TestService : ITestService
    {
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Echo(string message)
        {
            _logger.LogInformation("Received {MESSAGE}", message);
            return message;
        }
    }
#endif
}
