using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TAP
{
    public sealed class TAPHelper
    {
        // Use custom awaitable example
        public static async Task CustomAwaitableExample()
        {
            var task = new CustomTask();
            await task;
        }


        public static async Task OperationCompletedOnAwaitAsync()
        {
            Console.WriteLine("Async operation was completed before await");
            Console.WriteLine($"{GetTimeNow()}: Code before async operation. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Delay(1000);
            Console.WriteLine($"{GetTimeNow()}: Code after async operation call. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            await task;
            Console.WriteLine($"{GetTimeNow()}: Code after await. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
        }

        public static async Task OperationNotCompletedOnAwaitAsync()
        {
            Console.WriteLine("Async operation was not completed before await");
            Console.WriteLine($"{GetTimeNow()}: Code before async operation. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Delay(1000);
            Console.WriteLine($"{GetTimeNow()}: Code after async operation call. Thread: {Thread.CurrentThread.ManagedThreadId}");
            await task;
            Console.WriteLine($"{GetTimeNow()}: Code after await. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
        }

        public static Task<string> CompletionSource()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Directory.GetCurrentDirectory(),
                NotifyFilter = NotifyFilters.LastAccess,
                EnableRaisingEvents = true
            };
            watcher.Changed += (o, e) => tcs.SetResult(e.FullPath);
            return tcs.Task;
        }

        public static async Task TaskExample()
        {
            Console.WriteLine($"{GetTimeNow()}: Main Thread: {Thread.CurrentThread.ManagedThreadId}");
            
            Task t1 = new Task(() => Console.WriteLine($"{GetTimeNow()}: Async operation. Thread: {Thread.CurrentThread.ManagedThreadId}"));
            t1.Start();

            await Task.Factory.StartNew(() => Console.WriteLine($"{GetTimeNow()}: Async operation. Thread: {Thread.CurrentThread.ManagedThreadId}"));

            await Task.Run(() => Console.WriteLine($"{GetTimeNow()}: Async operation. Thread: {Thread.CurrentThread.ManagedThreadId}"));
        }

        private static string GetTimeNow()
            => DateTime.Now.ToLongTimeString();
    }
}
