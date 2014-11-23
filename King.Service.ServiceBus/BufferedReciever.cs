namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Unit.Tests.Models;
    using King.Service.ServiceBus.Unit.Tests.Timing;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// <param name="concurrentCalls">Concurrent Calls (Default 10)</param>
        public BufferedReciever(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, byte concurrentCalls = 10)
            : this(queue, eventHandler, new Sleep(), concurrentCalls)
        {
        }

        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="sleep"></param>
        /// <param name="concurrentCalls">Concurrent Calls (Default 10)</param>
        public BufferedReciever(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, ISleep sleep, byte concurrentCalls = 10)
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
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override async Task OnMessageArrived(BrokeredMessage message)
        {
            var buffered = message.GetBody<IBufferedMessage>();

            this.sleep.Until(buffered.ReleaseAt);

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