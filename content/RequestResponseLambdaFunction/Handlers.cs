using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace EMG
{
    public interface IRequestResponseHandler<in TInput, TOutput>
    {
        Task<TOutput> HandleAsync(TInput input, ILambdaContext context);
    }
}
