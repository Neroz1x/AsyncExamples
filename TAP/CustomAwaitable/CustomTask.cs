using System;
using System.Threading.Tasks;

namespace TAP
{
    public sealed class CustomTask
    {
        internal async Task SomeTaskAsync()
        {
            Console.WriteLine("Custom task started");
            await Task.Delay(1000);
            Console.WriteLine("Custom task finished");
        }
    }

}
