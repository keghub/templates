using System;
using System.Threading.Tasks;
using EMG.Lambda.LocalRunner;

namespace LambdaRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /* TODO */
            // 1) Add a project reference to the project containing your Lambda function
            // 2) Uncomment one of the templates below according to your needs
            // 3) Customize it so that it builds (input type, output type)
            // 4) Make sure you are using the correct serializer
            // 5) Make sure the port is not conflicting with other applications. If needed, customize it

            /* Asynchronous function, accepting a string, returning a string, using default port (5000) and default serializer */
            // await LambdaRunner.Create()
            //                   .Receives<string>()
            //                   .Returns<string>()
            //                   .UsesAsyncFunction<Function>((function, input, context) => function.FunctionHandlerAsync(input, context))
            //                   .Build()
            //                   .RunAsync();

            /* Synchronous function, accepting a string, returning a string, using default port (5000) and custom serializer */
            // await LambdaRunner.Create()
            //                   .UseSerializer(() => new MyCustomSerializer())
            //                   .Receives<string>()
            //                   .Returns<string>()
            //                   .UsesFunction<Function>((function, input, context) => function.FunctionHandler(input, context))
            //                   .Build()
            //                   .RunAsync();

            /* Asynchronous function, accepting a string with no result, using a custom port and default serializer */
            // await LambdaRunner.Create()
            //                   .UsePort(5001)
            //                   .Receives<string>()
            //                   .UsesAsyncFunctionWithNoResult<Function>((function, input, context) => function.FunctionHandlerAsync(input, context))
            //                   .Build()
            //                   .RunAsync();

            /* Synchronous function, accepting a string with no result, using a custom port and custom serializer */
            // await LambdaRunner.Create()
            //                   .UsePort(5001)
            //                   .UseSerializer(() => new MyCustomSerializer())
            //                   .Receives<string>()
            //                   .UsesFunctionWithNoResult<Function>((function, input, context) => function.FunctionHandler(input, context))
            //                   .Build()
            //                   .RunAsync();
        }
    }
}
