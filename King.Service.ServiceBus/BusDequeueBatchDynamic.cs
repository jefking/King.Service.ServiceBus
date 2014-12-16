namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using Microsoft.ServiceBus;
    using System;

    /// <summary>
    /// Bus Dequeue Batch Dynamic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusDequeueBatchDynamic<T> : DequeueBatchDynamic<T>
    {
        #region Members
        /// <summary>
        /// Maximum batchsize = 32
        /// </summary>
        public const byte MaxBatchSize = 32;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="processor">Processor</param>
        /// <param name="batchCount">Batch Count</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatchDynamic(string name, string connectionString, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : this(new BusQueueReciever(name, connectionString), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="queue">Storage Queue</param>
        /// <param name="processor">Processor</param>
        /// <param name="batchCount">Batch Count</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatchDynamic(IBusQueueReciever queue, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : this(new ServiceBusQueuePoller<T>(queue, ServiceBusQueuePoller<T>.DefaultWaitTime), processor, new TimingTracker(queue.LockDuration().Result, MaxBatchSize), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="queue">Storage Queue</param>
        /// <param name="processor">Processor</param>
        /// <param name="batchCount">Batch Count</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatchDynamic(IPoller<T> queue, IProcessor<T> processor, ITimingTracker tracker, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(queue, processor, tracker, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}