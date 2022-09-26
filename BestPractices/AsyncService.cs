namespace BestPractices
{
    public sealed class AsyncService
    {
        private readonly Dictionary<int, int> _cache = new Dictionary<int, int>(){ { 1, 1 } };
        
        // Async all the way down
        public async Task<string> GetStringAsync()
        {
            var myString = "Sub";
            return myString += await GetStringFromFileAsync();
        }
        private async Task<string> GetStringFromFileAsync()
            => await Task.FromResult("substring");

        // Use CancellationToken whenever possible
        public async Task<int> GetIntAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1_000, cancellationToken);
            return 1;
        }

        public async Task ExceptionExample()
        {
            await Task.Delay(1_000);
            throw new Exception("Task failed");
        }

        // Exceptions thrown into async void will most likely break the app down
        public async void BackgroundTask()
        {
            await Task.Delay(1_000);
            throw new Exception("Background task failed");
        }
        
        // Could be usefull for Long Running Tasks
        public async Task LongRunningTask()
            => await Task.Factory.StartNew(() => true, TaskCreationOptions.LongRunning);

        // Use value tasks if it may return not only Task<T>, but T
        public async ValueTask<int> ValueTaskExample(bool useChache)
        {
            if (useChache)
            {
                return _cache[1];
            }
            return await Task.FromResult(1);
        }

        // Better to use code below, unless try/catch or using
        // Reasons: Memory, Switching Sync contexts
        //public async Task<string> DoNotReturnAwait()
        //    => await Task.FromResult("DoNotReturnAwait");

        public Task<string> DoNotReturnAwait()
            => Task.FromResult("DoNotReturnAwait");
    }
}
