namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Timing;
    using King.Service.ServiceBus.Wrappers;

    /// <summary>
    /// Buffered Reciever
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class BufferedReciever<T> : BusEvents<BufferedMessage>
    {
        #region Constructors
        /// <summary>
        /// Service Bus Events
        /// </summary>
        /// <param name="reciever">Queu/Topic</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BufferedReciever(IBusReciever reciever, IBusEventHandler<T> eventHandler, byte concurrentCalls = BusEvents<T>.DefaultConcurrentCalls)
            : base(reciever, new BufferedMessageEventHandler<T>(eventHandler, new Sleep()), concurrentCalls)
        {
        }
        #endregion
    }
}