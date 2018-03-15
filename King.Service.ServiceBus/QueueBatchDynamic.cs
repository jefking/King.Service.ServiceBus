namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using Microsoft.Azure.ServiceBus;
    using Service.Timing;
    using Timing;

    /// <summary>
    /// Bus Dequeue from Queue Batch Dynamic
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class QueueBatchDynamic<T> : DequeueBatchDynamic<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public QueueBatchDynamic(string name, string connectionString, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new BusPoller<T>(new BusQueueReciever(name, connectionString)), processor, new QueueTimingTracker(new QueueClient(name, connectionString)), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}