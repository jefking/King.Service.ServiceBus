namespace King.Service.ServiceBus.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Queue Client Wrapper
    /// </summary>
    public class BusQueueClient : IBusQueueClient
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly QueueClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Connetion</param>
        public BusQueueClient(string name, string connection)
            : this(QueueClient.CreateFromConnectionString(connection, name))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        public BusQueueClient(QueueClient client)
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
        /// Queue Client
        /// </summary>
        public virtual QueueClient Client
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
        public virtual async Task Send(BrokeredMessage message)
        {
            await this.client.SendAsync(message);
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="message">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<BrokeredMessage> messages)
        {
            await this.client.SendBatchAsync(messages);
        }

        /// <summary>
        /// Recieve
        /// </summary>
        /// <param name="serverWaitTime">Server Wait Time</param>
        /// <returns>Brokered Message</returns>
        public virtual async Task<BrokeredMessage> Recieve(TimeSpan serverWaitTime)
        {
            return await this.client.ReceiveAsync(serverWaitTime);
        }

        /// <summary>
        /// Recieve Batch
        /// </summary>
        /// <param name="messageCount">Message Count</param>
        /// <param name="serverWaitTime">Server Wait Time</param>
        /// <returns>Brokered Messages</returns>
        public virtual async Task<IEnumerable<BrokeredMessage>> RecieveBatch(int messageCount, TimeSpan serverWaitTime)
        {
            return await this.client.ReceiveBatchAsync(messageCount, serverWaitTime);
        }

        /// <summary>
        /// On Message
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public virtual void OnMessage(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessageAsync(callback, options);
        }
        #endregion
    }
}