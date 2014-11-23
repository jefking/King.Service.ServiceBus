namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using System;
    using System.Threading;

    public class Sleep : ISleep
    {
        #region Methods
        public void Until(DateTime time)
        {
            var duration = time.Subtract(DateTime.UtcNow);
            Thread.Sleep(duration);
        }
        #endregion
    }
}