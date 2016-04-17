namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Azure.Data;

    /// <summary>
    /// Bus Queue Shards
    /// </summary>
    public class BusQueueShardSender : IAzureStorage
    {
        #region Members
        /// <summary>
        /// Shard Count
        /// </summary>
        protected readonly byte shardCount = 2;

        /// <summary>
        /// Base of the Name
        /// </summary>
        protected readonly string baseName;

        /// <summary>
        /// Service Bus Connection String
        /// </summary>
        protected readonly string connection;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Service Bus Connection String</param>
        /// <param name="shardCount">Shard Count</param>
        public BusQueueShardSender(string name, string connection, byte shardCount = 2)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("connection");
            }

            this.baseName = name;
            this.connection = connection;
            this.shardCount = shardCount > 0 ? shardCount : (byte)2;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.baseName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Queue Message
        /// </summary>
        /// <param name="obj">message</param>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Task</returns>
        public virtual async Task<bool> CreateIfNotExists()
        {
            var success = true;
            for (byte i = 0; i < this.shardCount; i++)
            {
                var q = new BusQueue(this.ShardName(i), this.connection);
                success &= await q.CreateIfNotExists();
            }

            return success;
        }

        /// <summary>
        /// Delete all queues
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Delete()
        {
            for (byte i = 0; i < this.shardCount; i++)
            {
                var q = new BusQueue(this.ShardName(i), this.connection);
                await q.Delete();
            }
        }

        /// <summary>
        /// Queue Message to shard, 0 means at random
        /// </summary>
        /// <param name="obj">message</param>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Task</returns>
        public virtual async Task Save(object obj, byte shardTarget = 0)
        {
            var q = new BusQueueSender(this.ShardName(shardTarget), this.connection);
            await q.Send(obj);
        }

        /// <summary>
        /// Determine index of queues to interact with
        /// </summary>
        /// <remarks>
        /// Specifically broken out for testing safety
        /// </remarks>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Index</returns>
        public virtual byte Index(byte shardTarget)
        {
            var random = new Random();
            var count = this.shardCount;
            return shardTarget == 0 || shardTarget > count ? (byte)random.Next(0, count) : shardTarget;
        }

        /// <summary>
        /// Shard Name
        /// </summary>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Name of Shard</returns>
        public virtual string ShardName(byte shardTarget)
        {
            if (0 > shardTarget || this.shardCount < shardTarget)
            {
                throw new ArgumentOutOfRangeException("Index out of Range.");
            }

            return string.Format("{0}{1}", this.baseName, shardTarget);
        }
        #endregion
    }
}