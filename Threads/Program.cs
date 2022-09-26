using System;

namespace Threads
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadHelper.RunTaskOnAThread();
            ThreadHelper.RunTaskOnThreadPool();

            Console.ReadKey();
        }
    }
}
