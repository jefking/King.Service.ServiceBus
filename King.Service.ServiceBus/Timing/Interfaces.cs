namespace King.Service.ServiceBus.Timing
{
    using System;

    #region ISleep
    /// <summary>
    /// Sleep Interface
    /// </summary>
    public interface ISleep
    {
        #region Methods
        /// <summary>
        /// Until
        /// </summary>
        /// <param name="time">Time</param>
        void Until(DateTime time);
        #endregion
    }
    #endregion
}