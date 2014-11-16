namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using System;

    public interface IBusQueue
    {

    }

    public interface IBusDequeueBatch<T>
    {

    }

    public interface IBusDequeue<T>
    {

    }

    public interface IBusEventHandler<T> : IProcessor<T>
    {
        void OnError(string action, Exception ex);
    }
}