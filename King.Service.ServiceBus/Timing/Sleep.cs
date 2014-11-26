namespace King.Service.ServiceBus.Timing
{
    using System;
    using System.Diagnostics;

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
            if (time > DateTime.UtcNow)
            {
                var duration = time.Subtract(DateTime.UtcNow);
                Trace.TraceInformation("Sleeping for: {0}.", duration);
                new System.Threading.ManualResetEvent(false).WaitOne(time.Subtract(DateTime.UtcNow));
            }
        }
        #endregion
    }
}