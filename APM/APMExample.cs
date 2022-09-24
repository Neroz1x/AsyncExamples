using System;
using System.Threading;

namespace APM
{
    class APMExample
    {
        // The method to be executed asynchronously.
        public int LongTaskMethod(int[] values)
        {
            var result = 0;
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Method running at the thread: {Thread.CurrentThread.ManagedThreadId}");
            foreach (var item in values)
            {
                Thread.Sleep(1000);
                result += item;
            }
            return result;
        }
    }
    
    // Delegate with the same signature
    public delegate int AsyncLongTaskCaller(int[] values);
}
