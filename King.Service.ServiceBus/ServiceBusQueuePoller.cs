namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.ServiceBus.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Poller, re-use for any queued models you have.
    /// </summary>
    /// <typeparam name="T">Message with T as Body</typeparam>
    public class ServiceBusQueuePoller<T> : IPoller<T>
    {
        #region Members
        /// <summary>
        /// Queue
        /// </summary>
        protected readonly IBusQueueReciever queue = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queue">Queue</param>
        public ServiceBusQueuePoller(IBusQueueReciever queue)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }

            this.queue = queue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        public virtual async Task<IQueued<T>> Poll()
        {
            var msg = await this.queue.Get();
            return null == msg ? null : new Queued<T>(msg);
        }

        /// <summary>
        /// Poll Many
        /// </summary>
        /// <param name="messageCount">Message Count</param>
        /// <returns>Queued Messages</returns>
        public virtual async Task<IEnumerable<IQueued<T>>> PollMany(int messageCount = 5)
        {
            var msgs = await this.queue.GetMany(messageCount);
            return null == msgs || !msgs.Any() ? null : msgs.Select(m => new Queued<T>(m));
        }
        #endregion
    }
}