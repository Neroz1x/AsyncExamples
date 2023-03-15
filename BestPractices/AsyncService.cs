namespace BestPractices
{
    public sealed class AsyncService
    {
        private readonly Dictionary<int, int> _cache = new Dictionary<int, int>(){ { 1, 1 } };

        /// <summary>
        /// Async all the way down
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetStringAsync()
        {
            var myString = "Sub";
            return myString += await GetStringFromFileAsync("test");
        }

        // It's better to remove async/await here, but it looks like this so as not to mix practices
        private async Task<string> GetStringFromFileAsync(string path)
            => await Task.FromResult("substring");

        /// <summary>
        /// Use CancellationToken whenever possible
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1_000, cancellationToken);
            return 1;
        }

        public async Task ExceptionExampleAsync()
        {
            await Task.Delay(1_000);
            throw new Exception("Task failed");
        }

        /// <summary>
        /// Async void example. Exceptions thrown into async void will most likely break the app down
        /// </summary>
        public async void BackgroundTaskAsync()
        {
            await Task.Delay(1_000);
            throw new Exception("Background task failed");
        }

        /// <summary>
        /// Could be usefull for Long Running Tasks
        /// </summary>
        /// <returns></returns>
        public async Task LongRunningTaskAsync()
            => await Task.Factory.StartNew(() => true, TaskCreationOptions.LongRunning);

        /// <summary>
        /// Use value tasks if it may return not only Task<T>, but T
        /// </summary>
        /// <param name="useChache"></param>
        /// <returns></returns>
        public async ValueTask<int> ValueTaskExampleAsync(bool useChache)
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

        /// <summary>
        /// Return Task<T> if there is no point to await the Task
        /// </summary>
        /// <returns></returns>
        public Task<string> DoNotReturnAwaitAsync()
            => Task.FromResult("DoNotReturnAwait");

        public async Task<string> ReadFromThreeFilesAsync(string path1, string path2, string path3)
        {
            var fileOne = await GetStringFromFileAsync(path1);
            var fileTwo = await GetStringFromFileAsync(path2);
            var fileThree = await GetStringFromFileAsync(path3);
            return fileOne + fileTwo + fileThree;
        }

        //public async Task<string> ReadFromThreeFilesAsync(string path1, string path2, string path3)
        //{
        //    var taskOne = GetStringFromFileAsync(path1);
        //    var taskTwo = GetStringFromFileAsync(path2);
        //    var taskThree = GetStringFromFileAsync(path3);

        //    await Task.WhenAll(taskOne, taskTwo, taskThree);

        //}
    }
}
