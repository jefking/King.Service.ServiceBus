namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Wrapper for Brokered Messages
    /// </summary>
    /// <typeparam name="T">Type, Serialized in Message Body</typeparam>
    public class Queued<T> : IQueued<T>
    {
        #region Members
        /// <summary>
        /// Brokered Message
        /// </summary>
        protected readonly BrokeredMessage message = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">Message</param>
        public Queued(BrokeredMessage msg)
        {
            if (null == msg)
            {
                throw new ArgumentNullException("msg");
            }

            this.message = msg;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Abandon Message
        /// </summary>
        /// <returns>Task</returns>
        public async Task Abandon()
        {
            await this.message.AbandonAsync();
        }

        /// <summary>
        /// Data
        /// </summary>
        /// <returns>Data</returns>
        public Task<T> Data()
        {
            return Task.FromResult(this.message.GetBody<T>());
        }

        /// <summary>
        /// Complete
        /// </summary>
        /// <returns>Task</returns>
        public async Task Complete()
        {
            await this.message.CompleteAsync();
        }
        #endregion
    }
}