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
                new System.Threading.ManualResetEvent(false).WaitOne(time.Subtract(DateTime.UtcNow.AddTicks(-1)));
                while (time > DateTime.UtcNow) { }
            }
        }
        #endregion
    }
}