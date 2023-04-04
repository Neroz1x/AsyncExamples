using System;
using System.Runtime.CompilerServices;

namespace TAP.CustomAwaitable
{
    class MyTaskMethodBuilder<T>
    {
        public static MyTaskMethodBuilder<T> Create() => throw new NotImplementedException();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
            => throw new NotImplementedException();

        public void SetStateMachine(IAsyncStateMachine stateMachine)
            => throw new NotImplementedException();
        public void SetException(Exception exception)
            => throw new NotImplementedException();
        public void SetResult(T result)
            => throw new NotImplementedException();

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => throw new NotImplementedException();
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => throw new NotImplementedException();

        public CustomTask Task { get; }
    }
}
