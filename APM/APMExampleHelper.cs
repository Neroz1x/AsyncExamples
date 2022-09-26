using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace APM
{
    internal sealed class APMExampleHelper
    {
        /// <summary>
        /// Example of APM in FileStream's BeginRead/EndRead
        /// </summary>
        /// <param name="values"></param>
        internal static void ReadExample(int[] values)
        {
            Console.WriteLine("Read example");

            var currentFileStream = new FileStream("Foo.txt", FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 8, true);
            var buffer = new byte[currentFileStream.Length];
            IAsyncResult result = currentFileStream.BeginRead(
                buffer, 
                0, 
                buffer.Length, 
                (ar) => Console.WriteLine("Completed"), 
                currentFileStream
            );
            currentFileStream.EndRead(result);
        }

        /// <summary>
        /// Block main thread by executing EndInvoke
        /// </summary>
        /// <param name="values"></param>
        internal static void BlockMainThread(int[] values)
        {
            Console.WriteLine("Block thread example");
            // Create an instance of the test class
            var apm = new APMExample();

            // Create the delegate
            var caller = new AsyncLongTaskCaller(apm.LongTaskMethod);

            // Initiate the asychronous call
            Console.WriteLine($"{GetTimeNow()}: Started long operation. Thread: {Thread.CurrentThread.ManagedThreadId}");
            IAsyncResult result = caller.BeginInvoke(values, null, null);

            Console.WriteLine($"{GetTimeNow()}: Block current thread by executing EndInvoke.");
            var operationResult = caller.EndInvoke(result);
            Console.WriteLine($"{GetTimeNow()}: Main thread running.");

            Console.WriteLine($"{GetTimeNow()}: The result is: {operationResult}. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
        }

        /// <summary>
        /// Check IAsuncResult's IsCompleted to wait for result
        /// </summary>
        /// <param name="values"></param>
        internal static void WaitTillCompletion(int[] values)
        {
            Console.WriteLine("Wait example");
            // Create an instance of the test class
            var apm = new APMExample();

            // Create the delegate
            var caller = new AsyncLongTaskCaller(apm.LongTaskMethod);

            // Initiate the asychronous call
            Console.WriteLine($"{GetTimeNow()}: Started long operation. Thread: {Thread.CurrentThread.ManagedThreadId}");
            IAsyncResult result = caller.BeginInvoke(values, null, null);

            // Wait completion
            while (result.IsCompleted == false)
            {
                Thread.Sleep(250);
                Console.Write($"");
            }
            Console.WriteLine($"{GetTimeNow()}: Method complete.");
            // Call EndInvoke to retrieve the results.
            Console.WriteLine($"{GetTimeNow()}: The result is: {caller.EndInvoke(result)}. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
        }

        /// <summary>
        /// Execute callback after Async Action is completed
        /// </summary>
        /// <param name="values"></param>
        internal static void ExecuteCallbackOnComplete(int[] values)
        {
            Console.WriteLine("Callback example");
            // Create an instance of the test class
            var apm = new APMExample();

            // Create the delegate
            var caller = new AsyncLongTaskCaller(apm.LongTaskMethod);

            // Initiate the asychronous call
            Console.WriteLine($"{GetTimeNow()}: Started long operation. Thread: {Thread.CurrentThread.ManagedThreadId}");
            var stateInfo = "{0}: Callback executed. The result is {1}. Thread: {2}";
            IAsyncResult result = caller.BeginInvoke(values, CallBack, stateInfo);

            Console.WriteLine($"{GetTimeNow()}: Method continues executing.");
        }

        /// <summary>
        /// Callback to be execute on completed 
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void CallBack(IAsyncResult asyncResult)
        {
            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)asyncResult;
            AsyncLongTaskCaller caller = (AsyncLongTaskCaller)result.AsyncDelegate;

            // Retrieve state info
            var format = result.AsyncState as string;

            // Call EndInvoke to retrieve the results.
            Console.WriteLine(format, GetTimeNow(), caller.EndInvoke(asyncResult), Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        private static string GetTimeNow()
            => DateTime.Now.ToLongTimeString();
    }
}
