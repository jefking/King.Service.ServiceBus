namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Service Bus Queue Events
    /// </summary>
    public class BusEvents<T> : InitializeTask
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly QueueClient client = null;

        /// <summary>
        /// 
        /// </summary>
        protected readonly IBusEventHandler<T> eventHandler = null;

        /// <summary>
        /// Maximum Concurrent Calls
        /// </summary>
        protected readonly byte concurrentCalls = 10;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="eventHandler"></param>
        /// <param name="concurrentCalls"></param>
        public BusEvents(QueueClient client, IBusEventHandler<T> eventHandler, byte concurrentCalls = 10)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }

            this.client = client;
            this.eventHandler = eventHandler;
            this.concurrentCalls = concurrentCalls;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public override void Run()
        {
            // Build the messaging options.
            var eventDrivenMessagingOptions = new OnMessageOptions()
            {
                AutoComplete = true,
                MaxConcurrentCalls = concurrentCalls,
            };

            eventDrivenMessagingOptions.ExceptionReceived += OnExceptionReceived;

            // Subscribe for messages.
            this.client.OnMessage(OnMessageArrived, eventDrivenMessagingOptions);
        }

        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessageArrived(BrokeredMessage message)
        {
            var data = message.GetBody<T>();
            this.eventHandler.Process(data);
        }

        /// <summary>
        /// Event handler for each time an error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                this.eventHandler.OnError(e.Action, e.Exception);
            }
        }
        #endregion
    }
}