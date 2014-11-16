namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BusQueue : IBusQueue
    {
        #region Members
        protected readonly QueueClient client = null;

        protected readonly string name = null;

        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> CreateIfNotExists()
        {
            var created = false;
            if (!manager.QueueExists(name))
            {
                await manager.CreateQueueAsync(name);
                created = true;
            }

            return created;
        }

        /// <summary>
        /// Delete Queue
        /// </summary>
        /// <returns></returns>
        public virtual async Task Delete()
        {
            await manager.DeleteQueueAsync(this.name);
        }

        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        public virtual async Task<BrokeredMessage> Get()
        {
            return await this.client.ReceiveAsync();
        }

        /// <summary>
        /// Approixmate Message Count
        /// </summary>
        /// <returns>Message Count</returns>
        public virtual async Task<long> ApproixmateMessageCount()
        {
            var queue = await this.manager.GetQueueAsync(this.name);
            return queue.MessageCount;
        }

        /// <summary>
        /// Get Many Cloud Queue Message
        /// </summary>
        /// <returns>Messages</returns>
        public virtual async Task<IEnumerable<BrokeredMessage>> GetMany(int messageCount = 5)
        {
            if (0 > messageCount)
            {
                throw new ArgumentException("Message count must be greater than 0.");
            }

            return await this.client.ReceiveBatchAsync(messageCount);
        }

        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Save(BrokeredMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            await this.client.SendAsync(message);
        }

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        public virtual async Task Save(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is BrokeredMessage)
            {
                await this.Save(obj as BrokeredMessage);
            }
            else
            {
                await this.Save(new BrokeredMessage(JsonConvert.SerializeObject(obj)));
            }
        }
        #endregion
    }
}