namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BusQueueClient : IBusQueueClient
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly QueueClient client = null;
        #endregion

        #region Constructors
        public BusQueueClient(QueueClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Methods
        public virtual async Task Send(BrokeredMessage message)
        {
            await this.client.SendAsync(message);
        }

        public virtual async Task<BrokeredMessage> Receive(TimeSpan serverWaitTime)
        {
            return await this.client.ReceiveAsync(serverWaitTime);
        }

        public virtual async Task<IEnumerable<BrokeredMessage>> ReceiveBatch(int messageCount, TimeSpan serverWaitTime)
        {
            return await this.client.ReceiveBatchAsync(messageCount, serverWaitTime);
        }

        public virtual void OnMessage(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessageAsync(callback, options);
        }
        #endregion
    }
}