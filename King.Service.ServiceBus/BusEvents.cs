namespace King.Service.ServiceBus
{
    using Microsoft.Azure.ServiceBus;
    using King.Service.ServiceBus.Models;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

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
        protected readonly IBusReciever reciever = null;

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
        public BusEvents(IBusReciever reciever, IBusEventHandler<T> eventHandler, byte concurrentCalls = DefaultConcurrentCalls)
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
            var options = new MessageHandlerOptions(this.OnExceptionReceived)
            {
                AutoComplete = true,
                MaxConcurrentCalls = concurrentCalls,
            };

            this.reciever.OnMessage(OnMessageArrived, options);
        }

        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message">Brokered Message</param>
        /// <param name="cancel">Cancellation Token</param>
        /// <returns>Task</returns>
        public virtual async Task OnMessageArrived(Message message, CancellationToken cancel)
        {
            var queued = new Queued<T>(message);
            var data = await queued.Data();
            await this.Process(data);
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
        /// <param name="e">Arguments</param>
        public virtual async Task OnExceptionReceived(ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                Trace.TraceError("'{0}' {1}", e.ExceptionReceivedContext.Action, e.Exception.ToString());

                await this.handler.OnError(e.ExceptionReceivedContext.Action, e.Exception);
            }
        }
        #endregion
    }
}