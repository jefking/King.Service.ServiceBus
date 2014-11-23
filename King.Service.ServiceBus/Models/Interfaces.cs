namespace King.Service.ServiceBus.Unit.Tests.Models
{
    using System;

    public interface IBufferedMessage
    {
        object Data
        {
            get;
        }
        DateTime ReleaseAt
        {
            get;
        }
    }
}