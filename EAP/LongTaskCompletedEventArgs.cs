using System;
using System.ComponentModel;

namespace EAP
{
    public class LongTaskCompletedEventArgs : AsyncCompletedEventArgs
    {
        private int _result;

        public LongTaskCompletedEventArgs(
            int result,
            Exception e,
            bool canceled,
            object state) : base(e, canceled, state)
        {
            _result = result;
        }

        public int Result
        {
            get
            {
                // Raise an exception if the operation failed or
                // was canceled.
                RaiseExceptionIfNecessary();

                // If the operation was successful, return the
                // property value.
                return _result;
            }
        }
    }
}
