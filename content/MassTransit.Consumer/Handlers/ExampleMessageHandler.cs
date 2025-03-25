namespace MassTransit.Publisher.Handlers;

public class ExampleMessageHandler<ExampleMessage> : IMessageHandler<ExampleMessage>
        where ExampleMessage : class
{
    public Task HandleAsync(ExampleMessage message)
    {
        throw new NotImplementedException();
    }
}
