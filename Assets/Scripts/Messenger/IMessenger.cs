using System;

namespace Messenger
{
    public interface IMessenger
    {
        void Pub<T>(T message);
        void Sub<T>(Action<T> action);
        void Unsub<T>(Action<T> action);
        void UnsubAll();
    }
}