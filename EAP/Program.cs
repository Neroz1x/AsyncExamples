using System;

namespace EAP
{
    class Program
    {
        static readonly int[] values = new int[] { 1, 2, 3, 4, 5 };

        static void Main(string[] args)
        {
            var eap = new EAPExample();
            eap.LongTaskCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Console.WriteLine($"{GetTimeNow()}: Long task ({e.UserState}) canceled");
                    return;
                }
                Console.WriteLine($"{GetTimeNow()}: Long task completed: result is {e.Result}");
            };

            eap.LongTaskProgressChanged += (s, e) =>
            {
                Console.WriteLine($"{GetTimeNow()}: Long task progress: {e.ProgressPercentage} %");
            };

            // Run Synchronously
            Console.WriteLine($"{GetTimeNow()}: Before long sync task started");
            var result = eap.LongTask(values);
            Console.WriteLine($"{GetTimeNow()}: After long sync task ended");
            Console.WriteLine($"{GetTimeNow()}: Result is {result}");
            Console.WriteLine();

            // Run Asynchronously
            Console.WriteLine($"{GetTimeNow()}: Before long async task started");
            eap.LongTaskAsync(values);
            Console.WriteLine($"{GetTimeNow()}: After long async task");

            // Run Asynchronously and cancel
            var guid = Guid.NewGuid();
            Console.WriteLine($"{GetTimeNow()}: Before long async task ({guid}) started");
            eap.LongTaskAsync(values, guid);
            Console.WriteLine($"{GetTimeNow()}: Long async task cancelled ({guid})");
            eap.CancelAsync(guid);
            Console.WriteLine($"{GetTimeNow()}: After long async task ({guid})");
            Console.WriteLine();

            Console.ReadKey();
        }

        private static string GetTimeNow() 
            => DateTime.Now.ToLongTimeString();
    }
}
