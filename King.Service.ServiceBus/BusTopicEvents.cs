namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Bus Topic Events
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class BusTopicEvents<T> : InitializeTask
    {
        #region Members
        /// <summary>
        /// Subscription Client
        /// </summary>
        protected readonly SubscriptionClient client;

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
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Connection</param>
        /// <param name="subscription">Subscription</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BusTopicEvents(string name, string connection, string subscription, IBusEventHandler<T> eventHandler, byte concurrentCalls = DefaultConcurrentCalls)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("connection");
            }
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentException("subscription");
            }
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }

            this.client = SubscriptionClient.CreateFromConnectionString(connection, name, subscription);
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

            this.client.OnMessageAsync(OnMessageArrived, eventDrivenMessagingOptions);
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
                Trace.TraceInformation("{0}: Message processed successfully from topic: {1}; with subscription: {2}.", this.eventHandler.GetType(), this.client.TopicPath, this.client.Name);
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0}: Message not processed successfully from topic: {1}; with subscription: {2}.", this.eventHandler.GetType(), this.client.TopicPath, this.client.Name));
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
