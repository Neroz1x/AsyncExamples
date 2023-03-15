namespace BestPractices
{
    public class AsyncConstructor
    {
        public AsyncConstructor() 
        {
            Something();
            Task.Run(() => { throw new Exception(); });
        }

        private async Task Something()
        {
            await Task.Delay(1_000);
        }
    }
}
