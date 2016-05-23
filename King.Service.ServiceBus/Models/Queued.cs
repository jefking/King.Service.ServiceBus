namespace King.Service.ServiceBus.Models
{
    using System;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using Wrappers;

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

        /// <summary>
        /// Cached Copy of Message Data
        /// </summary>
        protected T cache = default(T);

        /// <summary>
        /// Cached
        /// </summary>
        protected bool cached = false;
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
        public virtual async Task Abandon()
        {
            await this.message.AbandonAsync();
        }

        /// <summary>
        /// Data
        /// </summary>
        /// <returns>Data</returns>
        public virtual Task<T> Data()
        {
            if (!cached)
            {
                var p = this.message.Properties;

                if (p.ContainsKey(BusQueueClient.EncodingKey)
                    && (Encoding)p[BusQueueClient.EncodingKey] == Encoding.Json)
                {
                    var raw = this.message.GetBody<string>();
                    cache = JsonConvert.DeserializeObject<T>(raw);
                }
                else
                {
                    cache = this.message.GetBody<T>();
                }

                cached = true;
            }

            return Task.FromResult(cache);
        }

        /// <summary> 
        /// Complete
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Complete()
        {
            await this.message.CompleteAsync();
        }
        #endregion
    }
}