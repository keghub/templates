using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace EMG
{
    public interface IEventHandler<in TInput>
    {
        Task HandleAsync(TInput input, ILambdaContext context);
    }
}
