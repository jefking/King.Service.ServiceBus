namespace King.Service.ServiceBus
{
    using King.Service.Timing;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Bus Queue Events
    /// </summary>
    public class BusEvents<T> : RecurringTask
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly IBusQueueReciever queue = null;

        /// <summary>
        /// Bus Event Handler
        /// </summary>
        protected readonly IBusEventHandler<T> eventHandler = null;

        /// <summary>
        /// Maximum Concurrent Calls
        /// </summary>
        protected readonly byte concurrentCalls = DefaultConcurrentCalls;

        /// <summary>
        /// Default Concurrent Calls
        /// </summary>
        public const byte DefaultConcurrentCalls = 25;
        #endregion

        #region Constructors
        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BusEvents(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, byte concurrentCalls = DefaultConcurrentCalls)
            :base(BaseTimes.MinimumTiming, BaseTimes.NoRepeat)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }

            this.queue = queue;
            this.eventHandler = eventHandler;
            this.concurrentCalls = concurrentCalls <= 5 ? DefaultConcurrentCalls : concurrentCalls;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Maximum Concurrent Calls
        /// </summary>
        public virtual byte ConcurrentCalls
        {
            get
            {
                return this.concurrentCalls;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public override void Run()
        {
            var eventDrivenMessagingOptions = new OnMessageOptions
            {
                AutoComplete = true,
                MaxConcurrentCalls = concurrentCalls,
            };

            eventDrivenMessagingOptions.ExceptionReceived += OnExceptionReceived;

            this.queue.RegisterForEvents(OnMessageArrived, eventDrivenMessagingOptions);
        }

        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message">Brokered Message</param>
        /// <returns>Task</returns>
        public virtual async Task OnMessageArrived(BrokeredMessage message)
        {
            await Task.Factory.StartNew(this.MessageArrived, message.GetBody<T>());
        }

        /// <summary>
        /// Message Arrived, Background Thread
        /// </summary>
        /// <param name="body">Message Body</param>
        public virtual void MessageArrived(object body)
        {
            var success = this.eventHandler.Process((T)body).Result;
            if (success)
            {
                Trace.TraceInformation("{0}: Message processed successfully from queue: {1}.", this.eventHandler.GetType(), this.queue.Name);
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0}: Message not processed successfully from queue: {1}.", this.eventHandler.GetType(), this.queue.Name));
            }
        }

        /// <summary>
        /// Event handler for each time an error occurs.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        public virtual void OnExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                Trace.TraceError("'{0}' {1}", e.Action, e.Exception.ToString());

                this.eventHandler.OnError(e.Action, e.Exception);
            }
        }
        #endregion
    }
}