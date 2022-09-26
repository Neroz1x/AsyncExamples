using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TAP.StateMachine;

namespace TAP.StateMachine
{
    internal sealed class StateMachineExampleAsync : IAsyncStateMachine
    {
        public int state;

        public AsyncTaskMethodBuilder<int> t__builder;

        public int param;

        private int firstTaskResult;

        private int s__2;

        private TaskAwaiter<int> u__1;

        private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter u__2;

        private TaskAwaiter u__3;


        private void MoveNext()
        {
            int num = state;
            int result;
            try
            {
                TaskAwaiter<int> awaiter3;
                ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter2;
                TaskAwaiter awaiter;
                switch (num)
                {
                    default:
                        Console.WriteLine("State Machine");
                        Console.WriteLine("Code before first await");
                        awaiter3 = Task.FromResult(param).GetAwaiter();
                        if (!awaiter3.IsCompleted)
                        {
                            num = (state = 0);
                            u__1 = awaiter3;
                            StateMachineExampleAsync stateMachine = this;
                            t__builder.AwaitUnsafeOnCompleted(ref awaiter3, ref stateMachine);
                            return;
                        }
                        goto IL_009b;
                    case 0:
                        awaiter3 = u__1;
                        u__1 = default(TaskAwaiter<int>);
                        num = (state = -1);
                        goto IL_009b;
                    case 1:
                        awaiter2 = u__2;
                        u__2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
                        num = (state = -1);
                        goto IL_0128;
                    case 2:
                        {
                            awaiter = u__3;
                            u__3 = default(TaskAwaiter);
                            num = (state = -1);
                            break;
                        }
                    IL_0128:
                        awaiter2.GetResult();
                        Console.WriteLine("Code after the second await");
                        awaiter = Task.Delay(3000).GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            num = (state = 2);
                            u__3 = awaiter;
                            StateMachineExampleAsync stateMachine = this;
                            t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                            return;
                        }
                        break;
                    IL_009b:
                        s__2 = awaiter3.GetResult();
                        firstTaskResult = s__2;
                        Console.WriteLine("Code after the first await");
                        awaiter2 = Task.Delay(2000).ConfigureAwait(false).GetAwaiter();
                        if (!awaiter2.IsCompleted)
                        {
                            num = (state = 1);
                            u__2 = awaiter2;
                            StateMachineExampleAsync stateMachine = this;
                            t__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
                            return;
                        }
                        goto IL_0128;
                }
                awaiter.GetResult();
                Console.WriteLine("Code after the third await");
                Console.WriteLine(firstTaskResult);
                Console.WriteLine();
                result = 1;
            }
            catch (Exception exception)
            {
                state = -2;
                t__builder.SetException(exception);
                return;
            }
            state = -2;
            t__builder.SetResult(result);
        }

        void IAsyncStateMachine.MoveNext()
        {
            //ILSpy generated this explicit interface implementation from .override directive in MoveNext
            this.MoveNext();
        }

        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
            //ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
            this.SetStateMachine(stateMachine);
        }
    }
}

internal sealed class StateMachineTester
{
    private async Task StateMachineExampleAsync()
    {
        await StateMachineExample.StateMachineExampleAsync(1);
    }

    // That's how this method above will look like:
    [AsyncStateMachine(typeof(StateMachineExampleAsync))]
    public static Task<int> StateMachineExampleAsync(int param)
    {
        StateMachineExampleAsync stateMachine = new StateMachineExampleAsync();
        stateMachine.t__builder = AsyncTaskMethodBuilder<int>.Create();
        stateMachine.param = param;
        stateMachine.state = -1;
        stateMachine.t__builder.Start(ref stateMachine);
        return stateMachine.t__builder.Task;
    }
}

