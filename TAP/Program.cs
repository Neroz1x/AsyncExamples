using System;
using System.Threading.Tasks;

namespace TAP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Custom awaitable example
            await TAPHelper.CustomAwaitableExample();

            // await examples
            await TAPHelper.OperationCompletedOnAwaitAsync();
            await TAPHelper.OperationNotCompletedOnAwaitAsync();

            // State machine example
            //await StateMachineExample.StateMachineExampleAsync(1000);

            // TaskCompletionSource
            //await TAPHelper.CompletionSource();

            // Start new async
            //await TAPHelper.TaskExample();

            Console.ReadKey();
        }
    }
}
