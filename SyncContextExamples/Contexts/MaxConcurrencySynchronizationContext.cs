using System.Threading;
using System.Threading.Tasks;

namespace SyncContext.Contexts
{
    // Example of custom Synchronization context 
    internal sealed class MaxConcurrencySynchronizationContext : SynchronizationContext
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly SynchronizationContext _synchronizationContext;

        public MaxConcurrencySynchronizationContext(int maxConcurrencyLevel, 
            SynchronizationContext synchronizationContext = null)
        {
            _semaphore = new SemaphoreSlim(maxConcurrencyLevel);
            _synchronizationContext = synchronizationContext;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            if (_synchronizationContext is null)
            {
                _semaphore.WaitAsync().ContinueWith(delegate
                {
                    try { d(state); } finally { _semaphore.Release(); }
                }, default, TaskContinuationOptions.None, TaskScheduler.Default);
            }
            else
            {   
                _synchronizationContext.Post(d, state);
            }
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            _semaphore.Wait();
            try { d(state); } finally { _semaphore.Release(); }
        }
    }
}
