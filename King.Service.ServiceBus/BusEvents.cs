namespace King.Service.ServiceBus
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Service Bus Queue Events
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class BusEvents<T> : InitializeTask
    {
        #region Members
        /// <summary>
        /// Event Source
        /// </summary>
        protected readonly IBusEventReciever source = null;

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
        /// Service Bus Events
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BusEvents(IBusEventReciever source, IBusEventHandler<T> eventHandler, byte concurrentCalls = DefaultConcurrentCalls)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }

            this.source = source;
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
                MaxConcurrentCalls = concurrentCalls
            };

            eventDrivenMessagingOptions.ExceptionReceived += OnExceptionReceived;

            this.source.RegisterForEvents(OnMessageArrived, eventDrivenMessagingOptions);
        }

        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message">Brokered Message</param>
        /// <returns>Task</returns>
        public virtual async Task OnMessageArrived(BrokeredMessage message)
        {
            await this.Process(message.GetBody<T>());
        }

        /// <summary>
        /// Message Arrived, Background Thread
        /// </summary>
        /// <param name="contents">Message Body</param>
        public virtual async Task Process(T contents)
        {
            var success = await this.eventHandler.Process(contents);
            if (success)
            {
                Trace.TraceInformation("{0}: Message processed successfully from queue.", this.eventHandler.GetType());
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0}: Message not processed successfully from queue.", this.eventHandler.GetType()));
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