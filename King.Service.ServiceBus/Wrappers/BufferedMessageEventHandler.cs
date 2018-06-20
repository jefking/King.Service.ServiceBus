﻿namespace King.Service.ServiceBus.Wrappers
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Timing;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Buffered Message Event Handler
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class BufferedMessageEventHandler<T> : IBusEventHandler<BufferedMessage>
    {
        #region Members
        /// <summary>
        /// Sleep
        /// </summary>
        protected readonly ISleep sleep = null;

        /// <summary>
        /// Bus Event Handler
        /// </summary>
        protected readonly IBusEventHandler<T> eventHandler = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventHandler">Event Handler</param>
        /// <param name="sleep">Sleep</param>
        public BufferedMessageEventHandler(IBusEventHandler<T> eventHandler, ISleep sleep)
        {
            if (null == eventHandler)
            {
                throw new ArgumentNullException("eventHandler");
            }
            if (null == sleep)
            {
                throw new ArgumentNullException("sleep");
            }

            this.eventHandler = eventHandler;
            this.sleep = sleep;
        }
        #endregion

        #region Methods
        /// <summary>
        /// On Error
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="ex">Exception</param>
        public async Task OnError(string action, Exception ex)
        {
            await this.eventHandler.OnError(action, ex);
        }

        /// <summary>
        /// Process Buffered Message
        /// </summary>
        /// <param name="buffered">Buffered</param>
        /// <returns>Successful</returns>
        public async Task<bool> Process(BufferedMessage buffered)
        {
            var obj = string.IsNullOrWhiteSpace(buffered.Data) ? default(T) : JsonConvert.DeserializeObject<T>(buffered.Data);

            Trace.TraceInformation("{1}: Message timing: {0} before scheduled release.", buffered.ReleaseAt.Subtract(DateTime.UtcNow), this.eventHandler.GetType());

            this.sleep.Until(buffered.ReleaseAt);

            Trace.TraceInformation("{1}: Message timing: {0} afer scheduled release.", DateTime.UtcNow.Subtract(buffered.ReleaseAt), this.eventHandler.GetType());

            return await this.eventHandler.Process(obj);
        }
        #endregion
    }
}