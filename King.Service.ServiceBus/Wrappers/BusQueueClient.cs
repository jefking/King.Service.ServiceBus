namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        public QueueClient Client
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
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            await this.client.SendAsync(message);
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