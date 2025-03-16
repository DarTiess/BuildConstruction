using System;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    internal class Subscription<T> : ISubscription
    {
        public List<CallbackWithPrecondition<T>> Callbacks { get; } = new();

        public void Add(Action<T> callback)
        {
            Callbacks.Add(new CallbackWithPrecondition<T>(callback));
        }

        public int CallbacksCount => Callbacks.Count;
    }

    internal interface ISubscription
    {
        int CallbacksCount { get; }
    }
}