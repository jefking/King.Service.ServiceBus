﻿namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
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
        /// Default Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Connetion</param>
        public BusTopicClient(string name, string connection)
            : this(TopicClient.CreateFromConnectionString(connection, name))
        {
        }

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