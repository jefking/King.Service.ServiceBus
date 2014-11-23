namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Bus Queue Events
    /// </summary>
    public class BusEvents<T> : InitializeTask
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
        protected readonly byte concurrentCalls = 10;
        #endregion

        #region Constructors
        /// <summary>
        /// Service Bus Queue Events
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls (Default 10)</param>
        public BusEvents(IBusQueueReciever queue, IBusEventHandler<T> eventHandler, byte concurrentCalls = 10)
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
            this.concurrentCalls = concurrentCalls < 1 ? (byte)10 : concurrentCalls;
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
        public virtual async Task OnMessageArrived(BrokeredMessage message)
        {
            var data = message.GetBody<T>();
            var success = await this.eventHandler.Process(data);
            if (success)
            {
                Trace.TraceInformation("Message processed successfully");
            }
            else
            {
                throw new InvalidOperationException("Message not processed");
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