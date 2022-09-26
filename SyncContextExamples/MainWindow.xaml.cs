using SyncContext.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SyncContext
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

        // Deadlock example
        private void OnDeadlock(object sender, RoutedEventArgs e)
        {
            var result = MyTaskAsync().Result;
            Label.Content = "Completed";
        }

        // Use UIcontrol.Dispatcher.Invoke to perform callback on UI Thread
        private async void OnDispatcherInvokeAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(null);
            var result = await MyTaskAsync();
            Label.Dispatcher.Invoke(() => Label.Content = "Completed");
        }

        // Pass UI Synchronization Context to Post Continuation on it
        private async void OnFixedCustomContextAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(
                new MaxConcurrencySynchronizationContext(1, SynchronizationContext.Current));
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }

        // Label.Content = "Completed" will throw exception because UI context is lost
        private async void OnCustomContextAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(new MaxConcurrencySynchronizationContext(1));
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }

        // Label.Content = "Completed" will throw exception because UI context is lost
        private async void OnContextNullAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(null);
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }

        private async void OnWorksAsync(object sender, RoutedEventArgs e)
        {
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }

        private async Task<int> MyTaskAsync()
        {
            Label.Content = "Started";
            await Task.Delay(1_000);
            // Use this to prevent Deadlock when .Wait() on UI Thread or if there are Context Limitations
            //await Task.Delay(1_000);
            return 1;
        }

    }
}
