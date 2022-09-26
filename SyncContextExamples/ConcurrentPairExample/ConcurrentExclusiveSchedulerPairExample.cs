using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Controls;

namespace SyncContext
{
    internal sealed class ConcurrentExclusiveSchedulerPairExample
    {
        internal void Example()
        {
            CheckBox checkBox1 = new CheckBox();
            CheckBox checkBox2 = new CheckBox();
            CheckBox checkBox3 = new CheckBox();
            CheckBox checkBox4 = new CheckBox();

            // Broadcasts values to an ActionBlock<int> object that is associated
            // with each check box.
            BroadcastBlock<int> broadcaster = new BroadcastBlock<int>(null);

            // Create an ActionBlock<CheckBox> object that toggles the state
            // of CheckBox objects.
            // Specifying the current synchronization context enables the
            // action to run on the user-interface thread.
            var toggleCheckBox = new ActionBlock<CheckBox>(checkBox =>
            {
                checkBox.IsChecked = !checkBox.IsChecked;
            });

            // Create a ConcurrentExclusiveSchedulerPair object.
            // Readers will run on the concurrent part of the scheduler pair.
            // The writer will run on the exclusive part of the scheduler pair.
            var taskSchedulerPair = new ConcurrentExclusiveSchedulerPair();

            // Create an ActionBlock<int> object for each reader CheckBox object.
            // Each ActionBlock<int> object represents an action that can read 
            // from a resource in parallel to other readers.
            // Specifying the concurrent part of the scheduler pair enables the 
            // reader to run in parallel to other actions that are managed by 
            // that scheduler.
            var readerActions =
               from checkBox in new CheckBox[] { checkBox1, checkBox2, checkBox3 }
               select new ActionBlock<int>(milliseconds =>
               {
                   // Toggle the check box to the checked state.
                   toggleCheckBox.Post(checkBox);

                   // Perform the read action. For demonstration, suspend the current
                   // thread to simulate a lengthy read operation.
                   Thread.Sleep(milliseconds);

                   // Toggle the check box to the unchecked state.
                   toggleCheckBox.Post(checkBox);
               },
               new ExecutionDataflowBlockOptions
               {
                   TaskScheduler = taskSchedulerPair.ConcurrentScheduler
               });

            // Create an ActionBlock<int> object for the writer CheckBox object.
            // This ActionBlock<int> object represents an action that writes to 
            // a resource, but cannot run in parallel to readers.
            // Specifying the exclusive part of the scheduler pair enables the 
            // writer to run in exclusively with respect to other actions that are 
            // managed by the scheduler pair.
            var writerAction = new ActionBlock<int>(milliseconds =>
            {
                // Toggle the check box to the checked state.
                toggleCheckBox.Post(checkBox4);

                // Perform the write action. For demonstration, suspend the current
                // thread to simulate a lengthy write operation.
                Thread.Sleep(milliseconds);

                // Toggle the check box to the unchecked state.
                toggleCheckBox.Post(checkBox4);
            },
            new ExecutionDataflowBlockOptions
            {
                TaskScheduler = taskSchedulerPair.ExclusiveScheduler
            });

            // Link the broadcaster to each reader and writer block.
            // The BroadcastBlock<T> class propagates values that it 
            // receives to all connected targets.
            foreach (var readerAction in readerActions)
            {
                broadcaster.LinkTo(readerAction);
            }
            broadcaster.LinkTo(writerAction);
        }
    }
}
