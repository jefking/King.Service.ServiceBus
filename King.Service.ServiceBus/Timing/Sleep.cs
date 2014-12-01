namespace King.Service.ServiceBus.Timing
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
            if (time > DateTime.UtcNow)
            {
                var reset = new ManualResetEvent(false);
                reset.WaitOne(time.Subtract(DateTime.UtcNow));
                
                while (time >= DateTime.UtcNow) { } //Ensure release is made after specified timing
            }
        }
        #endregion
    }
}