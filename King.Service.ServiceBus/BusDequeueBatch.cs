namespace King.Service.ServiceBus
{
    using King.Azure.Data;
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
        public BusDequeueBatch(IBusMessageReciever reciever, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(new BusQueuePoller<T>(reciever), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}