namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Topic Client Wrapper
    /// </summary>
    public class BusTopicClient : IBusTopicClient
    {
        #region Members
        /// <summary>
        /// Topic Client
        /// </summary>
        protected readonly ITopicClient client = null;

        /// <summary>
        /// Encoding Property Key
        /// </summary>
        public const string EncodingKey = "encoding";
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="conection">Conection</param>
        public BusTopicClient(string connection, string name)
            : this(new TopicClient(connection, name))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Topic Client</param>
        public BusTopicClient(ITopicClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Topic Client
        /// </summary>
        public virtual ITopicClient Client
        {
            get
            {
                return this.client;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Send(Message message)
        {
            await this.client.SendAsync(message);
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="message">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<Message> messages)
        {
            await this.client.SendAsync(messages.ToList());
        }
        #endregion
    }
}