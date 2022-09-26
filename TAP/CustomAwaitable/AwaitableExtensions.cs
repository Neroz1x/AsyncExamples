using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TAP
{
    // Extension for Custom Awaitable type
    public static class AwaitableExtensions
    {
        public static CustomAwaiter GetAwaiter(this CustomTask task) => new CustomAwaiter(task);

        public class CustomAwaiter : INotifyCompletion
        {
            private readonly Task _task;

            public CustomAwaiter(CustomTask customTask)
            {
                _task = customTask.SomeTaskAsync();
            }

            public bool IsCompleted => _task.IsCompleted;
            public void GetResult() => _task.GetAwaiter().GetResult();
            public void OnCompleted(Action continuation) => continuation();
        }
    }
}
