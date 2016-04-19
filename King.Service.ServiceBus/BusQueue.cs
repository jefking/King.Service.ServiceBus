namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
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
        protected readonly IBusQueueClient client = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;

        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueue(string name, string connectionString)
            : this(name, NamespaceManager.CreateFromConnectionString(connectionString), new BusQueueClient(QueueClient.CreateFromConnectionString(connectionString, name)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client></param>
        public BusQueue(string name, NamespaceManager manager, IBusQueueClient client)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            this.name = name;
            this.manager = manager;
            this.client = client;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Queue Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Queue Client
        /// </summary>
        public virtual IBusQueueClient Client
        {
            get
            {
                return this.client;
            }
        }

        /// <summary>
        /// Namespace Manager
        /// </summary>
        public virtual NamespaceManager Manager
        {
            get
            {
                return this.manager;
            }
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
                var qd = new QueueDescription(name)
                {
                    EnablePartitioning = true,
                };

                await manager.CreateQueueAsync(qd);
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
        public virtual async Task<long?> ApproixmateMessageCount()
        {
            var queue = await this.Description();
            return queue.MessageCount;
        }

        /// <summary>
        /// Lock Duration
        /// </summary>
        /// <returns>Lock Duration</returns>
        public virtual async Task<TimeSpan> LockDuration()
        {
            var queue = await this.Description();
            return queue.LockDuration;
        }

        /// <summary>
        /// Description
        /// </summary>
        /// <returns>Queue Description</returns>
        public virtual async Task<QueueDescription> Description()
        {
            return await this.manager.GetQueueAsync(this.name);
        }
        #endregion
    }
}