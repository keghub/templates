using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EMG
{
    public class JobParameters
    {
        public string FirstArgument { get; set; }

        public int SecondArgument { get; set; }

        public string OptionalArgument { get; set; }
    }

    public class JobProcessor
    {
        private readonly ILogger<JobProcessor> _logger;

        public JobProcessor(ILogger<JobProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(JobParameters parameters)
        {
            try
            {
                await Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error in HandleAsync for {nameof(JobProcessor)}");
            }
        }
    }
}