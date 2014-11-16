namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Service Bus Dequeue Batch
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class BusDequeueBatch<T> : DequeueBatch<T>, IBusDequeueBatch<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeueBatch(QueueClient client, IProcessor<T> processor, int batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(new ServiceBusQueuePoller<T>(client), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}