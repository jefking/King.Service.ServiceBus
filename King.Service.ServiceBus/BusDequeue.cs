namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
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
        /// <param name="reciever">Storage</param>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public BusDequeue(IBusMessageReciever reciever, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new BusPoller<T>(reciever), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}