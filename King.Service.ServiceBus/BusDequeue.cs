﻿namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;

    /// <summary>
    /// Service Bus Dequeue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusDequeue<T> : Dequeue<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeue(IBusQueueReciever queue, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(new BusQueuePoller<T>(queue, TimeSpan.FromSeconds(minimumPeriodInSeconds)), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}