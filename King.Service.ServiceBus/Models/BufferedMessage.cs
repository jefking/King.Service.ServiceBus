namespace King.Service.ServiceBus.Unit.Tests.Models
{
    using System;

    public class BufferedMessage<T> : IBufferedMessage<T>
    {
        public virtual T Data
        {
            get;
            set;
        }

        public virtual DateTime ReleaseAt
        {
            get;
            set;
        }
    }
}