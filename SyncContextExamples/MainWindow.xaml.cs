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
            var result = MyTaskAsync().Result;
            Label.Content = "Completed";
        }

        private async void OnDispatcherInvokeAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(null);
            var result = await MyTaskAsync();
            Label.Dispatcher.Invoke(() => Label.Content = "Completed");
        }

        private async void OnFixedCustomContextAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(
                new MaxConcurrencySynchronizationContext(1, SynchronizationContext.Current));
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }
        
        private async void OnCustomContextAsync(object sender, RoutedEventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(new MaxConcurrencySynchronizationContext(1));
            var result = await MyTaskAsync();
            Label.Content = "Completed";
        }

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
            await Task.Delay(1_000).ConfigureAwait(false);
            return 1;
        }

    }
}
