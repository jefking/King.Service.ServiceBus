namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using System;

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
        /// <param name="clqueueient">Queue</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatch(IBusQueueReciever queue, IProcessor<T> processor, int batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(new ServiceBusQueuePoller<T>(queue, TimeSpan.FromSeconds(minimumPeriodInSeconds)), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}