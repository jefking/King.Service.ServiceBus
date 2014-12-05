namespace King.Service.ServiceBus.Wrappers
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

        public void OnError(string action, Exception ex)
        {
            this.eventHandler.OnError(action, ex);
        }

        public async Task<bool> Process(BufferedMessage buffered)
        {
            var obj = JsonConvert.DeserializeObject<T>(buffered.Data);

            Trace.TraceInformation("Message timing: {0} before scheduled release.", buffered.ReleaseAt.Subtract(DateTime.UtcNow));

            this.sleep.Until(buffered.ReleaseAt);

            Trace.TraceInformation("Message timing: {0} afer scheduled release.", DateTime.UtcNow.Subtract(buffered.ReleaseAt));

            return await this.eventHandler.Process(obj);
        }
    }
}
