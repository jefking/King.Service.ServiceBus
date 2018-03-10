﻿namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.Timing;

    /// <summary>
    /// Service Bus Dequeue Batch
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class BusDequeueBatch<T> : DequeueBatch<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reciever">Storage</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatch(IBusMessageReciever reciever, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new BusPoller<T>(reciever), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}