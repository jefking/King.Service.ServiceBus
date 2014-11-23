namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using System;

    public interface ISleep
    {
        void Until(DateTime time);
    }
}