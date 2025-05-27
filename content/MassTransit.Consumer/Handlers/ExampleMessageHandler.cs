using MassTransit.Publisher.Messages;

namespace MassTransit.Publisher.Handlers;

public class ExampleMessageHandler : IMessageHandler<ExampleMessage>
{
    public Task HandleAsync(ExampleMessage message)
    {
        throw new NotImplementedException();
    }
}
