namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using King.Service.ServiceBus.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Poller, re-use for any queued models you have.
    /// </summary>
    /// <typeparam name="T">Message with T as Body</typeparam>
    public class BusPoller<T> : IPoller<T>
    {
        #region Members
        /// <summary>
        /// Reciever
        /// </summary>
        protected readonly IBusMessageReciever reciever = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reciever">Reciever</param>
        public BusPoller(IBusMessageReciever reciever)
        {
            if (null == reciever)
            {
                throw new ArgumentNullException("reciever");
            }

            this.reciever = reciever;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        public virtual async Task<IQueued<T>> Poll()
        {
            //var msg = await this.reciever.Get(this.reciever.ServerWaitTime);
            //return null == msg ? null : new Queued<T>(msg);
            return null;
        }

        /// <summary>
        /// Poll Many
        /// </summary>
        /// <param name="messageCount">Message Count</param>
        /// <returns>Queued Messages</returns>
        public virtual async Task<IEnumerable<IQueued<T>>> PollMany(int messageCount = 5)
        {
            //var msgs = await this.reciever.GetMany(this.reciever.ServerWaitTime, messageCount);
            //return null == msgs || !msgs.Any() ? null : msgs.Select(m => new Queued<T>(m));
            return null;
        }
        #endregion
    }
}