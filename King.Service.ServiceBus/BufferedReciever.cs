namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Unit.Tests.Models;
    using King.Service.ServiceBus.Unit.Tests.Timing;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Buffered Reciever
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class BufferedReciever<T> : BusEvents<T>
    {
        #region Members
        /// <summary>
        /// Sleep
        /// </summary>
        protected readonly ISleep sleep = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BufferedReciever(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, byte concurrentCalls = BusEvents<T>.DefaultConcurrentCalls)
            : this(queue, eventHandler, new Sleep(), concurrentCalls)
        {
        }

        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="sleep"></param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BufferedReciever(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, ISleep sleep, byte concurrentCalls = BusEvents<T>.DefaultConcurrentCalls)
            : base(queue, eventHandler, concurrentCalls)
        {
            if (null == sleep)
            {
                throw new ArgumentNullException("sleep");
            }

            this.sleep = sleep;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message">Brokered Message</param>
        /// <returns>Task</returns>
        public override async Task OnMessageArrived(BrokeredMessage message)
        {
            var buffered = message.GetBody<BufferedMessage>();

            this.sleep.Until(buffered.ReleaseAt);

            Trace.TraceInformation("Message Released at: {0}; should be: {1}.", DateTime.UtcNow, buffered.ReleaseAt);

            var success = await this.eventHandler.Process((T)buffered.Data);
            if (success)
            {
                Trace.TraceInformation("Message processed successfully");
            }
            else
            {
                throw new InvalidOperationException("Message not processed");
            }
        }
        #endregion
    }
}