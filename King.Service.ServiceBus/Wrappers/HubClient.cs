namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Event Hub Client
    /// </summary>
    public class HubClient : IHubClient
    {
        #region Members
        /// <summary>
        /// Event Hub Message Sender
        /// </summary>
        protected readonly MessageSender client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Event Hub Client</param>
        public HubClient(MessageSender client)
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
        /// Event Hub Client
        /// </summary>
        public MessageSender Client
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
            await this.client.SendAsync(message);
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="message">Messages</param>
        /// <returns>Task</returns>
        public async Task Send(IEnumerable<BrokeredMessage> messages)
        {
            await this.client.SendBatchAsync(messages);
        }
        #endregion
    }
}