namespace King.Service.ServiceBus.Unit.Tests.Models
{
    using System;

    public interface IBufferedMessage<T>
    {
        T Data
        {
            get;
        }
        DateTime ReleaseAt
        {
            get;
        }
    }
}