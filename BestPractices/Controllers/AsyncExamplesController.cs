using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("BackgroundTask")]
        public IActionResult BackgroundTask()
        {
            _asyncService.BackgroundTask();
            
            return Ok();
        }

        [HttpGet("TaskWait")]
        public IActionResult TaskWait()
        {
            _asyncService.ExceptionExample().Wait();

            return Ok();
        }

        [HttpGet("TaskGetResult")]
        public IActionResult TaskGetResult()
        {
            _asyncService.ExceptionExample().GetAwaiter().GetResult();

            return Ok();
        }

        [HttpGet("DoNotReturnAwait")]
        public async Task<IActionResult> DoNotReturnAwait()
            => Ok(await _asyncService.DoNotReturnAwait());
    }
}