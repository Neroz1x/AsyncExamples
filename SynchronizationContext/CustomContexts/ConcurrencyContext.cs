using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationContext.CustomContexts
{
    internal sealed class ConcurrencySynchronizationContext : SynchronizationContext
    {
        private readonly SemaphoreSlim _semaphore;

        public ConcurrencySynchronizationContext(int maxConcurrencyLevel) =>
            _semaphore = new SemaphoreSlim(maxConcurrencyLevel);

        public override void Post(SendOrPostCallback d, object state) =>
            _semaphore.WaitAsync().ContinueWith(delegate
            {
                try { d(state); } finally { _semaphore.Release(); }
            }, default, TaskContinuationOptions.None, TaskScheduler.Default);

        public override void Send(SendOrPostCallback d, object state)
        {
            _semaphore.Wait();
            try { d(state); } finally { _semaphore.Release(); }
        }
    }
}
