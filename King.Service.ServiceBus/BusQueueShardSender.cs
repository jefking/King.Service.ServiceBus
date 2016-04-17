namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Shard Sender
    /// </summary>
    public class BusQueueShardSender : IBusShardSender
    {
        #region Members
        /// <summary>
        /// Queues
        /// </summary>
        protected readonly IEnumerable<IBusShard> queues;

        /// <summary>
        /// Base of the Name
        /// </summary>
        protected readonly string baseName;

        /// <summary>
        /// Default Shard Count
        /// </summary>
        public const byte DefaultShardCount = 2;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Connection</param>
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
            shardCount = shardCount > 0 ? shardCount : DefaultShardCount;

            var qs = new IBusShard[shardCount];
            for (var i = 0; i < shardCount; i++)
            {
                var n = string.Format("{0}{1}", this.baseName, i);
                var r = new BusQueue(n, connection);
                var s = new BusQueueSender(n, connection);
                qs[i] = new BusShard(r, s);
            }

            this.queues = new ReadOnlyCollection<IBusShard>(qs);
        }

        /// <summary>
        /// Constructor for mocking
        /// </summary>
        /// <param name="queues">Queues</param>
        public BusQueueShardSender(IEnumerable<IBusShard> queues)
        {
            if (null == queues)
            {
                throw new ArgumentNullException("queue");
            }
            if (0 == queues.Count())
            {
                throw new ArgumentException("Queues length is 0.");
            }

            this.queues = queues;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Queue Shards
        /// </summary>
        public virtual IReadOnlyCollection<IBusShard> Queues
        {
            get
            {
                return new ReadOnlyCollection<IBusShard>(this.queues.ToList());
            }
        }

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

        /// <summary>
        /// Shard Count
        /// </summary>
        public byte ShardCount
        {
            get
            {
                return (byte)this.queues.Count();
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
            foreach (var q in this.queues)
            {
                success &= await q.Resource.CreateIfNotExists();
            }

            return success;
        }

        /// <summary>
        /// Delete all queues
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Delete()
        {
            foreach (var q in this.queues)
            {
                await q.Resource.Delete();
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
            var index = this.Index(shardTarget);
            var q = this.queues.ElementAt(index);
            await q.Sender.Send(obj);
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
            var count = this.queues.Count();
            return shardTarget == 0 || shardTarget > count ? (byte)random.Next(0, count) : shardTarget;
        }
        #endregion
    }
}