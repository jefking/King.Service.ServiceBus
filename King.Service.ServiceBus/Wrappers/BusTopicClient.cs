namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Topic Client
    /// </summary>
    public class BusTopicClient : IBusTopicClient
    {
        #region Members
        /// <summary>
        /// Topic Client
        /// </summary>
        protected readonly TopicClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Topic Client</param>
        public BusTopicClient(TopicClient client)
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
        public TopicClient Client
        {
            get
            {
                return this.client;
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task Send(BrokeredMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            await this.client.SendAsync(message);
        }
        #endregion
    }
}