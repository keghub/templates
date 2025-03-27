namespace MassTransit.Publisher.Handlers;

public interface IMessageHandler<TMessage>
        where TMessage : class
{
    Task HandleAsync(TMessage message);
}
