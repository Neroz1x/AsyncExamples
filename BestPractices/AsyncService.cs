using System.Diagnostics;

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
        {
            await Task.Delay(1_000);
            return $"substring {path}";
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
        /// Use CancellationToken whenever possible
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> GetIntAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1_000_000, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1_000);
            }

            return 1;
        }

        public async Task ExceptionExampleAsync()
        {
            await Task.Delay(1_000);
            throw new Exception("Task failed");
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
        public async Task<string> DoNotReturnAwaitAsync()
            => await Task.FromResult("DoNotReturnAwait");


        public async Task<string> ReadFromThreeFilesAsync(string path1, string path2, string path3)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var fileOne = await GetStringFromFileAsync(path1);
            var fileTwo = await GetStringFromFileAsync(path2);
            var fileThree = await GetStringFromFileAsync(path3);

            stopwatch.Stop();

            return $"{fileOne} {fileTwo} {fileThree} time elepsed: {stopwatch.Elapsed}";
        }

        public async Task<string> ReadFromThreeFilesWAsync(string path1, string path2, string path3)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var fileOne = GetStringFromFileAsync(path1);
            var fileTwo = GetStringFromFileAsync(path2);
            var fileThree = GetStringFromFileAsync(path3);
            await Task.WhenAll(fileOne, fileThree, fileTwo);
            stopwatch.Stop();

            return $"{fileOne.Result} {fileTwo.Result} {fileThree.Result} time elepsed: {stopwatch.Elapsed}";
        }
    }
}
