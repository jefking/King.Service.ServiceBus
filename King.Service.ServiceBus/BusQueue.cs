namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue
    /// </summary>
    public class BusQueue : IBusQueue
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly QueueClient client = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;

        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;

        /// <summary>
        /// Transient Error Event
        /// </summary>
        public event EventHandler<MessagingException> TransientErrorOccured;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueue(string name, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            this.name = name;
            this.manager = NamespaceManager.CreateFromConnectionString(connectionString);
            this.client = QueueClient.CreateFromConnectionString(connectionString, name);
        }
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
        /// Approixmate Message Count
        /// </summary>
        /// <returns>Message Count</returns>
        public virtual async Task<long> ApproixmateMessageCount()
        {
            var queue = await this.manager.GetQueueAsync(this.name);
            return queue.MessageCount;
        }

        /// <summary>
        /// Handle Transient Error
        /// </summary>
        /// <param name="ex">Messaging Exception</param>
        public virtual void HandleTransientError(MessagingException ex)
        {
            if (null != ex)
            {
                var handle = this.TransientErrorOccured;
                if (null != handle)
                {
                    handle(this, ex);
                }

                Trace.TraceWarning("Transient Error: '{0}'", ex.ToString());
            }
        }
        #endregion
    }
}