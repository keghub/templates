using Amazon.Lambda.Core;
using EMG.Extensions.Logging.Loggly;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EMG
{
    /// <summary>
    /// Handles any event type as raw JSON. Once the trigger is known, replace <see cref="JsonElement"/>
    /// with a concrete event type (e.g. SQSEvent, SNSEvent, S3Event, ScheduledEvent) and update the
    /// handler registration in Function.cs.
    /// </summary>
    public class JsonEventHandler : IEventHandler<JsonElement>
    {
        private readonly ILogger<JsonEventHandler> _logger;
        private readonly ILogglyProcessor _logglyProcessor;

        public JsonEventHandler(ILogger<JsonEventHandler> logger, ILogglyProcessor logglyProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logglyProcessor = logglyProcessor ?? throw new ArgumentNullException(nameof(logglyProcessor));
        }

        public Task HandleAsync(JsonElement input, ILambdaContext context)
        {
            try
            {
                _logger.LogDebug("Received event: {Event}", input.GetRawText());

                return Task.CompletedTask;
            }
            finally
            {
                _logglyProcessor.FlushMessages(); // ensure all logs are sent to Loggly before the application terminates.
            }
        }
    }
}
