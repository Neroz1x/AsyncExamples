using System;
using System.Threading;

namespace Threads
{
    public sealed class ThreadHelper
    {
        public static void RunTaskOnThreadPool()
        {
            Console.WriteLine($"{GetTimeNow()}: Main thread started {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    Task,
                    i
                );
            }
        }

        private static void Task(object state)
        {
            Console.WriteLine($"{GetTimeNow()}: {state} Thread from Pool {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1_000);
        }

        private static string GetTimeNow()
            => DateTime.Now.ToLongTimeString();
    }
}
