namespace King.Service.ServiceBus
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Service Bus Event Processing
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class BusEvents<T> : InitializeTask
    {
        #region Members
        /// <summary>
        /// Event Source
        /// </summary>
        protected readonly IBusMessageReciever reciever = null;

        /// <summary>
        /// Bus Event Handler
        /// </summary>
        protected readonly IBusEventHandler<T> handler = null;

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
        /// <param name="reciever">Source</param>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="concurrentCalls">Concurrent Calls</param>
        public BusEvents(IBusMessageReciever reciever, IBusEventHandler<T> eventHandler, byte concurrentCalls = DefaultConcurrentCalls)
        {
            if (null == reciever)
            {
                throw new ArgumentNullException("source");
            }
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }

            this.reciever = reciever;
            this.handler = eventHandler;
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

            this.reciever.RegisterForEvents(OnMessageArrived, eventDrivenMessagingOptions);
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
            var success = await this.handler.Process(contents);
            if (success)
            {
                Trace.TraceInformation("{0}: Message processed successfully.", this.handler.GetType());
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0}: Message not processed successfully.", this.handler.GetType()));
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

                this.handler.OnError(e.Action, e.Exception);
            }
        }
        #endregion
    }
}