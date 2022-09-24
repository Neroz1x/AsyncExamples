using System;

namespace APM
{
    class Program
    {
        static readonly int[] values = new int[] { 1, 2, 3, 4, 5 };

        static void Main(string[] args)
        {
            APMExampleHelper.BlockMainThread(values);

            APMExampleHelper.WaitTillCompletion(values);
            
            APMExampleHelper.ExecuteCallbackOnComplete(values);

            Console.ReadKey();
        }
    }
}
