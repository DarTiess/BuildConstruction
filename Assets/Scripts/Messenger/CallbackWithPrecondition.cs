using System;

namespace Messenger
{
    internal class CallbackWithPrecondition<T>
    {
        public Action<T> Action { get; }
        public CallbackWithPrecondition(Action<T> action)
        {
            Action = action;
        }
    }
}