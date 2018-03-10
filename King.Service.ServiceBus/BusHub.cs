namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Bus Hub
    /// </summary>
    public class BusHub : IAzureStorage
    {
        #region Members
        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;

        /// <summary>
        /// partition count
        /// </summary>
        protected readonly int partitionCount = 16;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Hub Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="partitionCount"></param>
        public BusHub(string name, string connectionString, int partitionCount = 16)
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
            this.partitionCount = partitionCount < 8 ? 8 : partitionCount > 32 ? 32 : partitionCount;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Number of partitons to create the Event Hub With
        /// </summary>
        public virtual int PartitionCount
        {
            get
            {
                return this.partitionCount;
            }
        }

        /// <summary>
        /// Hub Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns>Created</returns>
        public virtual async Task<bool> CreateIfNotExists()
        {
            var description = new EventHubDescription(name)
            {
                PartitionCount = this.partitionCount,
            };

            var d = await this.manager.CreateEventHubIfNotExistsAsync(description);
            return d.Status == EntityStatus.Creating;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Delete()
        {
            await this.manager.DeleteEventHubAsync(this.name);
        }
        #endregion
    }
}
