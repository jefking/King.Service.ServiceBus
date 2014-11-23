namespace King.Service.ServiceBus.Unit.Tests.Models
{
    using System;

    public class BufferedMessage : IBufferedMessage
    {
        public virtual object Data
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