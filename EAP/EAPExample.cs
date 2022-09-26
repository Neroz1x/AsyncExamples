using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace EAP
{
    public sealed class EAPExample
    {
        // Used to notify cliend about long operation completion
        public delegate void LongTaskCompletedEventHandler(object sender, LongTaskCompletedEventArgs e);
        
        // Peroform on operation completion
        public event LongTaskCompletedEventHandler LongTaskCompleted;
        
        // Used to notify cliend about the operation's progress
        public delegate void LongTaskProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        
        // Peroform on operation completion
        public event LongTaskProgressChangedEventHandler LongTaskProgressChanged;
        
        // Sync long operation
        public int LongTask(int[] values)
        {
            var result = 0;
            foreach (var item in values)
            {
                Thread.Sleep(1000);
                result += item;
            }
            return result;
        }
        
        // Async long operation
        public void LongTaskAsync(int[] values)
        {
            var guid = Guid.NewGuid();
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(guid);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (_userStateToLifetime.SyncRoot)
            {
                if (_userStateToLifetime.Contains(guid))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                _userStateToLifetime[guid] = asyncOp;
            }

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(CalculateWorker);
            workerDelegate.BeginInvoke(
                values,
                asyncOp,
                null,
                null);
        }
        
        // Async long operation
        public void LongTaskAsync(int[] values, object userState)
        {
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(userState);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (_userStateToLifetime.SyncRoot)
            {
                if (_userStateToLifetime.Contains(userState))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                _userStateToLifetime[userState] = asyncOp;
            }

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(CalculateWorker);
            workerDelegate.BeginInvoke(
                values,
                asyncOp,
                null,
                null);
        }
        
        // Check whether task is canceled
        private bool TaskCanceled(object taskId)
        {
            return (_userStateToLifetime[taskId] == null);
        }
        
        // Cancels operation by id
        public void CancelAsync(object userState)
        {

            AsyncOperation asyncOp = _userStateToLifetime[userState] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (_userStateToLifetime.SyncRoot)
                {
                    _userStateToLifetime.Remove(userState);
                }
            }
        }
       
        // Works on background
        private void CalculateWorker(int[] values, AsyncOperation asyncOp)
        {
            Exception e = null;
            var result = 0;

            // Check that the task is still active.
            // The operation may have been canceled before
            // the thread was scheduled.
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    var eventArgs = new ProgressChangedEventArgs(0, asyncOp.UserSuppliedState);
                    LongTaskProgressChanged(this, eventArgs);
                    for (var i = 0; i < values.Length; i++)
                    {
                        Thread.Sleep(1000);
                        result += values[i];
                        eventArgs = new ProgressChangedEventArgs((i + 1) * 100 / values.Length, asyncOp.UserSuppliedState);
                        LongTaskProgressChanged(this, eventArgs);
                    }
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            CompletionMethod(result, e, TaskCanceled(asyncOp.UserSuppliedState), asyncOp);
        }
        
        // Runs when operaton is completed
        private void CompletionMethod(int result, Exception exception, bool canceled, AsyncOperation asyncOp)
        {
            // If the task was not previously canceled,
            // remove the task from the lifetime collection.
            if (!canceled)
            {
                lock (_userStateToLifetime.SyncRoot)
                {
                    _userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }

            // Package the results of the operation
            LongTaskCompletedEventArgs e =
                new LongTaskCompletedEventArgs(
                result,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            this.LongTaskCompleted(this, e);
        }
        
        // Used to provide work on a backgraund thread
        private delegate void WorkerEventHandler(int[] values, AsyncOperation asyncOp);
        
        // Stores operation ids
        private readonly HybridDictionary _userStateToLifetime = new HybridDictionary();
    }
}
