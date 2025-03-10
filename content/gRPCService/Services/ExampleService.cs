using EMG.gRPCService;
using Grpc.Core;
using static EMG.gRPCService.ExampleService;

namespace gRPCService.Services
{
    public class ExampleService : ExampleServiceBase
    {
        private readonly ILogger<ExampleService> _logger;

        public ExampleService(ILogger<ExampleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task<ExampleResponse> SendMessage(ExampleRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ExampleResponse
            {
                Message = "Success " + request.Name
            });
        }
    }
}
