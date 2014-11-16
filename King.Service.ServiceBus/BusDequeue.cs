namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Service Bus Dequeue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusDequeue<T> : Dequeue<T>, IBusDequeue<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeue(QueueClient client, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(new ServiceBusQueuePoller<T>(client), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}