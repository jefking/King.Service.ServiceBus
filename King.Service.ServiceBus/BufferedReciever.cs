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
        #region Members
        /// <summary>
        /// Default Concurrent Calls
        /// </summary>
        public new const byte DefaultConcurrentCalls = 50;
        #endregion

        #region Constructors
        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="sleep"></param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BufferedReciever(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, byte concurrentCalls = BufferedReciever<T>.DefaultConcurrentCalls)
            : base(queue, new BufferedMessageEventHandler<T>(eventHandler, new Sleep()), concurrentCalls)
        {
        }
        #endregion
    }
}