using System;

namespace Threads
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadHelper.RunTaskOnThreadPool();

            Console.ReadKey();
        }
    }
}
