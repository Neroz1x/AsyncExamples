namespace BestPractices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AsyncExamplesController : ControllerBase
    {
        private readonly AsyncService _asyncService;

        public AsyncExamplesController(AsyncService asyncService)
        {
            _asyncService = asyncService;
        }

        [HttpGet("AsyncTillTheEnd")]
        public async Task<IActionResult> AsyncTillTheEnd()
            => Ok(await _asyncService.GetStringAsync());

        // Start backgound async void
        [HttpGet("BackgroundTask")]
        public IActionResult BackgroundTask()
        {
            _asyncService.BackgroundTask();
            
            return Ok();
        }

        // Show Task.Wait() exception Stacktrace
        [HttpGet("TaskWait")]
        public IActionResult TaskWait()
        {
            _asyncService.ExceptionExample().Wait();

            return Ok();
        }

        // Show Task.GetAwaiter().GetResult() exception Stacktrace
        [HttpGet("TaskGetResult")]
        public IActionResult TaskGetResult()
        {
            _asyncService.ExceptionExample().GetAwaiter().GetResult();

            return Ok();
        }

        // await Task here so as not to do unnecessary await deepper
        [HttpGet("DoNotReturnAwait")]
        public async Task<IActionResult> DoNotReturnAwait()
            => Ok(await _asyncService.DoNotReturnAwait());
    }
}