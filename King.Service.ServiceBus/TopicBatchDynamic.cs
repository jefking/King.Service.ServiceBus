namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using Service.Timing;
    using Timing;

    /// <summary>
    /// Bus Dequeue from Topic Batch Dynamic
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class TopicBatchDynamic<T> : DequeueBatchDynamic<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="subscriptionName"></param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public TopicBatchDynamic(string name, string connectionString, string subscriptionName, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new BusPoller<T>(new BusSubscriptionReciever(name, connectionString, subscriptionName)), processor, new TopicTimingTracker(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}