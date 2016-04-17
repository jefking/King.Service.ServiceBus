namespace King.Service.ServiceBus.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Subscription Client
    /// </summary>
    public class BusSubscriptionClient : IBusReciever
    {
        #region Members
        /// <summary>
        /// Subscription Client
        /// </summary>
        protected readonly SubscriptionClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topicPath">Topic Name</param>
        /// <param name="connection">Connection String</param>
        /// <param name="name">Subscription Name</param>
        public BusSubscriptionClient(string topicPath, string connection, string name)
            : this(SubscriptionClient.CreateFromConnectionString(connection, topicPath, name))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        public BusSubscriptionClient(SubscriptionClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Methods
        /// <summary>
        /// On Message Async
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public void OnMessage(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessageAsync(callback, options);
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
        #endregion
    }
}