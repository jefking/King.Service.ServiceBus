namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using System;
    using System.Threading;

    /// <summary>
    /// Sleep
    /// </summary>
    public class Sleep : ISleep
    {
        #region Methods
        /// <summary>
        /// Until
        /// </summary>
        /// <param name="time">Time</param>
        public void Until(DateTime time)
        {
            var duration = time.Subtract(DateTime.UtcNow);
            Thread.Sleep(duration);
        }
        #endregion
    }
}