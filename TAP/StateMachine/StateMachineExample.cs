using System;
using System.Threading.Tasks;

namespace TAP.StateMachine
{
    public class StateMachineExample
    {
        public static async Task StateMachineExampleAsync(int param)
        {
            Console.WriteLine("State Machine");
            Console.WriteLine("Code before first await");
            var firstTaskResult = await Task.FromResult(param);
            Console.WriteLine("Code after the first await");

            await Task.Delay(2000).ConfigureAwait(false);
            Console.WriteLine("Code after the second await");

            await Task.Delay(3000);
            Console.WriteLine("Code after the third await");

            Console.WriteLine(firstTaskResult);

            Console.WriteLine();
        }
    }
}
