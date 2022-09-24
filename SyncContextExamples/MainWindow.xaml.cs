using SyncContextExamples.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SyncContextExamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDeadlock(object sender, RoutedEventArgs e)
        {
            var result = MyTask().Result;
            Label.Content = "Completed";
        }

        private async void OnConfigureAwait(object sender, RoutedEventArgs e)
        {
            var result = await MyTask().ConfigureAwait(false);
            Label.Dispatcher.Invoke(() => Label.Content = "Update from other thread");
            Label.Content = "Completed";
        }

        private async void OnCustomContext(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(new MaxConcurrencySynchronizationContext(1));
            var result = await MyTask();
            Label.Content = "Completed";
        }

        private async void OnContextNull (object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(null);
            var result = await MyTask();
            Label.Content = "Completed";
        }

        private async void OnWorks(object sender, RoutedEventArgs e)
        {
            var result = await MyTask();
            Label.Content = "Completed";
        }

        private async Task<int> MyTask()
        {
            Label.Content = "Started";
            await Task.Delay(1_000);
            return await Task.FromResult(1);
        }

    }
}
